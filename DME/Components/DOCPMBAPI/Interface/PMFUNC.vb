Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Module PMFUNC
	'*********************************************************************
	'
	' General Global Declaration
	'
	'*********************************************************************
	'
	
	
	Public g_ssSnapshot As DAO.Snapshot
	Public g_dsDynaset As DAO.Dynaset
	
	Public g_sDBRoot As String = ""
	Public g_sDBName As String = ""
	Public g_sHistoryRoot As String = ""
	
	Public g_iUserAccessLevel As Integer ' User Access Level
	Public g_sUsername As String = "" ' User Name
	Public g_iAdministrator As Integer ' Maintanance level access
	
	Public g_lCabinetNumber As Integer
	Public g_lDrawerNumber As Integer
	Public g_lDocumentNumber As Integer
	
	Public g_sPageNames() As String
	
	Public g_iPageType As Integer
	
	Public g_sGroupIniName As String = ""
	Public g_sCommand As String = ""
	Public g_iAtomNumber As Integer
	
	Public g_iLevelAccess As Integer
	Public g_iLoginID As Integer
	
	Public g_iNextPrevDoc As Integer
	
	Public g_sSearchData() As String
	Public g_lCurrentSearch As Integer
	Public g_iSearchForm As Integer
	
	Public g_utInfo As DMSDDB.g_utSearchInfo = DMSDDB.g_utSearchInfo.CreateInstance()
	Public g_lKeywordNumbers() As Integer
	
	Public g_sAppType As New FixedLengthString(4)
	
	Public g_iRC As Integer
	Public g_iTmp As Integer
	
	Public Const PIXEL As Integer = 3
	Public Const EXPAND_TREE As Integer = 1
	Public Const COLLAPSE_TREE As Integer = 2
	
	Public Const TYPE_TIF As Integer = 1
	Public Const TYPE_RTF As Integer = 2
	Public Const TYPE_TEXT As Integer = 3
	
	' Number of times to retry a database update
	Public Const UPDATERETRY As Integer = 5
	Public Const RETRYDELAY As Double = 0.25
	
	Public Const PM_FAIL As Integer = 0
	Public Const PM_FALSE As Integer = 0
	Public Const PM_OK As Integer = -1
	Public Const PM_TRUE As Integer = -1
	Public Const PM_CANCEL As Integer = -2
	Public Const PM_DATACHANGED As Integer = -3
	Public Const PM_DUPLICATEKEY As Integer = -4
	Public Const PM_RETRY As Integer = -5
	Public Const PM_DMSAPI As Integer = -6
	Public Const PM_NOPAGES As Integer = -7
	Public Const PM_DOCTYPESEARCH As Integer = -8
	Public Const PM_DOCTYPENORMAL As Integer = -9
	Public Const PM_ERROR As Integer = -99
	
	' data types for CreateRecordSet()
	Public Const RS_DYNASET As Integer = 1
	Public Const RS_SNAPSHOT As Integer = 2
	Public Const RS_TABLE As Integer = 3
	' database calling convetion to use
	Public Const SQL_LOCAL As Integer = 0
	Public Const SQL_PASSTHROUGH As Integer = 1
	
	' Some constant strings used in MsgBox captions
	Const MSG_DBWARNING As String = "Database Warning"
	Const MSG_DBERROR As String = "Database Error"
	Const MSG_DBINFO As String = "Database Information"
	Const MSG_DBREASON As String = " This may be because the database is currently in use by another user. The operation may work if you retry."
	
	'Media stuff ...
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure utDeviceMap
		<VBFixedString(2),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2)> _
		Public sDrive As FixedLengthString
		Dim lDevice As Integer
		Public Shared Function CreateInstance() As utDeviceMap
			Dim result As New utDeviceMap
			result.sDrive = New FixedLengthString(2)
			Return result
		End Function
	End Structure
	
	Public g_utDeviceMappings() As utDeviceMap = Nothing
	
	'*********************************************************************
	'
	' Module Name: CenterForm
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Center form to window or MDI form.
	'
	'*********************************************************************
	'
	'Sub CenterForm(frmCenter As Form)
	'Dim lHeight, lWidth, lTop, lLeft, lleftOffset, lTopOffset
	'
	'1001:
	'
	'    If (frmCenter.MDIChild) Then
	''        lHeight = frmMain.ScaleHeight
	''        lWidth = frmMain.ScaleWidth
	''        lTop = frmMain.Top
	''        lLeft = frmMain.Left
	'    Else
	'        lHeight = Screen.Height
	'        lWidth = Screen.Width
	'        lTop = 0
	'        lLeft = 0
	'    End If
	'
	'    ' Get Left Offset
	'    lleftOffset = ((lWidth - frmCenter.Width) / 2) + lLeft
	'
	'    If (lleftOffset + frmCenter.Width > Screen.Width Or lleftOffset < 100) Then
	'        lleftOffset = 100
	'    End If
	'
	'    ' Get Top Offset
	'    lTopOffset = ((lHeight - frmCenter.Height) / 2) + lTop
	'
	'    If (lTopOffset + frmCenter.Height > Screen.Height Or lTopOffset < 100) Then
	'        lTopOffset = 100
	'    End If
	'
	'    ' Center the form
	'    frmCenter.Move lleftOffset, lTopOffset
	'
	'    PaintControls frmCenter
	'
	'End Sub
	
	Function CheckLicence() As Integer
		Dim result As Integer = 0
		
		
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT * FROM licence")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
				' Check licence key is OK
                result = (LicenceOK(ssSnapShot("product_code").Value.Trim(), ssSnapShot("company_num").Value, ssSnapShot("licence_limit").Value, ssSnapShot("licence_key").Value.Trim()))
			Else
				result = False
			End If
			
			ssSnapShot.Close()
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: CheckPageType
	'      Author: simonb
	'        Date: 2/05/95
	'
	' Description: Checks the page type, tif, word etc.
	'
	'*********************************************************************
	'
	Function CheckPageType(ByRef sPageName As String) As Integer
		Dim result As Integer = 0
		
		

		Try 
			
			
			Select Case sPageName.Substring(sPageName.Length - 3)
				Case "TIF"
					result = TYPE_TIF
				Case "RTF", "DOC"
					result = TYPE_RTF
				Case "TXT"
					result = TYPE_TEXT
			End Select
			
			Return result
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	
	Function CheckUnique(ByRef FormName As String) As Integer
		
		
		Dim X As Integer
		
		' See if a previous existance is present
		Dim Handle As Integer = FindWindow(CStr(0), FormName)
		
		If Handle = 0 Then
			' if there is, -1 is a true value.
			Return -1
		Else
			Return 0
		End If
		
	End Function
	
	Function CommitDatabase() As Integer
		
		' Loops UPDATERETRY number of times or until the DeleteDynaset1 works
		' Prompts user to retry after each attempt.
		
		Dim result As Integer = 0
		Dim iRetVal, iTmp As Integer
		
		' Loop until we reach our maximum number of retries
		
		For iTmp = 1 To UPDATERETRY
			
			iRetVal = CommitDatabase1()
			
			
			Select Case iRetVal
				Case PM_RETRY
					' Auto retry if this is the first time. Otherwise ask user what to do
					
					Select Case db_MsgBox("Unable to commit changes to database." & MSG_DBREASON, MB_ICONEXCLAMATION + MB_RETRYCANCEL, MSG_DBWARNING, Information.Err().Number)
						Case IDCANCEL
							Return PM_CANCEL
						Case PM_DMSAPI
							result = PM_CANCEL
					End Select
					
				Case Else
					' It either worked or was a serious error that the calling function
					' will want to sort out.
					
					Return iRetVal
			End Select
			
			sleep(1)
			
		Next iTmp
		
		' Retries exceeded. Behave as though the user wanted us to give up.
		
		result = PM_CANCEL
		iTmp = db_MsgBox("Unable to commit changes to the database.", MB_ICONSTOP, MSG_DBERROR, 0)
		
		Return result
		
	End Function
	
	Function CommitDatabase1() As Integer
		
		' stick in a line number for debugging
		
		Try 
			
			DAO_DBEngine_definst.CommitTrans()
			DAO_DBEngine_definst.FreeLocks()
			
			Return PM_TRUE
		
		Catch 
			
			
			
			DAO_DBEngine_definst.FreeLocks()
			
			
			
			Select Case Information.Err().Number
				Case 3046, 3186, 3187, 3188, 3260 'Can't update, currently locked by ...
					
					Return PM_RETRY
					
				Case Else
					
					Return Information.Err().Number
					
			End Select
		End Try
		
	End Function
	
	'Function CopyFile%(sFileIn$, sFileOut$)
	'
	'    On Error GoTo ErrCopyFile
	'
	'    FileCopy sFileIn$, sFileOut$
	'
	'    CopyFile = PM_TRUE
	'
	'    Exit Function
	'
	'ErrCopyFile:
	'
	'    CopyFile = PM_FALSE
	'    Exit Function
	'
	'End Function
	
	Function db_MsgBox(ByRef sMsgText As String, ByRef iMsgType As Integer, ByRef sMsgCaption As String, ByRef iErr As Integer) As Integer
		' If g_sUsername = DMSAPI then we don't display the message. We then return PM_DMSAPI
		' This routine is useful for bypassiing messages in interactive mode for none interactive
		' programs.
		
		Dim result As Integer = 0
		result = PM_DMSAPI
		
		If g_sUsername.ToUpper() = "DMSAPI" Then
			Return result
		End If
		
		
		Return Interaction.MsgBox(sMsgText & Strings.Chr(10).ToString() & Conversion.ErrorToString(iErr), iMsgType, sMsgCaption)
		
	End Function
	
	Function DeleteDynaset(ByRef dsName As DAO.Dynaset) As Integer
		
		' Loops UPDATERETRY number of times or until the DeleteDynaset1 works
		' Prompts user to retry after each attempt.
		
		Dim result As Integer = 0
		Dim iRetVal, iTmp As Integer
		
		' Loop until we reach our maximum number of retries
		
		For iTmp = 1 To UPDATERETRY
			
			iRetVal = DeleteDynaset1(dsName)
			
			
			Select Case iRetVal
				Case PM_RETRY
					' Auto retry if this is the first time. Otherwise ask user what to do
					
					If iTmp > 2 Then
						If db_MsgBox("The delete request did not work." & MSG_DBREASON, MB_ICONEXCLAMATION + MB_RETRYCANCEL, MSG_DBWARNING, Information.Err().Number) = IDCANCEL Then
							Return PM_CANCEL
						End If
					End If
					
				Case Else
					' It either worked or was a serious error that the calling function
					' will want to sort out.
					
					Return iRetVal
			End Select
			
			sleep(1)
			
		Next iTmp
		
		' Retries exceeded. Behave as though the user wanted us to give up.
		
		result = PM_CANCEL
		iTmp = db_MsgBox("Unable to perform the delete request on the database.", MB_ICONSTOP, MSG_DBERROR, 0)
		
		Return result
		
	End Function
	
	Function DeleteDynaset1(ByRef dsName As DAO.Dynaset) As Integer
		' stick in a line number for debugging
		
		Try 
			
			dsName.Delete()
			DAO_DBEngine_definst.FreeLocks()
			
			Return PM_TRUE
		
		Catch 
			
			
			
			DAO_DBEngine_definst.FreeLocks()
			
			
			Select Case Information.Err().Number
				Case 3046, 3186, 3187, 3188, 3260 'Can't update, currently locked by ...
					
					Return PM_RETRY
					
				Case 3197 ' data has changed
					Return PM_DATACHANGED
					
				Case 3022 ' Duplicate Key
					Return PM_DUPLICATEKEY
					
				Case Else
					Return Information.Err().Number
					
			End Select
		End Try
		
	End Function
	
	Function DoubleAmpersand(ByRef sLine As String) As String
		
		Dim sTmp As New StringBuilder
		
		For iTmp As Integer = 1 To sLine.Trim().Length
			
			sTmp.Append(Mid(sLine, iTmp, 1))
			
			If Mid(sLine, iTmp, 1) = "&" Then
				
				If Mid(sLine, iTmp + 1, 1) = "&" Then
					
					sTmp.Append("&")
					iTmp += 1
					
				Else
					
					sTmp.Append("&")
					
				End If
				
			End If
			
			If iTmp = sLine.Trim().Length Then
				Exit For
			End If
			
		Next iTmp
		
		
		Return sTmp.ToString()
		
		
		
		Return sLine
		
	End Function
	
	Function FindFreeChildIndex() As Integer
		Dim result As Integer = 0
		
		
		Dim ArrayCount As Integer
		
		Try 
			
			ArrayCount = g_utChildInfo.GetUpperBound(0)
			
			' Cycle throught the ChildInfo array. If one of the
			' Children has been deleted, then return that
			' index.
			For iCntr As Integer = 1 To ArrayCount
				If g_utChildInfo(iCntr).iDeleted Then
					result = iCntr
					g_utChildInfo(iCntr).iDeleted = False
					g_utChildInfo(iCntr).dDocDate = 0
					
					Return result
				End If
			Next 
			
			ReDim Preserve g_utChildInfo(ArrayCount + 1)
			
			Return g_utChildInfo.GetUpperBound(0)
		
		Catch 
			
			
			ReDim g_utChildInfo(1)
			g_utChildInfo(1).iDeleted = True
			ArrayCount = 1
			


			
			Return result
		End Try
	End Function
	
	Function GetAccess(ByRef iLevel As Integer, ByRef lSearchNumber As Integer, ByRef iPassword As Integer) As Integer
		Dim ErrGetPassword As Boolean = False
		Dim ErrGetAccessLevel As Boolean = False
		Dim result As Integer = 0
		
		
		Dim sLevelDesc, sSQLQuery As String
		Dim ssTmpSnapshot As DAO.Snapshot
		Dim iPasswordFlag As Integer
		Dim sLevelName As String = ""
		
		result = True
		
		Dim GetPass As New frmGetPassword
		
		If lSearchNumber > 0 Then
			' No password
			Return result
		End If
        'developer guide no.101
        Dim PrevMouse As Cursor = Cursor.Current
		
		Dim lTmpNumber As Integer = CInt(Math.Abs(lSearchNumber))
		
		Select Case (iLevel)
			Case CABINET
				sLevelDesc = "Cabinet"
				sSQLQuery = "SELECT access_level, cabinet_name FROM cabinet WHERE cabinet_num = " & lTmpNumber
			Case DRAWER
				sLevelDesc = "Drawer"
				sSQLQuery = "SELECT access_level, drawer_name FROM drawer WHERE drawer_num = " & lTmpNumber
			Case FOLDER
				sLevelDesc = "Folder"
				sSQLQuery = "SELECT access_level, folder_name FROM folder WHERE folder_num = " & lTmpNumber
			Case DOCUMENT
				sLevelDesc = "Document"
				sSQLQuery = "SELECT access_level, doc_name FROM document WHERE doc_num = " & lTmpNumber
		End Select
		
		' Check Access Level
		
		Try 
			ErrGetAccessLevel = True
			ErrGetPassword = False
			
			ssTmpSnapshot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssTmpSnapshot.RecordCount = 0 Then
				result = False
				ssTmpSnapshot.Close()
				ssTmpSnapshot = Nothing
				Return result
			Else
                sLevelName = ssTmpSnapshot(1).Value.Trim()
				If Conversion.Val(ssTmpSnapshot(0)) < g_iUserAccessLevel Then
					'            If (iPassword) Then
					Interaction.MsgBox("Access denied on " & sLevelDesc & ", " & sLevelName, MB_ICONEXCLAMATION, "DocuMaster Access Error")
					'            End If
					result = False
					ssTmpSnapshot.Close()
					ssTmpSnapshot = Nothing
					Return result
				End If
			End If
			
			ssTmpSnapshot.Close()
			ssTmpSnapshot = Nothing
			
			ErrGetPassword = True
			ErrGetAccessLevel = False
			
			If iPassword Then
				' Enter password
				
				iPasswordFlag = True
				Cursor.Current = Cursors.Default
				
				GetPass.txtPassword.Text = ""

                GetPass.lbl3Title.Text = " " & sLevelDesc & ": " & sLevelName
				
				While (iPasswordFlag)
					GetPass.ShowDialog()
					
					' Check Password Entered
					If GetPass.txtPassword.Text <> "" Then
						If Not (CheckPassword(iLevel, lSearchNumber, GetPass.txtPassword.Text)) Then
							Interaction.MsgBox(sLevelDesc & " password incorrect!", MB_ICONEXCLAMATION, "DocuMaster Password Entry")
							GetPass.txtPassword.Text = ""
						Else
							iPasswordFlag = False
						End If
					Else
						' User Pressed Cancel
						GetPass.Close()
						result = False
						iPasswordFlag = False

						Cursor.Current = PrevMouse
						Return result
					End If
				End While
				
				GetPass.Close()
			End If
			

			Cursor.Current = PrevMouse
			Return result
		
		Catch excep As System.Exception
			If Not ErrGetPassword And Not ErrGetAccessLevel Then
				Throw excep
			End If
			
			If ErrGetAccessLevel Then
				
				
				ssTmpSnapshot.Close()
				Return False
				
			End If
			If ErrGetPassword Or ErrGetAccessLevel Then
				
				
				GetPass.Close()
				result = False

				Cursor.Current = PrevMouse
				Return result
				
			End If
		End Try
	End Function
	
	Function GetLicenceLimit() As Integer
		Dim result As Integer = 0
		
		
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT licence_limit FROM licence")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
                result = ssSnapShot("licence_limit").Value
			Else
				result = 0
			End If
			
			ssSnapShot.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function GetPageType(ByRef lDocumentNumber As Integer) As Integer
		Dim result As Integer = 0
		
		
		Dim ssSnapShot As DAO.Snapshot
		
		
		Try 
			
			'Get page name from document number
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT doc_type FROM document WHERE doc_num = " & lDocumentNumber)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
                Select Case ssSnapShot("doc_type").Value
                    Case "I"
                        result = TYPE_TIF
                    Case "R"
                        result = TYPE_RTF
                    Case "L", "N"
                        result = TYPE_TEXT
                End Select
			End If
			
			ssSnapShot.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'Function KillFile(sFile$) As Integer
	'
	'    On Error GoTo ErrKillFile
	'
	'    Kill sFile$
	'
	'    KillFile = PM_TRUE
	'    Exit Function
	'
	'ErrKillFile:
	'
	'    KillFile = PM_FALSE
	'    Exit Function
	'
	'End Function
	
	Function LoginAllowed() As Integer
		Dim sTmp As String = ""
		Dim iLen As Integer
		
		Try 
			
			'see if DMS Speed wants us off
			sTmp = New String(" "c, 129)
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("Setting")
			Try 
				iLen = GetPrivateProfileString("DMSLock", tmpPtr, "", sTmp, 128, g_sDBRoot & "\dmsdb.lck")
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			
			'if the lock file exists then we must logout...
			
			If iLen > 0 Then
				Return PM_FALSE
			Else
				Return PM_TRUE
			End If
		
		Catch 
		End Try
		
		
		
		Return PM_TRUE
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: Message
	'      Author: simonb
	'        Date: 20/04/95
	'
	' Description: Displays a message in the status bar.
	'
	'*********************************************************************
	'
	Sub message(ByRef frmFormName As frmExistingLogin, ByRef sText As String)
		Dim pan3StatusBar As Object
		
		

        frmFormName.lbl3StatusBar.Text = " " & sText
		
	End Sub
	
	Sub PaintControls(ByRef fTmp As Form)
		
		'This dosnt work properly 'cos most of the 3D controls
		'  don't have a BackColor property.
		Exit Sub
		
		Dim iPaint As Integer
		

		Try  'GoTo ErrPaintControls
			


			For i As Integer = 0 To ContainerHelper.Controls(fTmp).Count
				iPaint = True
				

				If TypeOf ContainerHelper.Controls(fTmp)(i) Is TextBox Then
					iPaint = False
				ElseIf TypeOf ContainerHelper.Controls(fTmp)(i) Is ComboBox Then 
					iPaint = False
				ElseIf TypeOf ContainerHelper.Controls(fTmp)(i) Is AxMSOutl.AxOutline Then 
					iPaint = False
				ElseIf TypeOf ContainerHelper.Controls(fTmp)(i) Is ListBox Then 
					iPaint = False
				End If
				
				If iPaint Then

					ContainerHelper.Controls(fTmp)(i).BackColor = Color.Silver 'Light Gray
				End If
				
			Next i
			
			Try 
				Exit Sub
			
			Catch 
				
				
				
				Exit Sub
			End Try
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	'*********************************************************************
	'
	' Module Name: PasswordStatus
	'      Author: simonb
	'        Date: 20/04/95
	'
	' Description: Displays a message in the password status bar.
	'
	'*********************************************************************
    '
    'developer guide no.101
    Sub PasswordStatus(ByRef frmFormName As Object, ByRef sText As String)
        Dim pan3PasswordStatus As Object




        frmFormName.pan3PasswordStatus = sText

    End Sub
	
	Sub Pause(ByRef iNumberSeconds As Integer)
		
		
		
		For iTmp As Integer = 1 To iNumberSeconds
			
			For iTmp2 As Integer = 0 To 25000
			Next iTmp2
			
			Application.DoEvents()
		Next iTmp
		
	End Sub
	
	Sub sleep(ByRef iTime As Integer)
		
		
		Dim lTimer As Integer = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
		
		While ((lTimer + iTime) > DateTime.Now.TimeOfDay.TotalSeconds)
			Application.DoEvents()
		End While
		
	End Sub
	
	Function StripExtension(ByRef sFilename As String) As String
		
		Dim sTmpFilename As New StringBuilder
		
		Try 
			
			sTmpFilename = New StringBuilder("")
			
			For iCntr As Integer = 1 To sFilename.Length
				If sFilename.Substring(iCntr - 1, 1) = "." Then
					' Found start of extension, that it laddy!
					Exit For
				Else
					sTmpFilename.Append(sFilename.Substring(iCntr - 1, 1))
				End If
			Next iCntr
			
			
			Return sTmpFilename.ToString()
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function UpdateDynaset(ByRef dsName As DAO.Dynaset) As Integer
		
		' Loops UPDATERETRY number of times or until the UpdateDynaset1 works
		' Prompts user to retry after each attempt.
		
		Dim result As Integer = 0
		Dim iRetVal, iTmp As Integer
		
		' Loop until we reach our maximum number of retries
		
		For iTmp = 1 To UPDATERETRY
			
			iRetVal = UpdateDynaset1(dsName)
			
			
			Select Case iRetVal
				Case PM_RETRY
					' Auto retry if this is the first time. Otherwise ask user what to do
					
					If iTmp > 2 Then
						If db_MsgBox("The update did not work." & MSG_DBREASON, MB_ICONEXCLAMATION + MB_RETRYCANCEL, MSG_DBWARNING, Information.Err().Number) = IDCANCEL Then
							Return PM_CANCEL
						End If
					End If
					
				Case Else
					' It either worked or was a serious error that the calling function
					' will want to sort out.
					
					Return iRetVal
			End Select
			
			sleep(1)
			
		Next iTmp
		
		' Retries exceeded. Behave as though the user wanted us to give up.
		
		result = PM_CANCEL
		iTmp = db_MsgBox("Unable to perform the update request on the database.", MB_ICONSTOP, MSG_DBERROR, 0)
		
		Return result
		
	End Function
	
	Function UpdateDynaset1(ByRef dsName As DAO.Dynaset) As Integer
		' stick in a line number for debugging
		
		Try 
			
			dsName.Update()
			DAO_DBEngine_definst.FreeLocks()
			Return PM_TRUE
		
		Catch 
			
			
			
			DAO_DBEngine_definst.FreeLocks()
			
			
			
			Select Case Information.Err().Number
				Case 3046, 3186, 3187, 3188, 3260 'Can't update, currently locked by ...
					
					Return PM_RETRY ' and retry the update
					
				Case 3197 ' data has changed
					Return PM_DATACHANGED
					
					
				Case 3022 ' Duplicate Key
					Return PM_DUPLICATEKEY
					
				Case Else
					Return Information.Err().Number
					
			End Select
		End Try
		
	End Function
	
	Function UpdateTable(ByRef tbName As DAO.Table) As Integer
		
		' Loops UPDATERETRY number of times or until the UpdateTable1 works
		' Prompts user to retry after each attempt.
		
		Dim result As Integer = 0
		Dim iRetVal, iTmp As Integer
		
		' Loop until we reach our maximum number of retries
		
		For iTmp = 1 To UPDATERETRY
			
			iRetVal = UpdateTable1(tbName)
			
			
			Select Case iRetVal
				Case PM_RETRY
					' Auto retry if this is the first time. Otherwise ask user what to do
					
					If iTmp > 2 Then
						If db_MsgBox("The update of a table in the database did not work." & MSG_DBREASON, MB_ICONEXCLAMATION + MB_RETRYCANCEL, MSG_DBWARNING, Information.Err().Number) = IDCANCEL Then
							Return PM_CANCEL
						End If
					End If
					
				Case Else
					' It either worked or was a serious error that the calling function
					' will want to sort out.
					
					Return iRetVal
			End Select
			
			sleep(1)
			
		Next iTmp
		
		' Retries exceeded. Behave as though the user wanted us to give up.
		
		result = PM_CANCEL
		iTmp = db_MsgBox("Unable to perform the table update request on the database.", MB_ICONSTOP, MSG_DBERROR, 0)
		
		Return result
		
	End Function
	
	Function UpdateTable1(ByRef tbName As DAO.Table) As Integer
		' stick in a line number for debugging
		
		Try 
			
			tbName.Update()
			DAO_DBEngine_definst.FreeLocks()
			
			Return PM_TRUE
		
		Catch 
			
			
			
			DAO_DBEngine_definst.FreeLocks()
			
			
			
			Select Case Information.Err().Number
				Case 3046, 3186, 3187, 3188, 3260 'Can't update, currently locked by ...
					
					Return PM_RETRY
					
				Case 3197 ' data has changed
					Return PM_DATACHANGED
					
					
				Case 3022 ' Duplicate Key
					Return PM_DUPLICATEKEY
					
					
				Case Else
					Return Information.Err().Number
					
			End Select
		End Try
		
	End Function
End Module