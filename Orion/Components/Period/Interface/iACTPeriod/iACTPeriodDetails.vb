Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no 129
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: {TodaysDate}
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
    ' which class this is.
    'Developer Guide no. 129
    Dim frmDetails As frmDetails
    Dim prevVal As Object
	Private Const ACClass As String = "frmDetails"
    Private Const vbFormCode As Integer = 0
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
    Public lastperiodname As Object
    Public lastperioddate As Object
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lPeriodID As Integer
	Private m_sYearName As New FixedLengthString(20)
	Private m_sPeriodName As New FixedLengthString(15)
	Private m_dtPeriodEndDate As Date
	Public SubBranchID As Integer
	
	' CTAF 200100
	Private m_sOldYearName As String = ""
	Private m_bYearChanged As String = ""
	
	Private m_bPreventGridError As Boolean
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGridGeneral As iACTPeriod.GridGeneral
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	
	Public Property YearChanged() As Boolean
		Get
			Return CBool(m_bYearChanged)
		End Get
		Set(ByVal Value As Boolean)
			m_bYearChanged = CStr(Value)
		End Set
	End Property
	
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	

	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	
	Public Property YearName() As String
		Get
			
			' Return the year name
			Return m_sYearName.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the year name
			m_sYearName.Value = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise(ByRef oBusiness As Object) As Integer
		
        Dim result As Integer = 0
       
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the instance of the business object
			' into the memberfor use by lookup
			m_oBusiness = oBusiness
			
			' Create an instance of the general interface object.
			m_oGridGeneral = New iACTPeriod.GridGeneral()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGridGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: Load (Standard Method)
	'
	' Description: Load Interface defaults and get details replaces
	'              form load event
	'
	' ***************************************************************** '
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Try 

			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' This normally done after SetInterfaceDefaults
			' but in this case we need to do first or we miss
			' the invisible populate grid event
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGridGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the year name panel from the property
			m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If

			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'grdMainData.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            'grdMainData.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable

			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: ShowForm (Standard Method)
	'
	' Description: Show the form
	'
	' ***************************************************************** '
	Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Show the the form, allow user input etc.
			VB6.ShowForm(Me, lDisplayState)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
        Try
 
            result = gPMConstants.PMEReturnCode.PMTrue
            'Take the value of oBusiness through obj_oBusiness,checked at run time


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vYearName:=m_sYearName.Value, vSubBranchID:=SubBranchID)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Public Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage to be
			' used with the grid.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Initialise the data array (Allow one extra field
			' for the unique key to been assigned).
			ReDim g_vGridData(3, 0)
			
			' Retrieve all of the details from the business object.

			While m_oBusiness.GetNext(vPeriodID:=m_lPeriodID, vCompanyID:=g_iSourceID, vYearName:=m_sYearName.Value, vPeriodName:=m_sPeriodName.Value, vPeriodEndDate:=m_dtPeriodEndDate) = gPMConstants.PMEReturnCode.PMTrue
				
				' Company ID and Year Name remain constant for all periods in year
				' so no need to store in the array
				' Store all of the displayable data.


				g_vGridData(ACSubPeriodName, g_vGridData.GetUpperBound(1)) = m_sPeriodName.Value


				g_vGridData(ACSubPeriodEndDate, g_vGridData.GetUpperBound(1)) = m_dtPeriodEndDate
				
				' Store all of the non-displayable data needed.


				g_vGridData(ACSubPeriodID, g_vGridData.GetUpperBound(1)) = m_lPeriodID
				
				' {* USER DEFINED CODE (End) *}
				
				' Store unique key for this row.


                g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1)) = g_vGridData.GetUpperBound(1) + 1
				
				' Increment the data array.

				ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) + 1)
			End While
			
			' Check if we have data in the grid array.
			If Information.IsArray(g_vGridData) Then

				If g_vGridData.GetUpperBound(1) > 0 Then
					' Decrement the data array.

					ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) - 1)
				End If
				
				' Store the new index value, for when we wish start
				' adding data.

				m_oGridGeneral.NewIndex = g_vGridData.GetUpperBound(1) + 1
				
				' Update the approxamate number of rows.

                'modified as per R&D done.
                'grdMainData.ApproxCount = g_vGridData.GetUpperBound(1)


			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToBusiness
	'
	' Description: Updates all business members from the data storage.
	'
	' ***************************************************************** '
	Public Function DataToBusiness(ByRef lMode As Integer, ByRef lDataID As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		Dim sTemp As String = ""
		
		Const AC_TIME As String = " 23:59:59"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details from the data storage to
			' the business object.
			' Store all of the displayable data.

            m_sPeriodName.Value = CStr(g_vGridData(ACSubPeriodName, lDataID))


            If Information.IsDate(g_vGridData(ACSubPeriodEndDate, lDataID)) Then


                sTemp = DateTime.Parse(g_vGridData(ACSubPeriodEndDate, lDataID)).ToString("d") & AC_TIME

                'If (Right$(sTemp, Len(AC_TIME)) <> AC_TIME) Then
                '    sTemp = sTemp & AC_TIME
                'End If

                'm_dtPeriodEndDate = g_vGridData(ACSubPeriodEndDate, lDataID&)

                m_dtPeriodEndDate = CDate(sTemp)

            End If

            ' Store all of the non-displayable data needed.

            'BB Period ID not changeable and we need current ID for new entries
            ' so use what's already in m_lPeriodID from the last GetNext
            ' m_lPeriodID = g_vGridData(ACSubPeriodID, lDataID&)

            ' Store unique key for this row.


            lBusinessDataID = CInt(Conversion.Val(CStr(g_vGridData(g_vGridData.GetUpperBound(0), lDataID))))

            ' Check the task.
            Select Case (lMode)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPeriodID:=m_lPeriodID, vCompanyID:=g_iSourceID, vSubBranchID:=SubBranchID, vYearName:=m_sYearName.Value, vPeriodName:=m_sPeriodName.Value, vPeriodEndDate:=m_dtPeriodEndDate, vPeriodEndComplete:=0)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPeriodID:=m_lPeriodID, vCompanyID:=g_iSourceID, vSubBranchID:=SubBranchID, vYearName:=m_sYearName.Value, vPeriodName:=m_sPeriodName.Value, vPeriodEndDate:=m_dtPeriodEndDate)
                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Inform the business object with a deleted data item.

                    m_lReturn = m_oBusiness.EditDelete(lBusinessDataID)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the data details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        'Dim oValueItems As TrueDBGrid.ValueItems

        Try


            ' Get the lookup values.

            '    m_lReturn& = GetLookupValues()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    Set oValueItems = GrdMainData.Columns(2).ValueItems
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        oValueItems:=oValueItems)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetGridInterfaceDefaults
    '
    ' Description: Sets all of the grid default values.
    '
    ' ***************************************************************** '
    Public Function SetGridInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Assign the grid default values, if any.
            With grdMainData


                Dim i As Integer = 0
                For i = 0 To UBound(g_vGridData, 2)

                    .Rows.Add(g_vGridData(0, i), CDate(g_vGridData(1, i)).ToString("dd MMMM yyyy"))
                Next i

                '        .Columns(0).DefaultValue = .Columns(0).ValueItems(0).Value

                'TODOLIST:
                '.get_Columns(ACSubPeriodName).DefaultValue = m_sPeriodName.Value

                '.Columns(ACSubPeriodName).HeaderText = m_sPeriodName.Value
                lastperiodname = m_sPeriodName.Value
                'TODOLIST:
                '.get_Columns(ACSubPeriodEndDate).DefaultValue = DateTimeHelper.ToString(m_dtPeriodEndDate)

                '.Columns(ACSubPeriodEndDate).HeaderText = m_dtPeriodEndDate
                lastperioddate = m_dtPeriodEndDate
            End With

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the grid defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGridInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates all interface details from the Form properties
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            'panYearName.Caption = m_sYearName$

            ' CTAF 200100 - Changed so user can edit year name
            txtYearName.Text = m_sYearName.Value.Trim()

            m_sOldYearName = m_sYearName.Value

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Set grid to view mode.
                'TODOLIST:
                'start
                'grdMainData.AllowAddNew = False
                grdMainData.AllowUserToAddRows = False
                'grdMainData.AllowUpdate = False
                grdMainData.AllowUserToResizeRows = False
                'grdMainData.AllowDelete = False
                grdMainData.AllowUserToDeleteRows = False
                grdMainData.AllowUserToOrderColumns = False
                'end
            End If

            ' Set any other default values to the interface.
            'TODOLIST:
            'grdMainData.AllowAddNew = True

            'TR - 15/05/03 - Issue3451 - Removed the Default from cmdOk as grid
            'events were not happening in correct order when user hits Enter
            'button to leve screen after editing a cell
            '    cmdOK.Default = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    GrdMainData.Columns(0).Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACGridCaption1, _
            ''        iDataType:=PMResString)
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '

    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.
    '
    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.
    '
    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef oValueItems As TrueDBGrid.ValueItems) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    'Dim oValueItem As New TrueDBGrid.ValueItem
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the object.
    '
    'oValueItem.Value = m_vLookupDetails(ACDetailKey, lCntr)

    'oValueItem.DisplayValue = m_vLookupDetails(ACDetailDesc, lCntr)
    'oValueItems.Add(oValueItem)
    '
    ' Check if this is the selected index.
    'If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then
    '            ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'Next lCntr
    '
    ' Check if the selected index is zero. If so,
    ' we set the controls index to zero.
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = 0 Then
    '        ctlLookup.ListIndex = 0
    'End If
    '
    ' Set the validate option to not allow
    ' any other values to be set.
    'oValueItems.Validate = True
    'oValueItems.Translate = True
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)
    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide no 19
            If UnloadMode <> vbFormCode Then

                ' Call the update method to flush any
                ' new data currently being added.
                grdMainData.Update()
                'issue resolved 1435
                GridEdit_CellLeave()
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGridGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGridGeneral.Dispose()

            ' Check for errors.
            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGridGeneral = Nothing

            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload"
            '    End If
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: UpdateYearNames
    '
    ' Description:
    '
    ' History: 20/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateYearNames(ByVal v_sNewYearName As String) As Integer

        Dim result As Integer = 0
        Dim vPeriodArray As Array
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop through all the periods in the current year and update the year name

            lLower = g_vGridData.GetLowerBound(1)

            lUpper = g_vGridData.GetUpperBound(1)

            vPeriodArray = Array.CreateInstance(GetType(Object), New Integer() {1, lUpper - lLower + 1}, New Integer() {0, lLower})

            For iLoop1 As Integer = lLower To lUpper

                vPeriodArray(0, iLoop1) = g_vGridData(2, iLoop1)
            Next iLoop1

            ' Now call the business to update

            m_lReturn = m_oBusiness.UpdatePeriodYearNames(v_vPeriodArray:=vPeriodArray, v_sYearName:=v_sNewYearName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMRecordInUse Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update year name for periods.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateYearNames", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    MessageBox.Show("The year '" & v_sNewYearName & "' is already in use.", "Year", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateYearNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateYearNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sYearName As String = ""

        ' Click event of the OK button.

        Try

            'PN12720 - RDT 22/07/2004 - Check the list of periods for duplicates.

            If IsPeriodUnique() Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                ' Call the update method to flush any
                ' new data currently being added.
                grdMainData.Update()

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGridGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
                ' CTAF 200100 - Check if year name has been changed
                sYearName = txtYearName.Text
                If m_sOldYearName.Trim().ToLower() <> sYearName.Trim().ToLower() Then
                    m_lReturn = CType(UpdateYearNames(v_sNewYearName:=sYearName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                    ' Set the year has changed property
                    YearChanged = True
                    YearName = sYearName
                Else
                    YearChanged = False
                End If

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Call the update method to flush any
            ' new data currently being added.
            grdMainData.Update()

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGridGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Call the update method to flush any
            ' new data currently being added.
            grdMainData.Update()

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGridGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'Modified as per The document of trueDBgrid in team explorer
    'Private Sub grdMainData_AfterColEdit(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid.TrueDBGridEvents_AfterColEditEvent) Handles grdMainData.AfterColEdit
    Private Sub grdMainData_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdMainData.CellEndEdit

        Try

            ' Updates the grid after a column has been edited.
            grdMainData.Update()

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    'ToDoList:vijaypal 
    'Private Sub grdMainData_Error(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid.TrueDBGridEvents_ErrorEvent) Handles grdMainData.Error


    '    If m_bPreventGridError Then
    '        eventArgs.Response = 0
    '        m_bPreventGridError = False
    '    End If

    'End Sub


    'Modified as per The document of trueDBgrid in team explorer
    'Private Sub grdMainData_UnboundReadDataEx(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid.TrueDBGridEvents_UnboundReadDataExEvent) Handles grdMainData.UnboundReadDataEx
    Private Sub grdMainData_CellValueNeeded(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles grdMainData.CellValueNeeded

        ' This event is fired when the grid is first shown, when
        ' Refresh or ReBind is used, when the grid is scrolled,
        ' and after a record in the grid is modified and the user
        ' commits the change by moving off of the current row. The
        ' grid fetches data in "chunks", and the number of rows
        ' the grid is asking for is given by RowBuf.RowCount.

        Try

            ' Check if the data array has been initialised
            ' yet. If not we don't need to proceed.
            If Not Information.IsArray(g_vGridData) Then
                Exit Sub
            End If

            ' Read the data.
            'ToDoList
            'm_lReturn = CType(m_oGridGeneral.GridRead(RowBuf:=eventArgs.RowBuf, vStartLocation:=eventArgs.vStartLocation, lOffset:=eventArgs.lOffset, lApproximatePosition:=eventArgs.lApproximatePosition), gPMConstants.PMEReturnCode)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundReadDataEx", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    'Modified as per The document of trueDBgrid in team explorer
    'Private Sub grdMainData_CellValuePushed(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles grdMainData.CellValuePushed

    '    Dim lSelectedPeriodID As Integer
    '    Dim dtSelectedPeriodEndDate, dtLatestUsedPeriod As Date

    '    Try

    '        Dim auxVar As Object = eventArgs.RowBuf.Value(0, 1)

    '        
    '        
    '        If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
    '            If Not Information.IsDate(eventArgs.RowBuf.Value(0, 1)) Then
    '                MessageBox.Show("Periods end date is not a valid date.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
    '                m_bPreventGridError = True
    '                eventArgs.RowBuf.RowCount = 0
    '                Exit Sub
    '            End If
    '        End If

    '        
    '        lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, eventArgs.vWriteLocation))
    '        
    '        dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, eventArgs.vWriteLocation))

    '        
    '        m_lReturn = m_oBusiness.GetLatestUsedPeriod(v_lPeriodID:=lSelectedPeriodID, r_dtLatestUsedPeriod:=dtLatestUsedPeriod)

    '        'Only allow users to edit period end dates that are after the latest period that is being used.
    '        Dim auxVar_2 As Object = eventArgs.RowBuf.Value(0, 1)
    '        
    '        
    '        If dtSelectedPeriodEndDate <= dtLatestUsedPeriod And Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then
    '            MessageBox.Show("Cannot edit period end dates for periods that are already in use.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
    '            m_bPreventGridError = True
    '            eventArgs.RowBuf.RowCount = 0
    '            Exit Sub
    '        End If

    '        ' Read the data.
    '        m_lReturn = CType(m_oGridGeneral.GridWrite(RowBuf:=eventArgs.RowBuf, vWriteLocation:=eventArgs.vWriteLocation), gPMConstants.PMEReturnCode)

    '        ' Check for errors.
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundWriteData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Exit Sub

    '    End Try


    'End Sub


    'ToDoList:vijaypal
    'Private Sub grdMainData_UnboundAddData(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid.TrueDBGridEvents_UnboundAddDataEvent) Handles grdMainData.UnboundAddData

    '    ' This event is fired when the user adds a new
    '    ' row of data.

    '    Try

    '        ' Check if the data array has been initialised.
    '        If Not Information.IsArray(g_vGridData) Then
    '            g_vGridData = Nothing

    '            ' {* USER DEFINED CODE (Begin) *}

    '            ' Initialise the grid array.
    '            ReDim g_vGridData(3, 0)

    '            ' {* USER DEFINED CODE (End) *}
    '        End If

    '        ' Read the data.

    '        m_lReturn = CType(m_oGridGeneral.GridAdd(RowBuf:=eventArgs.RowBuf, vNewRowBookmark:=CStr(eventArgs.vNewRowBookmark)), gPMConstants.PMEReturnCode)

    '        ' Check for errors.
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundAddData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Exit Sub

    '    End Try

    'End Sub

    'ToDoList:vijaypal
    'Private Sub grdMainData_UnboundDeleteRow(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid.TrueDBGridEvents_UnboundDeleteRowEvent) Handles grdMainData.UnboundDeleteRow

    '    Dim lSelectedPeriodID As Integer
    '    Dim dtSelectedPeriodEndDate, dtLatestUsedPeriod As Date

    '    Try


    '        lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, eventArgs.vBookmark))

    '        dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, eventArgs.vBookmark))


    '        m_lReturn = m_oBusiness.GetLatestUsedPeriod(v_lPeriodID:=lSelectedPeriodID, r_dtLatestUsedPeriod:=dtLatestUsedPeriod)

    '        'Only allow users to delete periods that are after the latest period that is being used.
    '        If dtSelectedPeriodEndDate <= dtLatestUsedPeriod Then
    '            MessageBox.Show("Cannot delete periods that are already in use.", "Invalid Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
    '            m_bPreventGridError = True


    '            eventArgs.vBookmark = DBNull.Value
    '        Else

    '            ' Read the data.

    '            m_lReturn = CType(m_oGridGeneral.GridDelete(vBookmark:=CStr(eventArgs.vBookmark)), gPMConstants.PMEReturnCode)

    '            ' Check for errors.
    '            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
    '            End If

    '        End If

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundDeleteRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Exit Sub

    '    End Try

    'End Sub
    ' PRIVATE Events (End)

    Private Sub txtYearName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtYearName.Enter

        txtYearName.SelectionStart = 0
        txtYearName.SelectionLength = Strings.Len(txtYearName.Text)

    End Sub


    ' ***************************************************************** '
    ' Name: IsUnique
    '
    ' Description: Checks that the given string is unique within the array
    '
    ' ***************************************************************** '
    Private Function IsPeriodUnique() As Boolean

        Dim result As Boolean = False
        Dim lUBnd As Integer
        Dim sTitle, sMessage As String

        Try

            result = True

            If Information.IsArray(g_vGridData) Then


                lUBnd = g_vGridData.GetUpperBound(1)

                For lRow As Integer = 0 To lUBnd - 1
                    For lRow2 As Integer = lRow + 1 To lUBnd
                        ''''''
                        If Not g_vGridData(0, lRow).GetType.FullName = "System.String" Then
                            g_vGridData(0, lRow) = g_vGridData(0, lRow).value
                        End If
                        If Not g_vGridData(0, lRow2).GetType.FullName = "System.String" Then
                            g_vGridData(0, lRow2) = g_vGridData(0, lRow2).value
                        End If
                        If Not g_vGridData(1, lRow).GetType.FullName = "System.String" Then
                            If Not g_vGridData(1, lRow).GetType.FullName = "System.DateTime" Then
                                g_vGridData(1, lRow) = g_vGridData(1, lRow).value
                            End If
                        End If
                        If Not g_vGridData(1, lRow2).GetType.FullName = "System.String" Then
                            If Not g_vGridData(1, lRow2).GetType.FullName = "System.DateTime" Then
                                g_vGridData(1, lRow2) = g_vGridData(1, lRow2).value
                            End If
                        End If
                        If CStr(g_vGridData(0, lRow)).TrimEnd() = CStr(g_vGridData(0, lRow2)).TrimEnd() Then

                            result = False


                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriodNotUniqueTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriodNotUnique, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                            MessageBox.Show(sMessage & CStr(g_vGridData(0, lRow)).TrimEnd(), sTitle, MessageBoxButtons.OK)


                            Return result

                        End If



                        If CDate(g_vGridData(1, lRow)).ToString("yyyyMMdd") = CDate(g_vGridData(1, lRow2)).ToString("yyyyMMdd") Then

                            result = False


                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriodNotUniqueTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            sMessage = "The periods end date must be unique. The following end date is not unique: "


                            MessageBox.Show(sMessage & CStr(g_vGridData(1, lRow)).TrimEnd(), sTitle, MessageBoxButtons.OK)

                            Return result
                        End If
                    Next lRow2
                Next lRow

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to determine if period is unique", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPeriodUnique", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub frmDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub



    'Private Sub grdMainData_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdMainData.DoubleClick
    'Dim lSelectedPeriodID As Integer
    'Dim dtSelectedPeriodEndDate, dtLatestUsedPeriod As Date

    'Try
    '    Dim auxVar As Object = grdMainData.CurrentCell.Value()
    '    'Dim auxVar As Object = e.RowBuf.Value(0, 1)

    '    'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
    '    If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
    '        'If Not Information.IsDate(e.RowBuf.Value(0, 1)) Then
    '        'If Not Information.IsDate(e.RowBuf.Value(0, 1)) Then
    '        If Not Information.IsDate(auxVar) Then
    '            MessageBox.Show("Periods end date is not a valid date.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
    '            m_bPreventGridError = True
    '            'modified
    '            'e.RowBuf.RowCount = 0
    '            Exit Sub
    '        End If
    '    End If

    '    'UPGRADE_WARNING: (1068) g_vGridData() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
    '    'lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, e.vWriteLocation))
    '    lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, grdMainData.CurrentCell.RowIndex))
    '    'UPGRADE_WARNING: (1068) g_vGridData() of type Variant is being forced to Date. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
    '    'dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, e.vWriteLocation))
    '    dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, grdMainData.CurrentCell.RowIndex))

    '    'UPGRADE_TODO: (1067) Member GetLatestUsedPeriod is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
    '    m_lReturn = m_oBusiness.GetLatestUsedPeriod(v_lPeriodID:=lSelectedPeriodID, r_dtLatestUsedPeriod:=dtLatestUsedPeriod)

    '    'Only allow users to edit period end dates that are after the latest period that is being used.
    '    'Dim auxVar_2 As Object = e.RowBuf.Value(0, 1)
    '    Dim auxVar_2 As Object = grdMainData.CurrentCell.Value()
    '    'UPGRADE_WARNING: (1068) RowBuf.Value() of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
    '    'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
    '    If dtSelectedPeriodEndDate <= dtLatestUsedPeriod And Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then
    '        MessageBox.Show("Cannot edit period end dates for periods that are already in use.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
    '        m_bPreventGridError = True
    '        'modified
    '        'e.RowBuf.RowCount = 0

    '        Exit Sub
    '    End If

    '    ' Read the data.
    '    'modified
    '    'm_lReturn = CType(m_oGridGeneral.GridWrite(RowBuf:=e.RowBuf, vWriteLocation:=e.vWriteLocation), gPMConstants.PMEReturnCode)
    '    m_lReturn = CType(m_oGridGeneral.GridWrite(RowBuf:=grdMainData, vWriteLocation:=grdMainData.CurrentCell.ColumnIndex), gPMConstants.PMEReturnCode)

    '    ' Check for errors.
    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
    '    End If

    'Catch excep As System.Exception



    '    ' Error Section.

    '    ' Log Error.
    '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundWriteData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '    Exit Sub

    'End Try
    ' End Sub
    'issue resolved 1435
    Public Sub GridEdit_CellLeave()


        If grdMainData.SelectedCells.Count > 0 Then
            For lRow As Integer = 0 To grdMainData.SelectedCells.Count
                If Not grdMainData.EditingControl Is Nothing Then
                    Dim auxVar As Object = grdMainData.EditingControl.Text

                    If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                        If Not Information.IsDate(auxVar) Then
                            If grdMainData.SelectedCells.Item(0).Value <> auxVar Then

                                'If Not (e.RowIndex > g_vGridData.GetUpperBound(1)) Then
                                MessageBox.Show("Periods end date is not a valid date.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                                m_bPreventGridError = True
                                'If prevVal = "" Then
                                '    grdMainData.EditingControl.Text = g_vGridData(ACSubPeriodEndDate, grdMainData.Rows(lRow).Cells(lRow) - 1)
                                'Else
                                '    grdMainData.EditingControl.Text = prevVal
                                'End If
                                Exit Sub
                                'End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Function IsValidDateString(ByVal dateString As String) As Boolean
        Try
            Dim minDate As DateTime = New DateTime(1753, 1, 1)
            If Information.IsDate(dateString) Then
                If (CDate(dateString) < minDate) Then
                    Return False
                End If
            Else
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Sub grdMainData_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMainData.CellLeave
        Dim lSelectedPeriodID As Integer
        Dim dtSelectedPeriodEndDate, dtLatestUsedPeriod As Date

        Try
            If e.ColumnIndex = 1 Then
                Dim auxVar As Object = sender.editingcontrol.text

                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                    'If Not Information.IsDate(auxVar) Then
                    ' Fix for 5027
                    If Not IsValidDateString(auxVar) Then
                        If Not (e.RowIndex > g_vGridData.GetUpperBound(1)) Then
                            MessageBox.Show("Periods end date is not a valid date.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                            m_bPreventGridError = True
                            If prevVal = "" Then
                                sender.editingcontrol.text = g_vGridData(ACSubPeriodEndDate, e.RowIndex - 1)
                            Else
                                sender.editingcontrol.text = prevVal
                            End If
                            Exit Sub
                        End If
                    End If
                End If
            End If

            'If e.ColumnIndex = 0 Then
            '    sender.text = sender.editingcontrol.text
            'End If
            'lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, EventArgs.vWriteLocation))

            If e.RowIndex > g_vGridData.GetUpperBound(1) Then

                ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) + 1)
                If sender.editingcontrol.text = "" Then
                    If g_vGridData(ACSubPeriodEndDate, e.RowIndex) = "" And g_vGridData(ACSubPeriodName, e.RowIndex) = "" Then
                        ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) - 1)
                        Exit Sub
                    End If
                End If
                g_vGridData(ACSubPeriodID, e.RowIndex) = CInt(g_vGridData(ACSubPeriodID, e.RowIndex - 1)) + 1
                g_vGridData(3, e.RowIndex) = CInt(g_vGridData(3, e.RowIndex - 1)) + 1
                If e.ColumnIndex = 0 Then
                    g_vGridData(ACSubPeriodEndDate, e.RowIndex) = CDate(g_vGridData(ACSubPeriodEndDate, e.RowIndex - 1))
                ElseIf e.ColumnIndex = 1 Then
                    g_vGridData(0, e.RowIndex) = g_vGridData(0, e.RowIndex - 1)
                End If
                m_lReturn = m_oGridGeneral.GridAdd(RowBuf:=grdMainData, vWriteLocation:=e.RowIndex, vWriteColumn:=e.ColumnIndex)
                'grdMainData.Rows.Insert(g_vGridData(0, e.RowIndex), CDate(g_vGridData(1, e.RowIndex)).ToString("dd MMMM yyyy"))
                'Dim dt As New DataTable
                'Dim d As DataRow
                'Dim dc As DataColumn
                'dc = New DataColumn
                'dc.ColumnName = "Period Name"
                'dt.Columns.Add(dc)
                'dc = New DataColumn
                'dc.ColumnName = "End Date"
                'dt.Columns.Add(dc)

                'For i As Integer = 0 To g_vGridData.GetUpperBound(1)
                '    d = dt.NewRow
                '    For j As Integer = 0 To g_vGridData.GetUpperBound(0) - 2
                '        d.ItemArray(j) = g_vGridData(j, i)
                '    Next
                '    dt.Rows.Add(d)
                'Next
                'grdMainData.DataSource = dt
                Exit Sub
            Else
                lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, e.RowIndex))

                'dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, EventArgs.vWriteLocation))
                dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, e.RowIndex))
            End If

            m_lReturn = m_oBusiness.GetLatestUsedPeriod(v_lPeriodID:=lSelectedPeriodID, r_dtLatestUsedPeriod:=dtLatestUsedPeriod)

            'Only allow users to edit period end dates that are after the latest period that is being used.
            If e.ColumnIndex = 1 Then
                'Dim auxVar_2 As Object = EventArgs.RowBuf.Value(0, 1)
                Dim auxVar_2 As Object = sender.editingcontrol.text
                'Modified,add an extra if condition
                'Fix For 5021
                If Not CDate(auxVar_2) = DateTime.ParseExact(CDate(dtSelectedPeriodEndDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None) Then
                    'If Not CDate(auxVar_2) = CDate(dtSelectedPeriodEndDate).ToString("dd/MM/yyyy") Then
                    If dtSelectedPeriodEndDate <= dtLatestUsedPeriod And Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then
                        MessageBox.Show("Cannot edit period end dates for periods that are already in use.", "Invalid Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                        m_bPreventGridError = True
                        sender.editingcontrol.text = prevVal
                        '''''''
                        'EventArgs.RowBuf.RowCount = 0

                        Exit Sub
                    End If
                End If
            End If
            ' Read the data.
            'm_lReturn = CType(m_oGridGeneral.GridWrite(RowBuf:=EventArgs.RowBuf, vWriteLocation:=EventArgs.vWriteLocation), gPMConstants.PMEReturnCode)
            m_lReturn = CType(m_oGridGeneral.GridWrite(RowBuf:=grdMainData, vWriteLocation:=e.RowIndex, vWriteColumn:=e.ColumnIndex), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundWriteData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub grdMainData_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMainData.CellEnter
        prevVal = grdMainData.CurrentCell.Value
    End Sub
    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub



    Private Sub grdMainData_RowDeleting(ByVal sender As System.Object, ByVal e As System.Data.DataRowChangeEventArgs) Handles grdMainData.RowDeleting

        Try


            'lSelectedPeriodID = CInt(g_vGridData(ACSubPeriodID, EventArgs.vBookmark))

            'dtSelectedPeriodEndDate = CDate(g_vGridData(ACSubPeriodEndDate, EventArgs.vBookmark))


            'm_lReturn = m_oBusiness.GetLatestUsedPeriod(v_lPeriodID:=lSelectedPeriodID, r_dtLatestUsedPeriod:=dtLatestUsedPeriod)

            ''Only allow users to delete periods that are after the latest period that is being used.
            'If dtSelectedPeriodEndDate <= dtLatestUsedPeriod Then
            '    MessageBox.Show("Cannot delete periods that are already in use.", "Invalid Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
            '    m_bPreventGridError = True


            '    EventArgs.vBookmark = DBNull.Value
            'Else

            '    ' Read the data.

            '    m_lReturn = CType(m_oGridGeneral.GridDelete(vBookmark:=CStr(EventArgs.vBookmark)), gPMConstants.PMEReturnCode)

            '    ' Check for errors.
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            '    End If

            'End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundDeleteRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub grdMainData_RowAdded(ByVal sender As System.Object, ByVal e As System.Data.DataTableNewRowEventArgs) Handles grdMainData.RowAdded
        ' This event is fired when the user adds a new
        ' row of data.

        Try

            ' Check if the data array has been initialised.
            If Not Information.IsArray(g_vGridData) Then
                g_vGridData = Nothing

                ' {* USER DEFINED CODE (Begin) *}

                ' Initialise the grid array.
                ReDim g_vGridData(3, 0)

                ' {* USER DEFINED CODE (End) *}
            End If

            ' Read the data.

            'm_lReturn = CType(m_oGridGeneral.GridAdd(RowBuf:=EventArgs.RowBuf, vNewRowBookmark:=CStr(EventArgs.vNewRowBookmark)), gPMConstants.PMEReturnCode)
            'm_lReturn = CType(m_oGridGeneral.GridAdd(RowBuf:=grdMainData, vNewRowBookmark:=CStr(e.)), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="Grid_UnboundAddData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub
End Class
