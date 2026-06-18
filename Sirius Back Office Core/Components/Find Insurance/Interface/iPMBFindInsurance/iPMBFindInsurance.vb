Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports System.Runtime.InteropServices

 Partial Friend Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 07/10/1998
	'
	' Description: Main interface.
	'
	' Edit History: TF071098 - Created from iFindInsurance
	' ED 05082002 : Code added to search SBO Policy based on the Front Office
	'               data based on the registry setting, whethere Carole Nash
	'               Search is activated
	' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lInsFileCnt As Integer
	Private m_sInsReference As String = ""
	Private m_lInsHolderCnt As Integer
	Private m_sShortName As String = "" 'JW190498
	Private m_sLongName As String = "" 'JW190498
	Private m_lInsuranceFolderCnt As Integer 'TF100398
	Private m_sRegistration As String = "" 'Tom 031198
	'Private m_lProductID As Long
	'TF211298
	Private m_lPartyUIK As Integer
	Private m_lPolicyUIK As Integer
	Private m_lLeadAgentCnt As Integer
	
	' TF311298 - changed from NavProcessCode
	Private m_sInsFileType As String = ""
	
	Private m_lFindMode As Integer
	Private m_sRunMode As String = "" ' RAM20040226   : PN Issue 10592
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iPMBFindInsurance.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
    ' Stores the search data from the business object.
    'developer guide no. 33
    Public m_vSearchData(,) As Object
	'SJ 19/04/2004 - start
	Private m_bUnderwritingBranchEnabled As Boolean
	Private m_bIsUnderwritingBranch As Boolean
	Private m_vAlternateReference As Object
	Private m_oAllowBranches As Hashtable
	Private m_oValidSource As Hashtable
	'SJ 19/04/2004 - end
	'SJ 22/04/2004 - start
	' ShowLapsedOnly
	Private m_bShowLapsedOnly As Boolean
	'SJ 22/04/2004 - end
	
	' Alix
	Private m_bIncludeClosedBranches As Boolean
	'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.2.1)
	Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
	Private m_iFileType As Integer

	Private hScrollValue As Integer = 0
	Const LVM_FIRST As Int32 = &H1000
	Const LVM_SCROLL As Int32 = LVM_FIRST + 20
	Const SBS_HORZ As Integer = 0



	Public Property DisableWildcardSearchOption() As Boolean
		Get
			Return m_bDisableWildcardSearchOption
		End Get
		Set(ByVal Value As Boolean)
			m_bDisableWildcardSearchOption = Value
		End Set
	End Property
	
	
	Public Property EnablePartialWildcardSearchOption() As Boolean
		Get
			Return m_bEnablePartialWildcardSearchOption
		End Get
		Set(ByVal Value As Boolean)
			m_bEnablePartialWildcardSearchOption = Value
		End Set
	End Property
	'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.2.1)
	
	Public WriteOnly Property IncludeClosedBranches() As Boolean
		Set(ByVal Value As Boolean)
			m_bIncludeClosedBranches = Value
		End Set
	End Property
	
	'SJ 22/04/2004 - start
	Public WriteOnly Property ShowLapsedOnly() As Boolean
		Set(ByVal Value As Boolean)
			m_bShowLapsedOnly = Value
		End Set
	End Property
	'SJ 22/04/2004 - end
	
	
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
	
	
	'Public Property Get ProductID() As Long
	'
	'    ProductID = m_lProductID&
	'
	'End Property
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
	
	Public WriteOnly Property InsFileType() As String
		Set(ByVal Value As String)
			
			m_sInsFileType = Value
			
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
	Public ReadOnly Property InsFileCnt() As Integer
		Get
			
			Return m_lInsFileCnt
			
		End Get
	End Property
	
	Public Property InsReference() As String
		Get
			
			Return m_sInsReference
			
		End Get
		Set(ByVal Value As String)
			
			m_sInsReference = Value
			
		End Set
	End Property
	
	Public Property InsHolderCnt() As Integer
		Get
			
			Return m_lInsHolderCnt
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lInsHolderCnt = Value
			
		End Set
	End Property
	
	Public Property LongName() As String
		Get
			
			Return m_sLongName
			
		End Get
		Set(ByVal Value As String)
			
			m_sLongName = Value
			
		End Set
	End Property
	
	Public Property ShortName() As String
		Get
			
			Return m_sShortName
			
		End Get
		Set(ByVal Value As String)
			
			m_sShortName = Value
			
		End Set
	End Property
	
	'TF100398
	Public ReadOnly Property InsuranceFolderCnt() As Integer
		Get
			
			Return m_lInsuranceFolderCnt
			
		End Get
	End Property
	
	Public Property VehicleRegistration() As String
		Get
			
			Return m_sRegistration
			
		End Get
		Set(ByVal Value As String)
			
			m_sRegistration = Value
			
		End Set
	End Property
	
	'TF211298
	Public ReadOnly Property PartyUIK() As Integer
		Get
			
			Return m_lPartyUIK
			
		End Get
	End Property
	
	'TF211298
	Public ReadOnly Property PolicyUIK() As Integer
		Get
			
			Return m_lPolicyUIK
			
		End Get
	End Property
	
	Public ReadOnly Property LeadAgentCnt() As Integer
		Get
			
			Return m_lLeadAgentCnt
			
		End Get
	End Property
	
	Public Property FindMode() As Integer
		Get
			Return m_lFindMode
		End Get
		Set(ByVal Value As Integer)
			m_lFindMode = Value
		End Set
	End Property
	
	Public Property RunMode() As String
		Get
			Return m_sRunMode
		End Get
		Set(ByVal Value As String)
			m_sRunMode = Value
		End Set
	End Property
	
	' PRIVATE Property Procedures (End)
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	' Edit History  :
	' RAM20040226   : PN Issue 10592 Changes
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Dim sInsRef, sInsFileType As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Property Updated
            If g_oBusiness Is Nothing Then

                If g_oObjectManager Is Nothing Then
                    g_oObjectManager = New bObjectManager.ObjectManager()
                End If
                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBusiness", "g_oObjectManager.Initialise failed")
                End If

                Dim temp_g_oBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindInsurance.Form", vInstanceManager:="ClientManager")
                g_oBusiness = temp_g_oBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBusiness", "g_oObjectManager.GetInstance failed")
                End If
            End If
            'UPGRADE_ISSUE: (2072) Control FindMode could not be resolved because it was within the generic namespace Form. More Information: http://www.vbtonet.com/ewis/ewi2072.aspx
            g_oBusiness.FindMode = m_lFindMode
			'Have we already got the details
			If txtRiskIndex.Text.Trim() <> "" Then
				DisplayStatusSearching()
				
				m_lReturn = CType(ValidateIndex(), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' set tab 1 to be active
					SSTabHelper.SetSelectedIndex(tabMainTab, 1)
				Else
					'Assign Values to Interface
					m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
				End If
				
				DisplayStatusFound()
				
				Return result
			End If
			
			If txtRegistrationNumber.Text.Trim() <> "" Then
				DisplayStatusSearching()
				m_lReturn = CType(ValidateVehicle(), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' set tab 1 to be active
					SSTabHelper.SetSelectedIndex(tabMainTab, 1)
					'EK 140199 Bug 206
				Else
					'Assign Values to Interface
					m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
				End If
				DisplayStatusFound()
				Return result
			Else
				
				If txtShortName.Text.Trim() <> "" Then
					' RAM20040226   : If, we have a Valid PartyCnt, we don't need to check it again
					If m_lInsHolderCnt < 1 Then
						m_lReturn = CType(ValidateLookups(), gPMConstants.PMEReturnCode)
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' set tab 1 to be active
							SSTabHelper.SetSelectedIndex(tabMainTab, 1)
							Return result
						End If
					End If
				Else
					m_lInsHolderCnt = 0
				End If
			End If
			
			' Get the details from the business object.
			
			' Display a searching message.
			DisplayStatusSearching()
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (Begin) *}
			sInsRef = txtInsReference.Text.Trim()
			
			Select Case m_sInsFileType
				Case "QUOTE"
					sInsFileType = "QUOTE"
				Case "MTAQUOTE"
					sInsFileType = "MTAQUOTE"
				Case "POLICY"
					sInsFileType = "POLICY"
				Case "RENEWAL"
					sInsFileType = "RENEWAL"
					'WPR12- Enhancement Quote Collection Process
				Case "MTAQREINS"
					sInsFileType = "MTAQREINS"
				Case "ALLQUOTE"
					sInsFileType = "ALLQUOTE"
				Case Else
					sInsFileType = "%"
			End Select
			

			g_oBusiness.FindMode = m_lFindMode


			m_lReturn = g_oBusiness.SearchByQuery(r_vResultArray:=m_vSearchData,
												  v_vInsuranceRef:=txtInsReference.Text.Trim(),
												  v_vInsFileType:=sInsFileType,
												  v_vShortName:=txtShortName.Text.Trim(),
												  v_vVehicleRegNo:=txtRegistrationNumber.Text.Trim(),
												  v_bShowLapsedOnly:=m_bShowLapsedOnly,
												  v_bLimitResults:=True,
												  v_lNumberOfRecords:=ACMaxSearchDetails
												  )


			'Assign Values to Interface
			m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check the return values.
			Select Case (m_lReturn)
				Case gPMConstants.PMEReturnCode.PMTrue
					' Found search details.
					
				Case gPMConstants.PMEReturnCode.PMNotFound
					' No search details found.
					
				Case Else
					' Failed to get details.
					result = gPMConstants.PMEReturnCode.PMFalse
					
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
					
					Return result
			End Select
			
			' Display the number of item found message.
			DisplayStatusFound()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ProcessFindParty
	'
	' Description: Process the Party lookup component.
	'
	' ***************************************************************** '
	
	' SB 31/03/98 defect 37
	' function added to enable find short name to work.
	
	Private Function ProcessFindParty() As Integer
		Dim result As Integer = 0

		'TF031298 - changed from SB 31/03/98 Defect 37
		

        Dim oFindParty As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			
			'PN-71681(Sushil Kumar)
			If g_oObjectManager Is Nothing Then
				g_oObjectManager = New bObjectManager.ObjectManager()
				
				' Call the initialise method.
				m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
			End If
			
			
			
			' Create Find Party object
			
            Dim temp_oFindParty As Object = Nothing
			m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oFindParty, "iPMBFindParty.Interface_Renamed", gPMConstants.PMGetLocalInterface)
			oFindParty = temp_oFindParty
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				result = gPMConstants.PMEReturnCode.PMFalse
				oFindParty = Nothing
				Return result
			End If
			
			' Set the process modes.

			m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' carry on - let FindParty use defaults
			End If
			
			' Set the properties.

			oFindParty.CallingAppName = m_sCallingAppName

			oFindParty.ShortName = txtShortName.Text.Trim()
			

			m_lReturn = oFindParty.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Retrieve Party properties

			If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
				With oFindParty

					txtShortName.Text = .ShortName.Trim()

					m_sShortName = .ShortName.Trim()

					m_lInsHolderCnt = .PartyCnt

					m_sLongName = .LongName.Trim()
				End With
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Destroy Find Party object

            oFindParty.Dispose()
			oFindParty = Nothing
			
			' Set the mouse pointer.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process find party", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
    ''' <summary>
    ''' Updates all interface details from the search data.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
	Public Function DataToInterface() As Integer
		
        Dim nResult As Integer
        Dim bMatch As Boolean

        Try

            nResult = PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not IsArray(m_vSearchData) Then
                Return nResult
            End If

            'Hide the registration column if not used...

            If (m_vSearchData.GetUpperBound(0) < ACIRegistration) Or (m_lFindMode = 1) Then
                lvwSearchDetails.Columns.Item(5).Width = CInt(0)
            Else
                lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(900))
            End If

            'Hide the value column if not used
            If m_vSearchData.GetUpperBound(0) < ACIIndexValue Then
                lvwSearchDetails.Columns.Item(6).Width = CInt(0)
            Else
                lvwSearchDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(900))
                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6a, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Assign the details to the interface.

            For nRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

             If m_bUnderwritingBranchEnabled Then
                    bMatch = False
                    If m_bIsUnderwritingBranch Then
                        ' The branch/company we are logged on as is an "Insurer" one
                        ' CJB 150604 Do not check for null as "" is returned!
                       If CStr(m_vSearchData(ACIAlternateReference, nRow)).Trim() <> "" Then
                            'This is and Edi policy created by an "Insurer" branch
                            bMatch = True
                        ElseIf ToSafeInteger(m_vSearchData(ACIPolicyTypeId, nRow)) = 3 Then
                            'The is a general policy
                            bMatch = True
                        ElseIf ToSafeInteger(m_vSearchData(ACIInsFileSourceId, nRow)) = g_iSourceID Then
                            'This policy is owned by the current branch/company
                            bMatch = True
                        End If
                    Else
                        ' The branch/company we are logged on as is a "Normal" one
                        ' CJB 150604 Do not check for null as "" is returned!
                       If CStr(m_vSearchData(ACIUnderwritingBranchInd, nRow)).Trim() = "" Then
                            'this policy was not created by an "Insurer" branch
                            bMatch = True
                        End If
                    End If
                Else
                    'If system not set up with underwriting branch option then show all.
                    bMatch = True
                End If

                If bMatch Then
                    If ValidSource(vSource:=m_vSearchData(ACIInsFileSourceId, nRow)) Then
                        m_lReturn = CType(AddRowToPolicyList(v_lRow:=nRow), PMEReturnCode)
                    End If
                End If
             Next nRow

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), PMEReturnCode)

            ' Check for errors
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to get details.
                nResult = PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp,vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
	End Function
	' ***************************************************************** '
	' Name: AddRowToPolicyList
	'
	' Description:
	'
	' History: 19/04/2004 SJ - Created.
	'
	' ***************************************************************** '
	Private Function AddRowToPolicyList(ByVal v_lRow As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Const ACFindImage As String = "FindImage"
			Dim oListItem As ListViewItem
			
			' Assign the details to the first column.
			' Column 1 Reference

            oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIInsReference, v_lRow)).Trim(), ACFindImage)
            ListViewHelper.SetListItemIconProperty(oListItem, ACFindImage)
			' Assign details to the other columns
			
			' Column 2 Product
			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIProductName, v_lRow)).Trim()
			
			' Column 3 Type
			Select Case (m_lFindMode)
				Case 0
					' Column 3 Type
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIInsFileType, v_lRow)).Trim()
				Case 1

					If (Convert.IsDBNull(m_vSearchData(ACIStatus, v_lRow)) Or IsNothing(m_vSearchData(ACIStatus, v_lRow))) Or (CStr(m_vSearchData(ACIStatus, v_lRow)).Trim() = "") Then
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Live"
					Else
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIStatus, v_lRow)).Trim()
					End If
			End Select
			
			' Column 4 Insured
			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIInsuredLongName, v_lRow)).Trim()
			
			' Column 5 Date Modified
			' TF311298 - Use Date Created if not yet modified

			If (Convert.IsDBNull(m_vSearchData(ACILastModified, v_lRow)) Or IsNothing(m_vSearchData(ACILastModified, v_lRow))) Or (CStr(m_vSearchData(ACILastModified, v_lRow)) = "") Then
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIDateCreated, v_lRow)))
			Else
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACILastModified, v_lRow)))
			End If
			
			' Column 6 Vehicle Registration
			If (m_vSearchData.GetUpperBound(0) = ACIRegistration) And (m_lFindMode = 0) Then
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIRegistration, v_lRow)).Trim()
			End If
			
			'Or maybe the index...
			If m_vSearchData.GetUpperBound(0) > ACIIndexValue Then
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIGISProperty, v_lRow)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACIIndexValue, v_lRow)).Trim()
			End If
			
			' Ram 08-01-2001  Column 5,6
			If m_vSearchData.GetUpperBound(0) > ACIValue Then
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIObjectName, v_lRow)).Trim() & ": " & CStr(m_vSearchData(ACIPropertyName, v_lRow)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACIValue, v_lRow)).Trim()
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			' Set the tag property with the index of
			' the search data storage.
			oListItem.Tag = CStr(v_lRow)
			
			' Refresh the first X amount of rows, to
			' allow the user to see the results instantly.
			If v_lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
				' Select the first item.
				lvwSearchDetails.Items.Item(0).Selected = True
				
				' Refresh the initial results.
				lvwSearchDetails.Refresh()
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRowToPolicyList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRowToPolicyList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: AllowOtherBranchesToViewPolicies
	'
	' Description:
	'
	' History: 19/04/2004 SJ - Created.
	'
	' ***************************************************************** '
	Private Function AllowOtherBranchesToViewPolicies() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            Dim vAllowBranchesArray(,) As Object = Nothing
			Dim sSourceId, sValue As String
			
			m_oAllowBranches = New Hashtable()
			

			m_lReturn = g_oBusiness.AllowOtherBranchesToViewPolicies(r_vAllowBranchesArray:=vAllowBranchesArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowOtherBranchesToViewPolicies")
				Return result
			End If
			
			If Not Information.IsArray(vAllowBranchesArray) Then
				Return result
			End If
			

			For i As Integer = 0 To vAllowBranchesArray.GetUpperBound(1)

                sSourceId = CStr(vAllowBranchesArray(0, i))

                sValue = CStr(vAllowBranchesArray(1, i))
				If sValue = "1" Then
					m_oAllowBranches.Add(sSourceId, sValue)
				End If
			Next i
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowOtherBranchesToViewPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	' ***************************************************************** '
	' Name: GetValidSources (Standard Method)
	'
	' Description: Calls the appropriate methods to get the Sources
	'              which the the current user can access
	'
	' History: 19/04/2004 SJ - Created.
	' ***************************************************************** '
	Private Function GetValidSources() As Integer
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			Dim oPMUser As bPMUser.Business
            Dim vSourceArray(,) As Object = Nothing
			
			m_oValidSource = New Hashtable()
			
            Dim temp_oPMUser As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oPMUser = temp_oPMUser
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources")
				Return result
			End If
			

			m_lReturn = oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_oObjectManager.UserID, v_bIncludeDeletedSources:=m_bIncludeClosedBranches)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources")
				Return result
			End If
			
			If Not (oPMUser Is Nothing) Then

                oPMUser.Dispose()
				oPMUser = Nothing
			End If
			
			If Not Information.IsArray(vSourceArray) Then
				Return result
			End If

            'developer guide no 162. 
            For i As Integer = 0 To vSourceArray.GetUpperBound(1)

                'developer guide no 162. 
                m_oValidSource.Add(CStr(vSourceArray(0, i)), CStr(vSourceArray(0, i)))
            Next i
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ValidSource
	'
	' Description:
	'
	' History: 19/04/2004 SJ - Created.
	'
	' ***************************************************************** '
	Private Function ValidSource(ByVal vSource As Object) As Boolean

		Dim sSourceId As String = CStr(vSource)
		
		If m_oAllowBranches.ContainsKey(sSourceId) Then
			Return True
		End If
		
		If m_oValidSource.ContainsKey(sSourceId) Then
			Return True
		End If
		
    End Function

    ''' <summary>
    ''' Updates the property member from the search data  storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataToProperties() As Integer

        Dim nResult As Integer
        Dim nSelectedItem As Integer
        Dim nSourceID As Integer
        Dim nKeyID As Integer
        Dim obPMUPolicy As Object
        Dim oResult As Object
        Dim bIsPendingPortfolioTransfer As Boolean
        Dim bIsPendingCloneTransfer As Boolean

        Try

            nResult = PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            nSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            m_lInsFileCnt = CInt(m_vSearchData(ACIInsFileCnt, nSelectedItem))
            m_sInsReference = m_vSearchData(ACIInsReference, nSelectedItem).Trim()
            m_lInsHolderCnt = CInt(m_vSearchData(ACIInsHolderCnt, nSelectedItem).Trim())
            m_sLongName = m_vSearchData(ACIInsuredLongName, nSelectedItem).Trim()
            m_sShortName = m_vSearchData(ACIInsuredShortName, nSelectedItem).Trim()
            m_lInsuranceFolderCnt = CInt(m_vSearchData(ACIInsFolderCnt, nSelectedItem))
            If m_vSearchData(ACILeadAgentCnt, nSelectedItem) <> "" Then
                m_lLeadAgentCnt = CInt(m_vSearchData(ACILeadAgentCnt, nSelectedItem))
            End If

            'Calculate the combined UIKs
            nSourceID = CInt(m_vSearchData(ACIInsFileSourceId, nSelectedItem))
            nKeyID = CInt(m_vSearchData(ACIInsFileId, nSelectedItem))

            '   SJP 04072002 - calcCombinedKey will return what is passed in
            '   Therefore we need to put m_lPolicyUIK to what it will need to be
            m_lPolicyUIK = m_lInsFileCnt

            m_lReturn = g_oBusiness.CalcCombinedKey(v_lSourceID:=nSourceID, v_lKeyID:=nKeyID, r_lCombinedKeyID:=m_lPolicyUIK)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set the Policy UIK.", vApp:=ACApp, _
                                   vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Err().Number, vErrDesc:=Err().Description)
                Return nResult
            End If

            nSourceID = CInt(m_vSearchData(ACIInsuredSourceId, nSelectedItem))
            nKeyID = CInt(m_vSearchData(ACIInsuredId, nSelectedItem))

            m_lReturn = g_oObjectManager.GetInstance(obPMUPolicy, "bPMUPolicy.Business", vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to create bPMUPolicy.Business.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If

            m_lReturn = obPMUPolicy.IsPendingPortfolioTransfer(sInsuranceFileRef:=m_sInsReference, _
                                                      r_oResult:=oResult, _
                                                      r_bIsPendingPortfolioTransfer:=bIsPendingPortfolioTransfer, _
                                                      r_bIsPendingCloneTransfer:=bIsPendingCloneTransfer)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="IsPendingPortfolioTransfer", vApp:=ACApp, vClass:=ACClass, _
                                   vMethod:="DataToProperties", vErrNo:=Err().Number, vErrDesc:=Err().Description)
                Return nResult
            End If

            If IsArray(oResult) Or bIsPendingPortfolioTransfer Then
                MessageBox.Show("Pending Portfolio Transfer.", "Pending portfolio transfer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return nResult
            ElseIf bIsPendingCloneTransfer Then
                MessageBox.Show("Pending Clone Transfer.", "Pending Clone transfer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return nResult
            End If

            '   SJP 04072002 - calcCombinedKey will return what is passed in
            '   Therefore we need to put m_lPartyUIK to what it will need to be
            m_lPartyUIK = m_lInsHolderCnt

            m_lReturn = g_oBusiness.CalcCombinedKey(v_lSourceID:=nSourceID, v_lKeyID:=nKeyID, r_lCombinedKeyID:=m_lPartyUIK)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set the Party UIK.", vApp:=ACApp, vClass:=ACClass, _
                                   vMethod:="DataToProperties", vErrNo:=Err().Number, vErrDesc:=Err().Description)
                Return nResult
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp,vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        Finally

            obPMUPolicy.Dispose()
            obPMUPolicy = Nothing

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
		
		' TF131298
		' Not used at present, but leave in as lookup boxes suppressed, not deleted
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_lReturn = CType(GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPartyType, ctlLookup:=cmbType), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: PropertiesToInterface
	'
	' Description: Updates the interface details from the property
	'              members.
	'
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		
		Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            txtInsReference.Text = m_sInsReference.Trim()
            txtShortName.Text = m_sShortName.Trim()
            txtRegistrationNumber.Text = m_sRegistration.Trim()

            ' {* USER DEFINED CODE (End) *}


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	' ***************************************************************** '
	' Name: ValidateLookups
	'
	' Description: Validates the interface lookups.
	'
	' ***************************************************************** '
	Private Function ValidateLookups() As Integer
		Dim result As Integer = 0

		' TF131298
		' Lookup boxes suppressed but performs validation on partyID
		

        Dim oFindParty As Object
		Dim lReturn, lPartyCnt As Integer
		Static sTitle, sMessage As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Exit if Lookup has wildcard (% or *) as
			' Search query will use text, not ID
			If (txtShortName.Text.Trim().EndsWith("%")) Or (txtShortName.Text.Trim().EndsWith("*")) Then
				Return result
			End If
			
			' Get an instance of the find party business
			' object via the public object manager.
            Dim temp_oFindParty As Object = Nothing
			lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oFindParty = temp_oFindParty
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the process modes.

			lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Check if we have the failed lookup descriptions.
			If sTitle = "" Then
				' Get failed lookup descriptions from the resource file.

                sTitle = CStr(iPMFunc.GetResData(g_iLanguageID, ACLookupFailTitle, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACLookupFail, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
			' Using the find party object, check the interface
			' lookup ID's are all valid.
			
			' {* USER DEFINED CODE (Begin) *}
			

			lReturn = oFindParty.GetID(vShortName:=txtShortName.Text, lId:=lPartyCnt)
			
			If lPartyCnt > 0 Then
				m_lInsHolderCnt = lPartyCnt
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
				'sb 030498 Defect 164
				'Correct action of default buttons
				txtShortName.Focus()
				SSTabHelper.SetSelectedIndex(tabMainTab, 1)
				
				' Display failed message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
				
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			' Call the terminate method

            oFindParty.Dispose()
			
			' Destroy the instance of the find party business
			' object from memory.
			oFindParty = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ValidateVehicle
	'
	' Description: Validates the interface vehicle lookup.
	'
	' ***************************************************************** '
	Private Function ValidateVehicle() As Integer
		
		Dim result As Integer = 0
		Dim lReturn As Integer
		Dim sRegistration As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sRegistration = txtRegistrationNumber.Text.Trim()
			

			lReturn = g_oBusiness.FindLikeVehicle(sRegistration:=sRegistration, vResultArray:=m_vSearchData)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate vehicles", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateVehicle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ValidateIndex
	'
	' Description: Validates the interface index.
	'
	' ***************************************************************** '
	Private Function ValidateIndex() As Integer
		
		Dim result As Integer = 0
		Dim lReturn As Integer
        Dim sIndex As String = ""

		'PN_71945 Start
		Dim lDataModelIndex As Integer = 1
		'PN_71945 End
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sIndex = txtRiskIndex.Text.Trim()
			'PN_71945 Start

            lReturn = g_oBusiness.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, lSpecificDataModelIndex:=lDataModelIndex, ifiletype:=m_iFileType)
			'PN_71945 End
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate index", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			' Position View control
			If Not cmdNavigate.Visible Then
				cmdView.Left = cmdNavigate.Left
			Else
				cmdView.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdNavigate.Left) = VB6.PixelsToTwipsX(cmdNavigate.Width) + 105)
			End If
			' Disable until a policy is selected
			' cmdView.Enabled = False
			' AJM 14/08/00 - do not show view button at all.
			cmdView.Visible = False
			
			If Not g_bPMGeminiLink Then
				'        tabMainTab.TabEnabled(2) = False
				SSTabHelper.SetTabVisible(tabMainTab, 2, False)
			End If
			
			'---------------------------------------------
			'ED 05082002 : Check if Registration Search Activated
			If g_bRegSearch Then
				SSTabHelper.SetTabVisible(tabMainTab, 2, True)
			End If
			'---------------------------------------------
			
			' Update the interface details with the
			' property members.
			m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' set the default radio button
			optQuote.Enabled = False
			optMTAQuote.Enabled = False
			optPolicy.Enabled = False
			optRenewal.Enabled = False
			optAllTypes.Enabled = False
			'WPR12- Enhancement Quote Collection Process
			optMTAReins.Enabled = False
			
			
			Select Case m_sInsFileType
                'Modified by Archana Tokas on 4/20/2010 10:25:32 AM changes refer vb6 code for the same function
                'Case gSIRLibrary.SIRInsFileTypeQuote
                Case SIRInsFileTypeQuote
                    optQuote.Checked = True
                Case gSIRLibrary.SIRInsFileTypeMTAQuote
                    optMTAQuote.Checked = True
                    'Modified by Archana Tokas on 4/20/2010 10:25:32 AM changes refer vb6 code for the same function
                    'Case gSIRLibrary.SIRInsFileTypePolicy
                Case SIRInsFileTypePolicy
                    optPolicy.Checked = True
                    'Modified by Archana Tokas on 4/20/2010 10:25:32 AM changes refer vb6 code for the same function
                    'Case gSIRLibrary.SIRInsFileTypeRenewal
                Case SIRInsFileTypeRenewal
                    optRenewal.Checked = True

                    'WPR12- Enhancement Quote Collection Process
                    'Modified by Archana Tokas on 4/20/2010 10:25:32 AM changes refer vb6 code for the same function
                    'Case gSIRLibrary.SIRInsFileTypeMTAReinstatement
                Case SIRInsFileTypeMTAReinstatement
                    optMTAReins.Checked = True
                Case gSIRLibrary.SIRInsFileTypeAllQuote
                    optAllTypes.Checked = True
                    optQuote.Enabled = True
                    optRenewal.Enabled = True
                    optMTAQuote.Enabled = True
                    optAllTypes.Enabled = True
                    optMTAReins.Enabled = True

                Case Else
                    optAllTypes.Checked = True
                    optQuote.Enabled = True
                    optMTAQuote.Enabled = True
                    optPolicy.Enabled = True
                    optRenewal.Enabled = True
                    optAllTypes.Enabled = True
                    'WPR12- Enhancement Quote Collection Process
                    optMTAReins.Enabled = True
            End Select
			
			' Display all of the lookup details.
			'BB Temporarily bypassed
			'BB m_lReturn& = DisplayLookupDetails()
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        SetInterfaceDefaults = PMFalse
			'        Exit Function
			'    End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the column widths for the search list.
			lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1600))
			lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(900))
			'RWH(25/05/01) Change size of header for wider content in mode 1.
			If m_lFindMode = 0 Then
				lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(850))
			Else
				lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1200))
			End If
			lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1800))
			lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1400))
			
			If g_bPMGeminiLink Then
				lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(900))
			Else
				lvwSearchDetails.Columns.Item(5).Width = CInt(0)
			End If
			
			'---------------------------------------------
			'ED 05082002 : Check if Registration Search Activated
			If g_bRegSearch Then
				lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(900))
			End If
			'---------------------------------------------
			
			lvwSearchDetails.Columns.Item(6).Width = CInt(0)
			
			' TF290498 - Moved this from Form_Initialize as it started Form_Load too early
			' SB 31/03/98
			' Hide the product group and type as it doesn't actually contain data
			' at the moment
			' Also hide Risk index as it is not being used for anything.
			
			cmbProduct.Visible = False
			lblProduct.Visible = False
			cmbType.Visible = False
			lblType.Visible = False
			'    txtRiskIndex.Visible = False
			'    lblRiskIndex.Visible = False
			
			
			
			'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			' RAM20040226   : Code Changes related to PN Issue 10592 - START
			'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			m_sRunMode = m_sRunMode.Trim()
			If m_sRunMode = "SearchByClientCode" Then
				' We need to diable the client short code field
				txtShortName.Enabled = False
				
				' We need to hide the Related client Find Command Button
				cmdRelatedPartyFind.Visible = False
				
				' We need to hide the New Search Command Button
				cmdNewSearch.Visible = False
			End If
			'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			' RAM20040226   : END
			'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ClearInterface
	'
	' Description: Clears all of the interface details for a new
	'              search.
	'
	' ***************************************************************** '
	Private Function ClearInterface() As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if the user still wishes to clear
			' the interface.
			

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Display the message.
			iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			
			' Check message result.
			If iMsgResult = System.Windows.Forms.DialogResult.No Then
				' Don't continue with the clear.
				Return result
			End If
			
			' Clear the interface details.
			
			' Clear the search data array.
			m_vSearchData = Nothing
			
			' Clear the search list details.
			lvwSearchDetails.Items.Clear()
			
			' Clear the search status bar.
            _stbStatus_Panel1.Text = ""
			
			' {* USER DEFINED CODE (Begin) *}
			
			txtInsReference.Text = ""
			' SB 31/03/98  Defect 36
			' All fields should be cleared.
			txtRiskIndex.Text = ""
			txtShortName.Text = ""
			txtRegistrationNumber.Text = ""
			
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			' Set focus to the search details.
			txtInsReference.Focus()
			
			' Set the default button.
			VB6.SetDefault(cmdFindNow, True)
			
			' {* USER DEFINED CODE (End) *}
			
			' Disable parts of the interface, so the
			' user can now only enter a new search
			m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' ***************************************************************** '
	Private Function SetFirstLastControls() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 2)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtInsReference
			m_ctlTabFirstLast(ACControlEnd, 0) = txtInsReference 'cmbProduct
			
			m_ctlTabFirstLast(ACControlStart, 1) = txtShortName
			m_ctlTabFirstLast(ACControlEnd, 1) = txtShortName 'cmbType
			
			m_ctlTabFirstLast(ACControlStart, 2) = txtRegistrationNumber
			m_ctlTabFirstLast(ACControlEnd, 2) = txtRegistrationNumber
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			If m_sCallingAppName = ACViaQuoteCollectionProcess Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACQuoteInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			Else

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			'cmdNew.Caption = iPMFunc.GetResData( _
			''    iLangID:=g_iLanguageID%, _
			''    lID:=ACNewButton, _
			''    iDataType:=PMResString)
			
			'cmdEdit.Caption = iPMFunc.GetResData( _
			''    iLangID:=g_iLanguageID%, _
			''    lID:=ACEditButton, _
			''    iDataType:=PMResString)
			

            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			If m_sCallingAppName = ACViaQuoteCollectionProcess Then

                SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			Else

                SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			' {* USER DEFINED CODE (Begin) *}
			


            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			If m_lFindMode = 0 Then


                lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			Else


                lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3a, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			


            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			If g_bPMGeminiLink Then
				'Link to Gemini - registration


                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			Else
				'Else status


                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3a, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
			'------------------------------------------------------------
			'ED 05082002 : Check if Registration Search Activated
			If g_bRegSearch Then


                lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			'------------------------------------------------------------
			


            lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblInsReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblRiskIndex.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskIndex, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            optAllTypes.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            optQuote.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACQuote, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            optPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicy, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            optRenewal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRenewal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            optMTAQuote.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMTAQuote, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblShortName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			'WPR12- Enhancement Quote Collection Process

            optMTAReins.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMTAQREINS, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
	' Name: DisableInterface
	'
	' Description: Disables parts of the interface while a search is
	'              in progress.
	'
	' ***************************************************************** '
	Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cmdOK.Enabled = Not bDisable
			'cmdEdit.Enabled = Not bDisable
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	Private Function GetLookupValues() As Integer
		
		' TF131298
		' Not used at present, but leave in as lookup boxes suppressed, not deleted
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			
			' Get all of the lookup values with the correct
			' effective date.

			m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
	Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
		
		' TF131298
		' Not used at present, but leave in as lookup boxes suppressed, not deleted
		
		Dim result As Integer = 0
		Dim lRow As Integer
		Dim bFoundMatch As Boolean
		
		' Lookup value contants.
		Const ACValueTableName As Integer = 0
		Const ACValueID As Integer = 1
		Const ACValueStartPos As Integer = 2
		Const ACValueNumber As Integer = 3
		
		' Lookup detail contants.
		Const ACDetailKey As Integer = 0
		Const ACDetailDesc As Integer = 1
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			bFoundMatch = False
			
			For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
				' Check for a match of the table name.
				If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
					' Found a match
					bFoundMatch = True
					Exit For
				End If
			Next lRow
			
			' Check if there has been a table match.
			If Not bFoundMatch Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
				
				Return result
			End If
			
			' Using the lookup values, populate the control with
			' the details from the lookup details array.
			
			For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
				' Add the details to the control.

                'NIIT - Replaced with the Migrated code 1144 
                ReflectionHelper.Invoke(ctlLookup, "AddItem", New Object() {m_vLookupDetails(ACDetailDesc, lCntr)})
                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


                'NIIT - Replaced with the Migrated code 1144 

                'ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr)
                ReflectionHelper.SetMember(ctlLookup, "ItemData", New Object() {ReflectionHelper.GetMember(ctlLookup, "NewIndex")}, m_vLookupDetails(ACDetailKey, lCntr))
				' Check if this is the selected index.
				If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


                    'NIIT - Replaced with the Migrated code 1144 
                    'ctlLookup.ListIndex = ctlLookup.NewIndex
                    ReflectionHelper.SetMember(ctlLookup, "ListIndex", ReflectionHelper.GetMember(ctlLookup, "NewIndex"))
				End If
			Next lCntr
			
			' Check if the selected index is blank. If so,
			' we set the controls index to zero.
			If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'NIIT - Replaced with the Migrated code 1144 
                'ctlLookup.ListIndex = 0
                ReflectionHelper.SetMember(ctlLookup, "ListIndex", 0)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupDesc
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupDesc) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupDesc(ByRef sLookupTable As String, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer
		'
		' TF131298
		' Not used at present, but leave in as lookup boxes suppressed, not deleted
		'
		'Dim result As Integer = 0
		'Dim lRow As Integer
		'Dim bFoundMatch As Boolean
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
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc")
				'
				'Return result
			'End If
			'
			' Using the lookup values, populate the lookup
			' string from the lookup details array when the
			' lookup ID has been matched.
			'
			'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
				' Check for a match on the ID.
				'If CInt(m_vLookupDetails(ACDetailKey, lCntr)) = lLookupID Then
					' Found a match
					'
					' Store the details to the lookup string.
					'sLookupDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
				'End If
			'Next lCntr
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: DisplayStatusSearching
	'
	' Description: Display the status searching message.
	'
	' ***************************************************************** '
	Private Sub DisplayStatusSearching()
		
		Static sMessage As String = ""
		
		Try 
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
			' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage
            Application.DoEvents()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: DisplayStatusFound
	'
	' Description: Display the status found message.
	'
	' ***************************************************************** '
	Private Sub DisplayStatusFound()
		
		Static sMessage As String = ""
		Dim lItemsFound As Integer
		
		Try 
			
			' Store the total of item found.
			If Not Information.IsArray(m_vSearchData) Then
				lItemsFound = 0
			Else
				lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
			End If
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
			' Display the status message.
            'developer guide no. 168
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckMandatory
	'
	' Description: Check if all mandatory fields have been entered in
	'              order for the search to proceed.
	'
	' ***************************************************************** '
	Private Function CheckMandatory() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Check all fields for data.
			
			If txtInsReference.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If txtShortName.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If txtRegistrationNumber.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If txtRiskIndex.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1455)
			cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1455)
			
			ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(1085)
			
			tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1670)
			
			lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
			lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(4000)
			
			cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
			cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
			cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			' AJM 14/08/00 - View button not required.
			'cmdView.Top = Me.Height - 1110
			
			'cmdNew.Top = Me.Height - 1110
			'cmdEdit.Top = Me.Height - 1110
			
			If cmdNavigate.Visible Then
				cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			End If
			
			Return result
		
		Catch 
			
			
			
			' Error Section.
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	' PRIVATE Methods (End)
	
	
	Private Sub cmdRelatedPartyFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelatedPartyFind.Click
		' SB 31/03/98  defect 37
		' ensure that Party look up works.
		
		' Click event for the Shortname lookup.
		
        Dim sName As String
        Dim lReturn As Integer
		
		Try 
			
			sName = txtShortName.Text.Trim()
			
			' Process the find party.
			lReturn = ProcessFindParty()
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the find short Name", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdrelatedPartyLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
		
        'Try 

        '	Dim sShortName As String = ""
        '	Dim iCount As Integer
        '	Dim lInsuranceFileCnt As Integer
        '          Dim oInsFile As Object = Nothing
        '	Dim sClassName As String = ""

        '	' Extract InsFileCnt of selected Item from Data Array
        '	sShortName = lvwSearchDetails.FocusedItem.Text

        '	For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
        '		If CStr(m_vSearchData(ACIInsReference, iCount)) = sShortName Then
        '			Exit For
        '		End If
        '	Next iCount

        '	lInsuranceFileCnt = CInt(m_vSearchData(ACIInsFileCnt, iCount))


        '	m_lReturn = g_oBusiness.GetPolicyInterface(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_sClassName:=sClassName)

        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Interface class name.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        '		Exit Sub
        '	End If

        '	' Create Interface object (will be Read Only by default)

        '	m_lReturn = g_oObjectManager.GetInstance(oObject:=oInsFile, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create object '" & sClassName & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        '		Exit Sub
        '	End If

        '	' Set Primary key & process Interface

        '	oInsFile.InsuranceFileCnt = lInsuranceFileCnt

        '	m_lReturn = oInsFile.Start()

        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process '" & sClassName & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        '		Exit Sub
        '	End If

        '	' Destroy Interface object

        '          oInsFile.Dispose()
        '	oInsFile = Nothing

        'Catch excep As System.Exception




        '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdView_Click event.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        '	Exit Sub

        'End Try
		
	End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iPMBFindInsurance.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Check if the search contains more or equal
			' to the miniumum search length.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'SJ 19/04/2004 - start
			m_lReturn = CType(CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				Exit Sub
			End If
			m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				Exit Sub
			End If
			
			m_lReturn = CType(AllowOtherBranchesToViewPolicies(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				Exit Sub
			End If
			'SJ 19/04/2004 - end
			
			If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
				' No supplied data so cannot
				' continue with the search.
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to terminate the interface", MainModule.ACApp, ACClass, "Form_QueryUnload", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try
		
	End Sub
	
	Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMainTab
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the
							' first tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
								' Display the previous tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
							End If
						End If
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the last tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
						Else
							' Check we are not on the
							' last tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
								' Display the next tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
							End If
						End If
						
					Case Keys.Home
						' Home key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
				End Select
			End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
		
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		m_oAllowBranches = Nothing
		m_oValidSource = Nothing
	End Sub
	
	Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click
		
		Dim sShortName As String = ""
		Dim iCount As Integer
		
		If lvwSearchDetails.Items.Count > 0 Then
			sShortName = lvwSearchDetails.FocusedItem.Text
			
			' loop around and get the other details...
			For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				
				' RAG 2002-04-08
				' Need a trim in here in case the array values have trailing spaces.
				'If (m_vSearchData(ACIInsReference, iCount%) = sShortName) Then
				If CStr(m_vSearchData(ACIInsReference, iCount)).Trim() = sShortName.Trim() Then
					Exit For
				End If
			Next iCount
			
			' stick the other details in here...?
			
			txtInsReference.Text = CStr(m_vSearchData(ACIInsReference, iCount))
			
			VB6.SetDefault(cmdOK, True)
			
			' Activate View button
			' AJM 14/08/00 - Ignore view button, not required here
			' cmdView.Enabled = True
		End If
		
	End Sub
	
	Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		If KeyCode <> 13 Then
			VB6.SetDefault(cmdOK, False)
		End If
		
	End Sub
	
	Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		Dim sShortName As String = ""
		Dim iCount As Integer
		
		If KeyAscii = 13 And lvwSearchDetails.Items.Count > 0 Then
			
			sShortName = lvwSearchDetails.FocusedItem.Text
			
			' loop around and get the other details...
			For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				If CStr(m_vSearchData(ACIInsReference, iCount)).Trim() = sShortName.Trim() Then
					Exit For
				End If
			Next iCount
			
			' stick the other details in here...?
			
			txtInsReference.Text = CStr(m_vSearchData(ACIInsReference, iCount))
			
			VB6.SetDefault(cmdOK, True)
			
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub optAllTypes_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAllTypes.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			If m_sCallingAppName = "iPMUQUoteCollectionProcess" Then
				m_sInsFileType = "ALLQUOTE"
			Else
				m_sInsFileType = ""
			End If
            m_iFileType = ACAllTypes
		End If
	End Sub
	
	Private Sub optMTAQuote_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optMTAQuote.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			m_sInsFileType = "MTAQUOTE"
            m_iFileType = ACMTAQuote
		End If
	End Sub
	
	'WPR12- Enhancement Quote Collection Process
	Private Sub optMTAReins_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optMTAReins.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			m_sInsFileType = "MTAQREINS"
            m_iFileType = ACMTAQREINS
		End If
	End Sub
	
	Private Sub optPolicy_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optPolicy.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			m_sInsFileType = "POLICY"
            m_iFileType = ACPolicy
		End If
	End Sub
	
	Private Sub optQuote_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optQuote.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			m_sInsFileType = "QUOTE"
            m_iFileType = ACQuote
		End If
	End Sub
	
	Private Sub optRenewal_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optRenewal.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			
			m_sInsFileType = "RENEWAL"
            m_iFileType = ACRenewal
		End If
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				'        If (.Tab < cmdNext.Count) Then
				'            cmdNext(.Tab).Default = True
				'        Else
				'            cmdOK.Default = True
				'        End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				Application.DoEvents()
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
					m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
				End If
			End With
		
		Catch 
			
			
			
			' Error Section.
			
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
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
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
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
	
	Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
		
        Dim sWildcardErrorMessage As String = ""
        ' Click event of the Cancel button.
		
		Try 
            m_vSearchData = Nothing
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
		
			'Check wildcard searches
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtInsReference.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
				txtInsReference.Focus()
				Exit Sub
				
			End If
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtRiskIndex.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
				txtRiskIndex.Focus()
				Exit Sub
				
			End If
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtShortName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
				txtShortName.Focus()
				Exit Sub
				
			End If
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtRegistrationNumber.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
				txtRegistrationNumber.Focus()
				Exit Sub
				
			End If
			
			'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
			
			
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
			End If
			
			If lvwSearchDetails.Items.Count > 0 Then
				VB6.SetDefault(cmdFindNow, False)
				VB6.SetDefault(cmdOK, False)
			End If
			
			' Set the focus.
			lvwSearchDetails.Focus()
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

		' Click event of the New Search button.
		
		Try 
			
			' Clear the interface details.
			m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to clear the interface details.
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
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
	
	Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
		
		' Double click event for the search details.
		
		Try 
			
			' Check if there are any items available.
			If lvwSearchDetails.Items.Count = 0 Then
				Exit Sub
			End If
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub

	Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick

		Dim lvwSelectedItem As ListViewItem = Nothing

		If lvwSearchDetails.SelectedItems IsNot Nothing AndAlso lvwSearchDetails.SelectedItems.Count > 0 Then
			lvwSelectedItem = lvwSearchDetails.SelectedItems(0)
		End If
		StoreHScrollValue()
		ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
		RecoverHorizontalScroll()
		If lvwSelectedItem IsNot Nothing Then
			lvwSelectedItem.Selected = True
			lvwSelectedItem.EnsureVisible()
		End If




	End Sub
	' PRIVATE Events (End)

	Private Sub txtInsReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsReference.Enter
		
		' Hightlight any text.
		iPMFunc.SelectText(txtInsReference)
		
	End Sub
	
	Private Sub txtInsReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsReference.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		' Check mandatory.
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	
	Private Sub txtRegistrationNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegistrationNumber.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		' Check mandatory.
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub txtRiskIndex_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRiskIndex.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		' Check mandatory.
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub txtShortName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.Enter
		
		' Hightlight any text.
		iPMFunc.SelectText(txtShortName)
		
		' Change the default button.
		'cmdRelatedPartyFind.Default = True
		
	End Sub
	
	Private Sub txtShortName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		' Check mandatory.
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub cmbType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbType.SelectedIndexChanged
		
		' Check mandatory.
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	Private Sub txtShortName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.Leave
		' SB 31/03/98 Defect 37
		' validate the short name

		'    If txtShortName.Text <> "" Then
		'        lErrorValue& = ValidateLookups()
		'    Else
		'        m_lInsHolderCnt& = 0
		'    End If
		'
		'    cmdFindNow.Default = True


		If txtShortName.Text = "" Then
			m_lInsHolderCnt = 0
		End If

		'SB 03/04/98 Correction to setting of default button
		' Defect 164

		' Change the default button.
		VB6.SetDefault(cmdFindNow, True)


	End Sub

	<DllImport("user32.dll")>
	Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

	End Function

	<DllImport("user32.dll")>
	Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

	End Function
	<DllImport("user32.dll")>
	Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

	End Function
	'Store the horizontal scroll value.
	Private Sub StoreHScrollValue()
		hScrollValue = GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
	End Sub
	'Recover the old scroll position
	Private Sub RecoverHorizontalScroll()
		LockWindowUpdate(lvwSearchDetails.Handle)
		'Calculate the value the scroll needs to scroll back.
		Dim dx As Integer = hScrollValue - GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
		'Send the scroll message.
		Dim b As Boolean = SendMessage(lvwSearchDetails.Handle, LVM_SCROLL, dx, 0)
		LockWindowUpdate(IntPtr.Zero)

	End Sub
End Class
