Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Module DMSJAPI
	
	'file process flags...
	Public Const PM_NONE As Integer = 1
	Public Const PM_NORMAL As Integer = 2
	Public Const PM_RECOVER As Integer = 3
	Public Const PM_ERRRETRY As Integer = 4
	
	Public fhErrorlog As Integer
	Public fhJournalErrorlog As Integer
	Public fhJournalBad As Integer
	
	Public g_iCompact As Integer
	Public g_sCurrentJournalPos As String = ""
	Public g_iIndexAccessLevel As Integer
	Public g_iIndexData As Integer
	Public g_iRunning As Integer
	
	Public Structure g_utControlData
		Dim task As String
		Dim cabinetname As String
		Dim drawername As String
		Dim foldername As String
		Dim linkfolder As String
		Dim documentname As String
		Dim keywords As String
		Dim event_Renamed As String
		Dim doctype As String
		Dim filename As String
		Dim annotation As String
		Dim access As Integer
		Dim hdbonly As Integer
		Dim username As String
		Dim emptyonly As Integer
		Dim message As String
		Dim external As Integer
		Public Shared Function CreateInstance() As g_utControlData
			Dim result As New g_utControlData
			result.task = String.Empty
			result.cabinetname = String.Empty
			result.drawername = String.Empty
			result.foldername = String.Empty
			result.linkfolder = String.Empty
			result.documentname = String.Empty
			result.keywords = String.Empty
			result.doctype = String.Empty
			result.filename = String.Empty
			result.annotation = String.Empty
			result.username = String.Empty
			result.message = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure g_utIndexListData
		Dim cabinetcode As String
		Dim cabinetname As String
		Dim drawercode As String
		Dim drawername As String
		Dim foldercode As String
		Dim foldername As String
		Public Shared Function CreateInstance() As g_utIndexListData
			Dim result As New g_utIndexListData
			result.cabinetcode = String.Empty
			result.cabinetname = String.Empty
			result.drawercode = String.Empty
			result.drawername = String.Empty
			result.foldercode = String.Empty
			result.foldername = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure g_utIndexCabinet
		Dim cabinetcode As String
		Dim CabinetNumber As Integer
		Public Shared Function CreateInstance() As g_utIndexCabinet
			Dim result As New g_utIndexCabinet
			result.cabinetcode = String.Empty
			Return result
		End Function
	End Structure
	
	Function AnnotationExists(ByRef lDocumentNumber As Integer, ByRef sAnnotationName As String) As Integer
		
		Dim result As Integer = 0
		Dim ssTmpExists As DAO.Snapshot
		
		Dim sSQLQuery As String = "SELECT doc_num FROM annotation WHERE "
		sSQLQuery = sSQLQuery & "doc_num = " & CStr(lDocumentNumber) & " AND "
		sSQLQuery = sSQLQuery & "ann_text = '" & sAnnotationName & "'"
		
		Try 
			
			ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			result = (ssTmpExists.RecordCount > 0)
			
			ssTmpExists.Close()
			Return result
		
		Catch 
			
			
			
			ssTmpExists.Close()
			Return False
		End Try
		
	End Function
	
	Function CabinetChanged(ByRef lCabinetNumber As Integer, ByRef sCabinetName As String) As Integer
		
		Dim result As Integer = 0
		Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
		
		result = False
		
		Try 
			
			If DDBChanged(CABINET, lCabinetNumber, sCabinetName) Then
				result = True
			End If
			
			' Update the history database
			' Salvo - update regardless of whether DDB changed as it's possible
			'         the Remoted History DB needs updating
			utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
			utDMSHistData.cabinetname.Value = sCabinetName
			
			If Not (UpdateHDB(MODCABINET, g_sHistoryRoot, utDMSHistData)) Then
				' Failed to create Cabinet history
				ErrorLog("ProcessControlData", "Failed to change Cabinet name, " & sCabinetName)
				result = False
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckIndexCabinet(ByRef utIndexCabinet() As g_utIndexCabinet, ByRef cabinetcode As String) As Integer
		
		Dim result As Integer = 0
		
		
		Try 
			
			For iCntr As Integer = 1 To utIndexCabinet.GetUpperBound(0)
				' Check if the cabinet code exists in me
				If utIndexCabinet(iCntr).cabinetcode.Trim() = cabinetcode.Trim() Then
					' Found a match
					result = utIndexCabinet(iCntr).CabinetNumber
					Exit For
				End If
			Next iCntr
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function CheckJournalBadFile() As Integer
		
		Dim sJournalName As String = ""
		
		Try 
			
			' Does journal.bad exist
			sJournalName = FileSystem.Dir(g_sHistoryRoot & "data\journal.bad", ATTR_NORMAL)
			
			
			Return sJournalName <> ""
		
		Catch 
		End Try
		
		
		
		Return False
		
	End Function
	
	Function CheckJournalFile() As Integer
		
		'This function will check for the existance of work files.
		'If work files exist it indicates that the API fell over while processing.
		'
		'If a journal.rty exists the the API fell over while retying import,
		'otherwise it fell over during normal processing...
		
		Dim sJournalName, sJournalCurrentName As String
		
		Try 
			
			' Check if journal.cur exists
			sJournalCurrentName = FileSystem.Dir(g_sHistoryRoot & "data\journal.cur", ATTR_NORMAL)
			
			If sJournalCurrentName <> "" Then
				g_sCurrentJournalPos = GetJournalPos(sJournalCurrentName)
			Else
				g_sCurrentJournalPos = ""
			End If
			
			' Does journal.rty already exist, were we processing the retry's ??
			'sJournalName = Dir$(g_sHistoryRoot & "data\journal.rty", ATTR_NORMAL)
			
			If FileSystem.Dir(g_sHistoryRoot & "data\journal.rty", ATTR_NORMAL) <> "" Then
				
				'    If (sJournalName <> "") Then
				' Recover from failure
				'
				Return PM_ERRRETRY
				
			ElseIf FileSystem.Dir(g_sHistoryRoot & "data\journal.wrk", ATTR_NORMAL) <> "" Then 
				
				' Does journal.wrk already exist, did we crash  ??
				'sJournalName = Dir$(g_sHistoryRoot & "data\journal.wrk", ATTR_NORMAL)
				
				'If (sJournalName <> "") Then
				' Recover from failure
				'
				'            ' Does journal.dms exist
				'            sJournalName = Dir$(g_sHistoryRoot & "data\journal.dms", ATTR_NORMAL)
				
				Return PM_RECOVER
				
				'End If
			Else
				
				' Does journal.dms exist
				sJournalName = FileSystem.Dir(g_sHistoryRoot & "data\journal.dms", ATTR_NORMAL)
				
				If sJournalName.Trim() = "" Then
					' Journal.dms doesn't exist
					Return PM_NONE
				Else
					Return PM_NORMAL
				End If
				
			End If
		
		Catch 
		End Try
		
		
		
		Return PM_FALSE
		
	End Function
	
	Function CheckLockFile() As Integer
		
		Dim result As Integer = 0
		Dim sLockName, sJournalName As String
		
		result = True
		Try 
			
			For iWaitLoop As Integer = 1 To 2
				' Does the lock.dms file exist
				sLockName = FileSystem.Dir(g_sHistoryRoot & "data\lock.dms", ATTR_NORMAL)
				
				If sLockName <> "" Then
					' Lock file exists
					'
					' Check if the journal.dms file exists
					sJournalName = FileSystem.Dir(g_sHistoryRoot & "data\journal.dms", ATTR_NORMAL)
					
					If sJournalName = "" Then
						' Journal.dms file doesn't exist
						'
						' Wait 10 seconds, and try again
						Pause(10)
					Else
						' Journal.dms file exists
						Exit For
					End If
				Else
					' lock.dms file doesn't exist
					Exit For
				End If
			Next iWaitLoop
			
			Return result
		
		Catch 
			
			
			
			JournalErrorLog("CheckLockFile", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
			Return False
		End Try
		
	End Function
	
	Function CheckLogins() As Integer
		
		Dim result As Integer = 0
		Dim ssLogins As DAO.Snapshot
		
		result = True
		
		Try 
			
			ssLogins = g_dbDDB.CreateSnapshot("SELECT user_name FROM login")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssLogins.RecordCount = 1 Then
				If ssLogins("user_name") <> "DMSAPI" Then
					' Not OK to continue, because its
					' not me running
					result = False
				End If
			Else
				result = False
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Sub CloseErrorLog(ByRef ControlName As String)
		
		Try 
			
			If FileSystem.LOF(fhErrorlog) = 0 Then
				FileSystem.FileClose(fhErrorlog)
				If KillFile(g_sHistoryRoot & "tmp\" & ControlName & ".log") = PM_FALSE Then
				End If
			Else
				FileSystem.FileClose(fhErrorlog)
			End If
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Function CloseIndexListData() As Integer
		
		Dim result As Integer = 0
		result = True
		
		Try 
			
			' Close journal file
			FileSystem.FileClose(g_iIndexData)
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Sub CloseJournalBad()
		
		Try 
			
			If FileSystem.LOF(fhJournalBad) = 0 Then
				FileSystem.FileClose(fhJournalBad)
				
				If KillFile(g_sHistoryRoot & "data\journal.bad") = PM_FALSE Then
				End If
			Else
				FileSystem.FileClose(fhJournalBad)
			End If
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Sub CloseJournalErrorLog()
		
		Try 
			
			If FileSystem.LOF(fhJournalErrorlog) = 0 Then
				FileSystem.FileClose(fhJournalErrorlog)
				If KillFile(g_sHistoryRoot & "data\journal.log") = PM_FALSE Then
				End If
			Else
				FileSystem.FileClose(fhJournalErrorlog)
			End If
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Function CommitHDB(ByRef iRetryErrors As Integer) As Integer
		Dim ErrFillStruct As Boolean = False
		Dim ErrCommitHDB As Boolean = False
		
		Dim result As Integer = 0
		Dim utDMSHistParams As DMSHIST.g_utDMSHistParams = DMSHIST.g_utDMSHistParams.CreateInstance()
		Dim dsDMSHistData As DAO.Dynaset
		Dim sLogMess(0) As String
		Dim sTaskDesc As String = ""
		
		result = True
		Dim iProcessIcon As Integer = True
		
		Try 
			ErrCommitHDB = True
			ErrFillStruct = False
			
			If iRetryErrors Then
                dsDMSHistData = g_dbHistDDB.CreateDynaset("SELECT * FROM historydata ORDER BY hdb_num")
				DAO_DBEngine_definst.FreeLocks()
			Else
				dsDMSHistData = g_dbHistDDB.CreateDynaset("SELECT * FROM historydata WHERE hderror = " & False & " ORDER BY hdb_num")
				DAO_DBEngine_definst.FreeLocks()
			End If
			
			dsDMSHistData.LockEdits = False
			
			If dsDMSHistData.RecordCount = 0 Then
				result = False
                dsDMSHistData.Close()
				dsDMSHistData = Nothing
				Return result
			End If
			
			utDMSHistParams.DMSDir.Value = g_sHistoryRoot
			
			' Process all history records
			While Not dsDMSHistData.EOF
				' Set Forms Ico - salvo(110796)
				'        If g_sAppType <> "WAPI" Then
				'            If (iProcessIcon) Then
				'                frmDMSMain.Icon = frmDMSMain!imgProcess1.Picture
				'                iProcessIcon = False
				'            Else
				'                frmDMSMain.Icon = frmDMSMain!imgProcess2.Picture
				'                iProcessIcon = True
				'            End If
				'        End If
				Application.DoEvents()
				
				ReDim sLogMess(5)
				
				' Append a log message to todays log file
				If Not (OpenLogFile(g_sHistoryRoot)) Then
					' Failed to log message
					ErrorLog("ProcessControlData", "Failed to open log file")
					Return False
                End If

                Select Case (dsDMSHistData("task").Value)
                    Case ADDCABINET
                        sTaskDesc = "ADDCABINET"
                    Case DELCABINET
                        sTaskDesc = "DELCABINET"
                    Case MODCABINET
                        sTaskDesc = "MODCABINET"
                    Case ADDDRAWER
                        sTaskDesc = "ADDDRAWER"
                    Case DELDRAWER
                        sTaskDesc = "DELDRAWER"
                    Case MODDRAWER
                        sTaskDesc = "MODDRAWER"
                    Case ADDFOLDER
                        sTaskDesc = "ADDFOLDER"
                    Case DELFOLDER
                        sTaskDesc = "DELFOLDER"
                    Case MODFOLDER
                        sTaskDesc = "MODFOLDER"
                    Case ADDDOCUMENT
                        sTaskDesc = "ADDDOCUMENT"
                    Case DELDOCUMENT
                        sTaskDesc = "DELDOCUMENT"
                    Case MODDOCUMENT
                        sTaskDesc = "MODDOCUMENT"
                End Select

                sLogMess(1) = "DocuMaster API Daemon - REMOTE HISTORY TASK, " & sTaskDesc
                iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                CloseLogFile(g_sHistoryRoot)

                ErrFillStruct = True
                ErrCommitHDB = False

                ' Fill structue from record details
                utDMSHistParams.NewRec.cabinetcode.Value = dsDMSHistData("cabinetcode")
                utDMSHistParams.NewRec.cabinetname.Value = dsDMSHistData("cabinetname")
                utDMSHistParams.NewRec.drawercode.Value = dsDMSHistData("drawercode")
                utDMSHistParams.NewRec.drawername.Value = dsDMSHistData("drawername")
                utDMSHistParams.NewRec.foldercode.Value = dsDMSHistData("foldercode")
                utDMSHistParams.NewRec.foldername.Value = dsDMSHistData("foldername")
                utDMSHistParams.NewRec.docref.Value = dsDMSHistData("docref")
                utDMSHistParams.NewRec.date_Renamed.Value = dsDMSHistData("date")
                utDMSHistParams.NewRec.time.Value = dsDMSHistData("time")
                utDMSHistParams.NewRec.eventtype.Value = dsDMSHistData("eventtype")
                utDMSHistParams.NewRec.description.Value = dsDMSHistData("description")
                utDMSHistParams.NewRec.volume.Value = dsDMSHistData("volume")
                utDMSHistParams.NewRec.pagefile.Value = dsDMSHistData("pagefile")
                utDMSHistParams.NewRec.doctype.Value = dsDMSHistData("doctype")
                utDMSHistParams.NewRec.filler.Value = New String(" "c, 13)

                Try

                    Select Case (dsDMSHistData("task").Value)
                        Case ADDCABINET
                            ' Add Cabinet to history database
                            NewCab(utDMSHistParams)
                            'TODO: Function/Sub NEWCAB parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> NEWCAB  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case DELCABINET
                            ' Delete Cabinet to history database
                            DelCab(utDMSHistParams)
                            'TODO: Function/Sub DELCAB parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> DELCAB  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case MODCABINET
                            ' Modify Cabinet to history database
                            ModCab(utDMSHistParams)
                            'TODO: Function/Sub MODCAB parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> MODCAB  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case ADDDRAWER
                            ' Add Drawer to history database
                            NewDrw(utDMSHistParams)
                            'TODO: Function/Sub NEWDRW parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> NEWDRW  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case DELDRAWER
                            ' Delete Drawer to history database
                            DelDrw(utDMSHistParams)
                            'TODO: Function/Sub DELDRW parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> DELDRW  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case MODDRAWER
                            ' Modify Drawer to history database
                            ModDrw(utDMSHistParams)
                            'TODO: Function/Sub MODDRW parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> MODDRW  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case ADDFOLDER
                            ' Add Folder to history database
                            NewFld(utDMSHistParams)
                            'TODO: Function/Sub NEWFLD parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> NEWFLD  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case DELFOLDER
                            ' Delete Folder to history database
                            DelFld(utDMSHistParams)
                            'TODO: Function/Sub DELFLD parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> DELFLD  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case MODFOLDER
                            ' Modify Folder to history database
                            ModFld(utDMSHistParams)
                            'TODO: Function/Sub MODFLD parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> MODFLD  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case ADDDOCUMENT
                            ' Add Document to history database
                            NewDoc(utDMSHistParams)
                            'TODO: Function/Sub NEWDOC parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> NEWDOC  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case DELDOCUMENT
                            ' Delete Document to history database
                            DelDoc(utDMSHistParams)
                            'TODO: Function/Sub DELDOC parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> DELDOC  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                        Case MODDOCUMENT
                            ' Modify Document to history database
                            ModDoc(utDMSHistParams)
                            'TODO: Function/Sub MODDOC parameters should be checked against the calling convention on the next line.
                            'TODO: -> Unknown WinAPI/Third-Party DLL Call -> MODDOC  Check Calling convention for compatibility with BSTR/UNICODE/ANSI Strings. See VB4DLL.TXT in VB directory.
                    End Select

                    Debug.WriteLine("History Return Code : " & utDMSHistParams.ReturnCode.Value)
                    If utDMSHistParams.FileStatus.Value.StartsWith("9") Then
                        Debug.WriteLine("History FileStatus Code : " & utDMSHistParams.FileStatus.Value.Substring(0, 1) & ", " & CStr(Strings.Asc(utDMSHistParams.FileStatus.Value.Substring(1, 1)(0))))
                    Else
                        Debug.WriteLine("History FileStatus Code : " & utDMSHistParams.FileStatus.Value)
                    End If

                    Select Case (dsDMSHistData("task").Value)
                        Case ADDCABINET, MODCABINET
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value & ", " & dsDMSHistData("cabinetname").Value
                        Case DELCABINET
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                        Case ADDDRAWER, MODDRAWER
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                            sLogMess(3) = "Drawer - " & dsDMSHistData("drawercode").Value & ", " & dsDMSHistData("drawername").Value
                        Case DELDRAWER
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                            sLogMess(3) = "Drawer - " & dsDMSHistData("drawercode").Value
                        Case ADDFOLDER, MODFOLDER
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                            sLogMess(3) = "Drawer - " & dsDMSHistData("drawercode").Value
                            sLogMess(4) = "Folder - " & dsDMSHistData("foldercode").Value & ", " & dsDMSHistData("foldername").Value
                        Case DELFOLDER
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                            sLogMess(3) = "Drawer - " & dsDMSHistData("drawercode").Value
                            sLogMess(4) = "Folder - " & dsDMSHistData("foldercode").Value
                        Case ADDDOCUMENT, DELDOCUMENT, MODDOCUMENT
                            sLogMess(2) = "Cabinet - " & dsDMSHistData("cabinetcode").Value
                            sLogMess(3) = "Drawer - " & dsDMSHistData("drawercode").Value
                            sLogMess(4) = "Folder - " & dsDMSHistData("foldercode").Value
                            sLogMess(5) = "Document - " & dsDMSHistData("docref").Value & ", " & dsDMSHistData("description").Value
                    End Select

                    If utDMSHistParams.ReturnCode.Value = "0000" Then
                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("CommitHDB", "Failed to open log file")
                        End If

                        Select Case (dsDMSHistData("task").Value)
                            Case ADDCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet created"
                            Case DELCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet deleted"
                            Case MODCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet modified"
                            Case ADDDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer created"
                            Case DELDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer deleted"
                            Case MODDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer modified"
                            Case ADDFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder created"
                            Case DELFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder deleted"
                            Case MODFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder modified"
                            Case ADDDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document created"
                            Case DELDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document deleted"
                            Case MODDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document modified"
                        End Select

                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                        CloseLogFile(g_sHistoryRoot)

                        ' Delete record now finished processing
                        g_iTmp = 0
                        g_iRC = DeleteDynaset(dsDMSHistData)
                        Select Case g_iRC
                            Case PM_TRUE
                            Case PM_FALSE
                                Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "CommitHDB")
                            Case PM_CANCEL
                                Interaction.MsgBox("Delete Cancelled. (ds)", MB_ICONEXCLAMATION, "CommitHDB")
                            Case Else
                                Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "CommitHDB")
                        End Select
                    Else
                        ' History DLL failed, log message
                        '
                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("CommitHDB", "Failed to open log file")
                        End If

                        Select Case (dsDMSHistData("task").Value)
                            Case ADDCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet create failed. "
                            Case DELCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet delete failed. "
                            Case MODCABINET
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet modify failed. "
                            Case ADDDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer create failed. "
                            Case DELDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer delete failed. "
                            Case MODDRAWER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer modify failed. "
                            Case ADDFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder create failed. "
                            Case DELFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder delete failed. "
                            Case MODFOLDER
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Folder modify failed. "
                            Case ADDDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document create failed. "
                            Case DELDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document delete failed. "
                            Case MODDOCUMENT
                                sLogMess(1) = "DocuMaster API Daemon - Remote History Document modify failed. "
                        End Select

                        If utDMSHistParams.FileStatus.Value.StartsWith("9") Then
                            sLogMess(1) = sLogMess(1) & "RC : " & utDMSHistParams.ReturnCode.Value & "  FS : " & utDMSHistParams.FileStatus.Value.Substring(0, 1) & ", " & CStr(Strings.Asc(utDMSHistParams.FileStatus.Value.Substring(1, 1)(0)))
                        Else
                            sLogMess(1) = sLogMess(1) & "RC : " & utDMSHistParams.ReturnCode.Value & "  FS : " & utDMSHistParams.FileStatus.Value
                        End If

                        iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)
                        CloseLogFile(g_sHistoryRoot)

                        ' Set hderror flag to true to say
                        ' that an error has occurred
                        dsDMSHistData.Edit()
                        dsDMSHistData("hderror").Value = True

                        g_iTmp = 0
                        g_iRC = UpdateDynaset(dsDMSHistData)
                        Select Case g_iRC
                            Case PM_TRUE
                            Case PM_FALSE
                                Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "CommitHDB")
                                result = False
                            Case PM_CANCEL
                                Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "CommitHDB")
                                result = False
                            Case PM_DUPLICATEKEY
                                Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "CommitHDB")
                                result = False
                            Case Else
                                Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "CommitHDB")
                                result = False
                        End Select
                    End If

                Catch
                End Try


                dsDMSHistData.MoveNext()
            End While

            Return result

        Catch excep As System.Exception
            If Not ErrFillStruct And Not ErrCommitHDB Then
                Throw excep
            End If

            If ErrFillStruct Then


                result = False
                dsDMSHistData.Close()
                Return result

            End If
            If ErrCommitHDB Or ErrFillStruct Then


                Return False

            End If
        End Try
    End Function

    Function CompactDDB() As Integer
        Dim ErrCompacting As Boolean = False
        Dim ErrCompactDDB As Boolean = False

        Dim result As Integer = 0
        Dim sDBName As String = ""
        Dim iDataLen As Integer

        result = True

        Try
            ErrCompactDDB = True
            ErrCompacting = False

            If Not (CheckLogins()) Then
                ' Users are still logged on
                Return False
            End If

            ' Get DDB root and name
            sDBName = New String(" "c, 128)
            Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("dbName")
            Try
                iDataLen = GetPrivateProfileString("Paths", tmpPtr, "", sDBName, 128, "pmDMS.ini")
            Finally
                Marshal.FreeHGlobal(tmpPtr)
            End Try
            sDBName = sDBName.Substring(0, iDataLen)

            ' OK to continue with the compacting
            '
            ' Close the DDB
            CloseDDB()

            ErrCompacting = True
            ErrCompactDDB = False

            ' Compact the DDB


            ' Remove the Backup DDB File
            If KillFile(g_sDBRoot & "\DMSDDB.BAK") = PM_FALSE Then
            End If

            ' Rename the DDB
            If Rename(g_sDBRoot & "\" & sDBName, g_sDBRoot & "\DMSDDB.BAK") = PM_FALSE Then
            End If

            If Rename(g_sDBRoot & "\" & "NWDMSDDB.MDB", g_sDBRoot & "\" & sDBName) = PM_FALSE Then
            End If

            If KillFile(g_sDBRoot & "\" & "NWDMSDDB.LDB") = PM_FALSE Then
            End If

            ErrCompactDDB = True
            ErrCompacting = False

            ' Everything seems to be OK
            '
            ' Open Database
            If Not (OpenDDB()) Then
                result = False
            End If

            Return result

        Catch excep As System.Exception
            If Not ErrCompacting And Not ErrCompactDDB Then
                Throw excep
            End If

            If ErrCompacting Then


                Debug.WriteLine("Failed to compact DDB!")
                result = False



            End If
            If ErrCompactDDB Or ErrCompacting Then


                result = False
                MessageBox.Show(CStr(Information.Err().Number) & ": " & Conversion.ErrorToString(), Application.ProductName)
                Return result

            End If
        End Try
    End Function

    Function CompactOK() As Integer

        Try


            If DateTime.Now.ToString("HHMM") >= "0000" And DateTime.Now.ToString("HHMM") <= "0100" And Not g_iCompact Then
                g_iCompact = True
                Return True
            ElseIf (DateTime.Now.ToString("HHMM") > "0100" And DateTime.Now.ToString("HHMM") < "0200") Then
                g_iCompact = False
                Return False
            Else
                Return False
            End If

        Catch
        End Try



        Return False

    End Function

    Sub ConfigDrives()
        Dim iDev As Integer
        Dim sVol As String = ""

        frmDMSMain.drvMDI.Refresh()
        For iCnt As Integer = 0 To frmDMSMain.drvMDI.Items.Count - 1

            sVol = GetVolumeName(frmDMSMain.drvMDI.Items(iCnt).Substring(0, Math.Min(frmDMSMain.drvMDI.Items(iCnt).Length, 2)))
            iDev = GetVolumeDevice(sVol)

            '       Debug.Print Mid$(drive1.List(iCnt%), 1, 2); sVol$; iDev%

            If sVol <> "" And iDev > 0 Then
                ' We have an initialised volume in this drive                
                Dim tmpPtr As IntPtr
                Dim tmpPtr2 As IntPtr
                Try
                    Dim auxVar_2 As String = frmDMSMain.drvMDI.Items(iCnt).Substring(0, Math.Min(frmDMSMain.drvMDI.Items(iCnt).Length, 2))
                    Dim auxVar As String = iDev.ToString()
                    tmpPtr = Marshal.StringToHGlobalAnsi(auxVar)
                    tmpPtr2 = Marshal.StringToHGlobalAnsi(auxVar_2)
                    If Not WritePrivateProfileString("DeviceMappings", tmpPtr, tmpPtr2, "pmdms.ini") Then
                        MessageBox.Show("ERROR: Failed to write to .ini file", Application.ProductName)
                    End If
                Finally
                    Marshal.FreeHGlobal(tmpPtr)
                    Marshal.FreeHGlobal(tmpPtr2)
                End Try

                If Not MountVolume(frmDMSMain.drvMDI.Items(iCnt).Substring(0, Math.Min(frmDMSMain.drvMDI.Items(iCnt).Length, 2))) Then
                    MessageBox.Show("Failed to mount volume in drive " & frmDMSMain.drvMDI.Items(iCnt).Substring(0, Math.Min(frmDMSMain.drvMDI.Items(iCnt).Length, 2)), Application.ProductName)
                End If
            End If

        Next iCnt

    End Sub

    Function ConvertSlashes(ByRef sString As String) As String


        Dim sTmpStr As New StringBuilder

        For iCntr As Integer = 1 To sString.Length
            If sString.Substring(iCntr - 1, 1) = "/" Then
                sTmpStr.Append("\")
            Else
                sTmpStr.Append(sString.Substring(iCntr - 1, 1))
            End If
        Next iCntr

        Return sTmpStr.ToString()

    End Function

    Function CopyFile2(ByRef sFrom As String, ByRef sTo As String) As Integer

        Try

            File.Copy(sFrom, sTo)

            Return PM_TRUE

        Catch



            Return PM_FALSE
        End Try

    End Function

    Function CreateAnnotations(ByRef lDocumentNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utAnnotation As DMSDDB.g_utAnnotations = DMSDDB.g_utAnnotations.CreateInstance()

        utAnnotation.doc_num = lDocumentNumber
        utAnnotation.ann_text.Value = utControlData.annotation
        utAnnotation.user_name.Value = utControlData.username
        utAnnotation.create_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate

        Return SaveAnnotationsData(utAnnotation)

    End Function

    Function CreateCabinet(ByRef utControlData As g_utControlData) As Integer

        Dim utCabinet As DMSDDB.g_utCabinet = DMSDDB.g_utCabinet.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        utCabinet.cabinet_name.Value = utControlData.cabinetname
        utCabinet.list_src.Value = ""
        utCabinet.access_level = 9 'utControlData.access
        utCabinet.password.Value = ""
        utCabinet.ex_code.Value = utControlData.cabinetname
        utCabinet.link = 0

        Dim lCabinetNumber As Integer = SaveCabinetData(utCabinet)

        If lCabinetNumber <> 0 Then 'And g_sAppType <> "WAPI") Then
            ' Update the history database
            utDMSHistData.cabinetcode.Value = utControlData.cabinetname
            utDMSHistData.cabinetname.Value = utControlData.cabinetname

            If Not (UpdateHDB(ADDCABINET, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lCabinetNumber = 0
            End If
        End If

        Return lCabinetNumber

    End Function

    Function CreateDocInfo(ByRef lDocumentNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utDocInfo As DMSDDB.g_utDocInfo = DMSDDB.g_utDocInfo.CreateInstance()

        ' Set default dates
        Dim iDDocDate As Integer = GetPrivateProfileInt("Settings", "DefaultDocDate", 0, g_sGroupIniName)
        Dim iDExDate As Integer = GetPrivateProfileInt("Settings", "DefaultExpiry", 0, g_sGroupIniName)

        utDocInfo.doc_num = lDocumentNumber
        utDocInfo.expiry_date = DateTime.Now.AddDays(iDExDate).ToOADate
        utDocInfo.scan_operator.Value = utControlData.username
        utDocInfo.doc_date = DateTime.Now.ToOADate - iDDocDate
        utDocInfo.last_user.Value = utControlData.username
        utDocInfo.last_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate

        Return SaveDocInfoData(utDocInfo)

    End Function

    Function CreateDocument(ByRef lFolderNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utDocument As DMSDDB.g_utDocument = DMSDDB.g_utDocument.CreateInstance()

        utDocument.doc_name.Value = utControlData.documentname
        utDocument.folder_num = lFolderNumber
        utDocument.access_level = 9 'utControlData.access
        utDocument.password.Value = ""
        utDocument.docset_num = 0

        'If g_sAppType = "WAPI" Then
        '    utDocument.ex_code = ""
        'Else
        utDocument.ex_code.Value = utControlData.documentname
        'End If

        utDocument.link = 0
        utDocument.doc_type.Value = utControlData.event_Renamed

        Return SaveDocumentData(utDocument)

    End Function

    Function CreateDrawer(ByRef lCabinetNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utDrawer As DMSDDB.g_utDrawer = DMSDDB.g_utDrawer.CreateInstance()
        Dim utFolder As DMSDDB.g_utFolder = DMSDDB.g_utFolder.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim lGeneralNumber As Integer

        utDrawer.drawer_name.Value = utControlData.drawername
        utDrawer.ex_code.Value = utControlData.drawername
        utDrawer.cabinet_num = lCabinetNumber
        utDrawer.access_level = 9 'utControlData.access
        utDrawer.password.Value = ""
        utDrawer.link = 0

        Dim lDrawerNumber As Integer = SaveDrawerData(utDrawer)

        If lDrawerNumber <> 0 Then
            ' Create default GENERAL folder
            utFolder.folder_name.Value = "GENERAL"
            utFolder.ex_code.Value = "GENERAL"
            utFolder.drawer_num = lDrawerNumber
            utFolder.access_level = 9
            utFolder.password.Value = ""
            utFolder.link = 0

            lGeneralNumber = SaveFolderData(utFolder)
        End If

        If lDrawerNumber <> 0 And lGeneralNumber <> 0 Then ' And g_sAppType <> "WAPI") Then
            ' Update the history database
            utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
            utDMSHistData.drawercode.Value = utControlData.drawername
            utDMSHistData.drawername.Value = utControlData.drawername

            If Not (UpdateHDB(ADDDRAWER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lDrawerNumber = 0
            End If
        End If

        Return lDrawerNumber

    End Function

    Function CreateEventAnnotations(ByRef lDocumentNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim result As Integer = 0
        Dim utAnnotation As DMSDDB.g_utAnnotations = DMSDDB.g_utAnnotations.CreateInstance()

        result = True

        utAnnotation.doc_num = lDocumentNumber
        utAnnotation.user_name.Value = utControlData.username
        utAnnotation.create_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate

        ' Add an Annotation for the event type
        Select Case (utControlData.event_Renamed)
            Case "E"
                utAnnotation.ann_text.Value = "Email"
            Case "L"
                utAnnotation.ann_text.Value = "Letter"
            Case "N"
                utAnnotation.ann_text.Value = "Note"
            Case "I"
                utAnnotation.ann_text.Value = "Incoming"
            Case "F"
                utAnnotation.ann_text.Value = "Fax"
            Case Else
                utAnnotation.ann_text.Value = ""
        End Select

        If utAnnotation.ann_text.Value <> "" Then
            result = SaveAnnotationsData(utAnnotation)
        End If

        Return result
    End Function

    Function CreateFolder(ByRef lCabinetNumber As Integer, ByRef lDrawerNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utFolder As DMSDDB.g_utFolder = DMSDDB.g_utFolder.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        utFolder.folder_name.Value = utControlData.foldername
        utFolder.ex_code.Value = utControlData.foldername
        utFolder.drawer_num = lDrawerNumber
        utFolder.access_level = 9 'utControlData.access
        utFolder.password.Value = ""
        utFolder.link = 0

        Dim lFolderNumber As Integer = SaveFolderData(utFolder)

        If lFolderNumber <> 0 Then ' And g_sAppType <> "WAPI")
            ' Update the history database
            utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
            utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, lDrawerNumber)
            utDMSHistData.foldercode.Value = utControlData.foldername
            utDMSHistData.foldername.Value = utControlData.foldername

            If Not (UpdateHDB(ADDFOLDER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lFolderNumber = 0
            End If
        End If

        Return lFolderNumber

    End Function

    Function CreateIndexCabinet(ByRef utIndexListData As g_utIndexListData) As Integer

        Dim utCabinet As DMSDDB.g_utCabinet = DMSDDB.g_utCabinet.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        utCabinet.cabinet_name.Value = utIndexListData.cabinetname
        utCabinet.list_src.Value = ""
        utCabinet.access_level = g_iIndexAccessLevel
        utCabinet.password.Value = ""
        utCabinet.ex_code.Value = utIndexListData.cabinetcode
        utCabinet.link = 0

        Dim lCabinetNumber As Integer = SaveCabinetData(utCabinet)

        If lCabinetNumber <> 0 Then 'And g_sAppType <> "WAPI") Then
            ' Update the history database
            utDMSHistData.cabinetcode.Value = utIndexListData.cabinetcode
            utDMSHistData.cabinetname.Value = utIndexListData.cabinetname

            If Not (UpdateHDB(ADDCABINET, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lCabinetNumber = 0
            End If
        End If

        Return lCabinetNumber

    End Function

    Function CreateIndexDrawer(ByRef lCabinetNumber As Integer, ByRef utIndexListData As g_utIndexListData) As Integer

        Dim utDrawer As DMSDDB.g_utDrawer = DMSDDB.g_utDrawer.CreateInstance()
        Dim utFolder As DMSDDB.g_utFolder = DMSDDB.g_utFolder.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim lGeneralNumber As Integer

        utDrawer.drawer_name.Value = utIndexListData.drawername
        utDrawer.ex_code.Value = utIndexListData.drawercode
        utDrawer.cabinet_num = lCabinetNumber
        utDrawer.access_level = g_iIndexAccessLevel
        utDrawer.password.Value = ""
        utDrawer.link = 0

        Dim lDrawerNumber As Integer = SaveDrawerData(utDrawer)

        If lDrawerNumber <> 0 Then
            ' Create default GENERAL folder
            utFolder.folder_name.Value = "GENERAL"
            utFolder.ex_code.Value = "GENERAL"
            utFolder.drawer_num = lDrawerNumber
            utFolder.access_level = 9
            utFolder.password.Value = ""
            utFolder.link = 0

            lGeneralNumber = SaveFolderData(utFolder)
        End If

        If lDrawerNumber <> 0 And lGeneralNumber <> 0 Then ' And g_sAppType <> "WAPI") Then
            ' Update the history database
            '        utDMSHistData.cabinetcode = GetDDBCode(CABINET, lCabinetNumber)
            utDMSHistData.cabinetcode.Value = utIndexListData.cabinetcode
            utDMSHistData.drawercode.Value = utIndexListData.drawercode
            utDMSHistData.drawername.Value = utIndexListData.drawername

            If Not (UpdateHDB(ADDDRAWER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lDrawerNumber = 0
            End If

            ' Update the history database for the GENERAL folder
            '        utDMSHistData.cabinetcode = GetDDBCode(CABINET, lCabinetNumber)
            '        utDMSHistData.drawercode = GetDDBCode(DRAWER, lDrawerNumber)
            utDMSHistData.cabinetcode.Value = utIndexListData.cabinetcode
            utDMSHistData.drawercode.Value = utIndexListData.drawercode
            utDMSHistData.foldercode.Value = "GENERAL"
            utDMSHistData.foldername.Value = "GENERAL"

            If Not (UpdateHDB(ADDFOLDER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lDrawerNumber = 0
            End If
        End If

        Return lDrawerNumber

    End Function

    Function CreateIndexFolder(ByRef lCabinetNumber As Integer, ByRef lDrawerNumber As Integer, ByRef utIndexListData As g_utIndexListData) As Integer

        Dim utFolder As DMSDDB.g_utFolder = DMSDDB.g_utFolder.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        utFolder.folder_name.Value = utIndexListData.foldername
        utFolder.ex_code.Value = utIndexListData.foldercode
        utFolder.drawer_num = lDrawerNumber
        utFolder.access_level = g_iIndexAccessLevel
        utFolder.password.Value = ""
        utFolder.link = 0

        Dim lFolderNumber As Integer = SaveFolderData(utFolder)

        If lFolderNumber <> 0 Then 'And g_sAppType <> "WAPI") Then
            ' Update the history database
            '        utDMSHistData.cabinetcode = GetDDBCode(CABINET, lCabinetNumber)
            '        utDMSHistData.drawercode = GetDDBCode(DRAWER, lDrawerNumber)
            utDMSHistData.cabinetcode.Value = utIndexListData.cabinetcode
            utDMSHistData.drawercode.Value = utIndexListData.drawercode
            utDMSHistData.foldercode.Value = utIndexListData.foldercode
            utDMSHistData.foldername.Value = utIndexListData.foldername

            If Not (UpdateHDB(ADDFOLDER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Cabinet history
                lFolderNumber = 0
            End If
        End If

        Return lFolderNumber

    End Function

    Function CreateKeyword(ByRef lDocumentNumber As Integer, ByRef lKeywordsNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utKeyword As DMSDDB.g_utKeyWord = DMSDDB.g_utKeyWord.CreateInstance()

        utKeyword.doc_num = lDocumentNumber
        utKeyword.key_num = lKeywordsNumber
        utKeyword.user_name.Value = utControlData.username
        utKeyword.create_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate

        Return SaveKeyWordData(utKeyword)

    End Function

    Function CreateKeywords(ByRef sKeyword As String) As Integer

        Dim utKeywords As DMSDDB.g_utKeyWords = DMSDDB.g_utKeyWords.CreateInstance()

        utKeywords.keyword.Value = sKeyword

        Return SaveKeyWordsData(utKeywords)

    End Function

    Function CreateLinkDocument(ByRef lFolderNumber As Integer, ByRef lDocumentLinkNumber As Integer, ByRef utControlData As g_utControlData) As Integer

        Dim utDocument As DMSDDB.g_utDocument = DMSDDB.g_utDocument.CreateInstance()

        utDocument.doc_name.Value = utControlData.documentname
        utDocument.folder_num = lFolderNumber
        utDocument.access_level = 9 'utControlData.access
        utDocument.password.Value = ""
        utDocument.docset_num = 0
        utDocument.ex_code.Value = utControlData.documentname
        utDocument.link = lDocumentLinkNumber
        utDocument.doc_type.Value = utControlData.event_Renamed

        Return SaveDocumentData(utDocument)

    End Function

    Function CreatePage(ByRef lDocumentNumber As Integer, ByRef utControlData As g_utControlData) As String

        Dim utPage As DMSDDB.g_utPage = DMSDDB.g_utPage.CreateInstance()

        '    utPage.page_name = GetNextPageName()
        utPage.doc_num = lDocumentNumber
        utPage.volume_name.Value = GetActiveVolumeName()
        utPage.backup_name.Value = ""
        utPage.page_type.Value = utControlData.doctype
        utPage.page_num = GetNextPageNumber(lDocumentNumber)
        utPage.scan_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate

        Dim sPageName As String = SavePageData(utPage)
        If sPageName.Trim() <> "" Then
            Return sPageName
        Else
            Return ""
        End If

    End Function

    Function DeleteCabinet(ByRef lCabinetNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim sCabinetCode As String = ""

        result = True

        Try

            '    sCabinetCode = GetDDBCode(CABINET, lCabinetNumber)


            ' Update the history database
            '    utDMSHistData.cabinetcode = sCabinetCode

            '    If Not (UpdateHDB(DELCABINET, g_sHistoryRoot, utDMSHistData)) Then
            ' Failed to create Cabinet history
            '        DeleteCabinet = False
            '    End If

            Return DeleteDDB(CABINET, lCabinetNumber)

        Catch



            Return False
        End Try

    End Function

    Function DeleteControlFiles(ByRef sControlName As String) As Integer

        Dim result As Integer = 0
        Dim utControlData As g_utControlData = g_utControlData.CreateInstance()
        Dim sTmpControlName As String = ""
        Dim iCntr As Integer

        result = True

        Try

            sTmpControlName = g_sHistoryRoot & "tmp\" & sControlName & ".ctl"

            If Not (GetControlData(sTmpControlName, utControlData)) Then
                ErrorLog("DeleteControlFiles", "Failed to get control data")
                Return False
            End If

            Select Case (utControlData.task)
                Case "ADD", "LOG"
                    If KillFile(g_sHistoryRoot & "tmp\" & sControlName & ".ctl") = PM_FALSE Then
                        ErrorLog("DeleteControlFile" & g_sHistoryRoot & "tmp\" & sControlName & ".ctl", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                        result = False
                    End If

                Case "ADDINDEX", "DELINDEX"
                    If KillFile(g_sHistoryRoot & "tmp\" & sControlName & ".ctl") = PM_FALSE Then
                        ErrorLog("DeleteControlFile" & g_sHistoryRoot & "tmp\" & sControlName & ".ctl", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                        result = False
                    End If
                    If KillFile(utControlData.filename) = PM_FALSE Then
                        ErrorLog("DeleteControlFile" & (utControlData.filename), "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                        result = False
                    End If
            End Select

            Return result

        Catch



            ErrorLog("DeleteControlFiles", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Function DeleteCurrentPos() As Integer

        Try

            File.Delete(g_sHistoryRoot & "data\journal.cur")

            Return True

        Catch



            ErrorLog("DeleteCurrentPos", g_sHistoryRoot & "data\journal.cur" & "Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Function DeleteDrawer(ByRef sCabinetCode As String, ByRef lDrawerNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim sDrawerCode As String = ""

        result = True

        Try

            '    sDrawerCode = GetDDBCode(DRAWER, lDrawerNumber)


            ' Update the history database
            '    utDMSHistData.cabinetcode = sCabinetCode
            '    utDMSHistData.drawercode = sDrawerCode

            '    If Not (UpdateHDB(DELDRAWER, g_sHistoryRoot, utDMSHistData)) Then
            ' Failed to create Drawer history
            '        DeleteDrawer = False
            '    End If

            Return DeleteDDB(DRAWER, lDrawerNumber)

        Catch



            Return False
        End Try

    End Function

    Sub DeleteErrorLog(ByRef ControlName As String)

        Try

            File.Delete(g_sHistoryRoot & "tmp\" & ControlName & ".log")

        Catch



            ErrorLog("DeleteErrorLog", g_sHistoryRoot & "tmp\" & ControlName & ".log" & "Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
            Exit Sub
        End Try


    End Sub

    Function DeleteFolder(ByRef sCabinetCode As String, ByRef sDrawerCode As String, ByRef lFolderNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim sFolderCode As String = ""

        result = True

        Try

            '    sFolderCode = GetDDBCode(FOLDER, lFolderNumber)


            ' Update the history database
            '    utDMSHistData.cabinetcode = sCabinetCode
            '    utDMSHistData.drawercode = sDrawerCode
            '    utDMSHistData.foldercode = sFolderCode

            '    If Not (UpdateHDB(DELFOLDER, g_sHistoryRoot, utDMSHistData)) Then
            ' Failed to create Folder history
            '        DeleteFolder = False
            '    End If

            Return DeleteDDB(FOLDER, lFolderNumber)

        Catch



            Return False
        End Try

    End Function

    Function DeleteJournalFile(ByRef iRetryErrors As Integer) As Integer

        Dim result As Integer = 0
        result = True

        Try

            If iRetryErrors Then
                If KillFile(g_sHistoryRoot & "data\journal.rty") = PM_FALSE Then
                    ErrorLog("DeleteJournalFile - ", g_sHistoryRoot & "data\journal.rty. Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
                    result = False
                End If
            Else
                If KillFile(g_sHistoryRoot & "data\journal.wrk") = PM_FALSE Then
                    ErrorLog("DeleteJournalFile - ", g_sHistoryRoot & "data\journal.wrk. Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
                    result = False
                End If

            End If

            Return result

        Catch



            ErrorLog("DeleteJournalFile", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Function DrawerChanged(ByRef lCabinetNumber As Integer, ByRef lDrawerNumber As Integer, ByRef sDrawerName As String) As Integer

        Dim result As Integer = 0
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        result = False

        Try

            If DDBChanged(DRAWER, lDrawerNumber, sDrawerName) Then
                result = True
            End If

            ' Update the history database
            ' Salvo - update regardless of whether DDB changed as it's possible
            '         the Remoted History DB needs updating
            utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
            utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, lDrawerNumber)
            utDMSHistData.drawername.Value = sDrawerName

            If Not (UpdateHDB(MODDRAWER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Drawer history
                ErrorLog("ProcessControlData", "Failed to change Drawer name, " & sDrawerName)
                result = False
            End If

            Return result

        Catch



            Return False
        End Try

    End Function

    Sub ErrorLog(ByRef FunctionName As String, ByRef ErrorMessage As String)

        Try

            If g_sAppType.Value = "WAPI" Then
                Exit Sub
            End If

            FileSystem.PrintLine(fhErrorlog, DateTime.Now.ToString("dd/MM/yy  HH:MM:ss") & " - " & "Func: " & FunctionName & ", " & ErrorMessage)

        Catch
        End Try



        Exit Sub

    End Sub

    Function EventAnnotationExists(ByRef lDocumentNumber As Integer, ByRef sTask As String) As Integer

        Dim result As Integer = 0
        Dim ssTmpExists As DAO.Snapshot
        Dim sAnnotationName As String = ""

        Select Case (sTask)
            Case "E"
                sAnnotationName = "Email"
            Case "L"
                sAnnotationName = "Letter"
            Case "N"
                sAnnotationName = "Note"
            Case "I"
                sAnnotationName = "Incoming"
            Case "F"
                sAnnotationName = "Fax"
            Case Else
                sAnnotationName = ""
        End Select

        Dim sSQLQuery As String = "SELECT doc_num FROM annotation WHERE "
        sSQLQuery = sSQLQuery & "doc_num = " & CStr(lDocumentNumber) & " AND "
        sSQLQuery = sSQLQuery & "ann_text = '" & sAnnotationName & "'"

        Try

            ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
            DAO_DBEngine_definst.FreeLocks()

            result = (ssTmpExists.RecordCount > 0)

            ssTmpExists.Close()
            Return result

        Catch



            ssTmpExists.Close()
            Return False
        End Try

    End Function

    Function FillHDBRecord(ByRef lDocumentNumber As Integer, ByRef utUpdateData As DMSHIST.g_utDMSHistData) As Integer

        Dim result As Integer = 0
        Dim ssUpdateData As DAO.Snapshot
        Dim sSQLQuery As String = ""

        result = True

        Try

            sSQLQuery = "SELECT cabinet.cabinet_name, cabinet.ex_code, "
            sSQLQuery = sSQLQuery & "drawer.drawer_name, drawer.ex_code, "
            sSQLQuery = sSQLQuery & "folder.folder_name, folder.ex_code, "
            sSQLQuery = sSQLQuery & "document.doc_name, document.doc_type, "
            sSQLQuery = sSQLQuery & "page.page_name, page.page_type, "
            sSQLQuery = sSQLQuery & "page.volume_name "
            sSQLQuery = sSQLQuery & "FROM document, page, folder, drawer, cabinet "
            sSQLQuery = sSQLQuery & "WHERE document.doc_num = page.doc_num AND "
            sSQLQuery = sSQLQuery & "document.folder_num = folder.folder_num AND "
            sSQLQuery = sSQLQuery & "folder.drawer_num = drawer.drawer_num AND "
            sSQLQuery = sSQLQuery & "drawer.cabinet_num = cabinet.cabinet_num AND "
            sSQLQuery = sSQLQuery & "document.doc_num = " & CStr(lDocumentNumber)

            ssUpdateData = g_dbDDB.CreateSnapshot(sSQLQuery)
            DAO_DBEngine_definst.FreeLocks()

            If ssUpdateData.RecordCount = 0 Then
                Return False
            End If

            ' Now we have the record, we can fill up
            ' the ye old structure with the data

            utUpdateData.cabinetcode.Value = ssUpdateData("cabinet.ex_code")
            utUpdateData.cabinetname.Value = ssUpdateData("cabinet.cabinet_name")
            utUpdateData.drawercode.Value = ssUpdateData("drawer.ex_code")
            utUpdateData.drawername.Value = ssUpdateData("drawer.drawer_name")
            utUpdateData.foldercode.Value = ssUpdateData("folder.ex_code")
            utUpdateData.foldername.Value = ssUpdateData("folder.folder_name")
            '    utUpdateData.docref = lDocumentNumber
            utUpdateData.date_Renamed.Value = DateTime.Now.ToString("yyyyMMdd")
            utUpdateData.time.Value = DateTime.Now.ToString("HHMMss")
            utUpdateData.eventtype.Value = ssUpdateData("document.doc_type")
            utUpdateData.description.Value = ssUpdateData("document.doc_name")
            utUpdateData.volume.Value = ssUpdateData("page.volume_name")
            '    utUpdateData.pagefile = StripSlashes((ssUpdateData("page.page_name")))
            utUpdateData.doctype.Value = ssUpdateData("page.page_type")
            utUpdateData.filler.Value = New String(" "c, 18)

            Return result

        Catch



            Return False
        End Try

    End Function

    Function FillIndexCabinet(ByRef utIndexCabinet() As g_utIndexCabinet) As Integer

        Dim result As Integer = 0
        Dim ssSnapShot As DAO.Snapshot

        result = PM_TRUE

        Try

            ssSnapShot = g_dbDDB.CreateSnapshot("SELECT ex_code, cabinet_num FROM cabinet WHERE ex_code <> ''")
            DAO_DBEngine_definst.FreeLocks()

            If ssSnapShot.RecordCount = 0 Then
                Return PM_TRUE
            End If

            ReDim utIndexCabinet(0)
            While Not ssSnapShot.EOF
                ReDim Preserve utIndexCabinet(utIndexCabinet.GetUpperBound(0) + 1)
                utIndexCabinet(utIndexCabinet.GetUpperBound(0)).cabinetcode = ssSnapShot("ex_code").Value
                utIndexCabinet(utIndexCabinet.GetUpperBound(0)).CabinetNumber = ssSnapShot("cabinet_num").Value

                ssSnapShot.MoveNext()
            End While

            ssSnapShot.Close()
            ssSnapShot = Nothing

            Return result

        Catch



            Return PM_FALSE
        End Try

    End Function

    Function FolderChanged(ByRef lCabinetNumber As Integer, ByRef lDrawerNumber As Integer, ByRef lFolderNumber As Integer, ByRef sFolderName As String) As Integer

        Dim result As Integer = 0
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()

        result = False

        Try

            If DDBChanged(FOLDER, lFolderNumber, sFolderName) Then
                result = True
            End If

            ' Update the history database
            ' Salvo - update regardless of whether DDB changed as it's possible
            '         the Remoted History DB needs updating
            utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
            utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, lDrawerNumber)
            utDMSHistData.foldercode.Value = GetDDBCode(FOLDER, lFolderNumber)
            utDMSHistData.foldername.Value = sFolderName

            If Not (UpdateHDB(MODFOLDER, g_sHistoryRoot, utDMSHistData)) Then
                ' Failed to create Folder history
                ErrorLog("ProcessControlData", "Failed to change Folder name, " & sFolderName)
                result = False
            End If

            Return result

        Catch



            Return False
        End Try

    End Function

    Function GetControlData(ByRef sControlName As String, ByRef utControlData As g_utControlData) As Integer

        Dim result As Integer = 0
        Dim iDataLen As Integer
        Dim sTmpString As String = ""

        result = True

        Try

            ' TASK
            utControlData.task = New String(" "c, 128)
            Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("TASK")
            Try
                iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr, "", utControlData.task, 128, sControlName)
            Finally
                Marshal.FreeHGlobal(tmpPtr)
            End Try
            utControlData.task = utControlData.task.Substring(0, iDataLen).Trim()

            If iDataLen = 0 Then
                If g_sAppType.Value = "WAPI" Then
                    ErrorWAPI(ERR_NOTASK, sControlName)
                Else
                    ErrorLog("GetControlData", "Failed to get TASK from control file, " & sControlName)
                    result = False
                End If
            End If

            Dim sTmp As String = ""
            Select Case utControlData.task.ToUpper()
                Case "ADD", "POPUP"
                    'Is the call using internal or external references (external as default)
                    sTmp = New String(" "c, 128)
                    Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi("EXTERN")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr2, "", sTmp, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr2)
                    End Try
                    sTmp = sTmp.Substring(0, iDataLen).Trim()

                    If iDataLen = 0 Then
                        utControlData.external = True
                    Else
                        utControlData.external = Not (sTmp.ToUpper().IndexOf("FALSE") + 1)
                    End If

                    ' CABINET
                    utControlData.cabinetname = New String(" "c, 128)
                    Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi("CABINET")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr3, "", utControlData.cabinetname, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr3)
                    End Try
                    utControlData.cabinetname = utControlData.cabinetname.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_NOCABINET, sControlName)
                        Else
                            ErrorLog("GetControlData", "Failed to get CABINET from control file, " & sControlName)
                        End If
                        result = False
                    End If

                    ' DRAWER
                    utControlData.drawername = New String(" "c, 128)
                    Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi("DRAWER")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr4, "", utControlData.drawername, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr4)
                    End Try
                    utControlData.drawername = utControlData.drawername.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_NODRAWER, sControlName)
                        Else
                            ErrorLog("GetControlData", "Failed to get DRAWER from control file, " & sControlName)
                        End If
                        result = False
                    End If

                    ' FOLDER
                    utControlData.foldername = New String(" "c, 128)
                    Dim tmpPtr5 As IntPtr = Marshal.StringToHGlobalAnsi("FOLDER")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr5, "", utControlData.foldername, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr5)
                    End Try
                    utControlData.foldername = utControlData.foldername.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_NOFOLDER, sControlName)
                        Else
                            ErrorLog("GetControlData", "Failed to get FOLDER from control file, " & sControlName)
                        End If
                        result = False
                    End If

                    ' LINKFOLDER
                    utControlData.linkfolder = New String(" "c, 128)
                    Dim tmpPtr6 As IntPtr = Marshal.StringToHGlobalAnsi("LINKFOLDER")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr6, "", utControlData.linkfolder, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr6)
                    End Try
                    utControlData.linkfolder = utControlData.linkfolder.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        utControlData.linkfolder = ""
                    End If

                    ' DOCUMENT
                    utControlData.documentname = New String(" "c, 128)
                    Dim tmpPtr7 As IntPtr = Marshal.StringToHGlobalAnsi("DOCUMENT")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr7, "", utControlData.documentname, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr7)
                    End Try
                    utControlData.documentname = utControlData.documentname.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_NODOCUMENT, sControlName)
                        Else
                            ErrorLog("GetControlData", "Failed to get DOCUMENT from control file, " & sControlName)
                        End If
                        result = False
                    End If

                    ' KEYWORDS
                    utControlData.keywords = New String(" "c, 128)
                    Dim tmpPtr8 As IntPtr = Marshal.StringToHGlobalAnsi("KEYWORDS")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr8, "", utControlData.keywords, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr8)
                    End Try
                    utControlData.keywords = utControlData.keywords.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        utControlData.keywords = ""
                    End If

                    ' DOCTYPE
                    utControlData.doctype = New String(" "c, 128)
                    Dim tmpPtr9 As IntPtr = Marshal.StringToHGlobalAnsi("DOCTYPE")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr9, "", utControlData.doctype, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr9)
                    End Try
                    utControlData.doctype = utControlData.doctype.Substring(0, iDataLen).Trim().ToUpper()
                    If iDataLen = 0 Then
                        utControlData.doctype = "TXT"
                    End If

                    ' EVENT
                    utControlData.event_Renamed = New String(" "c, 128)
                    Dim tmpPtr10 As IntPtr = Marshal.StringToHGlobalAnsi("EVENT")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr10, "", utControlData.event_Renamed, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr10)
                    End Try
                    utControlData.event_Renamed = utControlData.event_Renamed.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then

                        Select Case utControlData.doctype.ToUpper()
                            Case "TXT"
                                utControlData.event_Renamed = "L" 'letter
                            Case "RTF"
                                utControlData.event_Renamed = "R" 'Rich Text Document
                            Case "TIF"
                                utControlData.event_Renamed = "I" 'Image
                            Case Else
                                If g_sAppType.Value = "WAPI" Then
                                    'ErrorWAPI ERR_NOEVENT, sControlName$
                                Else
                                    ErrorLog("GetControlData", "Failed to get EVENT from control file, " & sControlName)
                                    result = False
                                End If
                        End Select

                    End If

                    ' FILENAME
                    utControlData.filename = New String(" "c, 128)
                    Dim tmpPtr11 As IntPtr = Marshal.StringToHGlobalAnsi("FILENAME")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr11, "", utControlData.filename, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr11)
                    End Try

                    If g_sAppType.Value = "WAPI" Then
                        ' the control file for WAPI will state the entire Path name ...
                        utControlData.filename = utControlData.filename.Substring(0, iDataLen).Trim()
                    Else
                        ' ... the API needs to append the Journal root to the file name
                        utControlData.filename = g_sHistoryRoot & utControlData.filename.Substring(0, iDataLen).Trim()
                    End If

                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_NOFILENAME, sControlName)
                        Else
                            ErrorLog("GetControlData", "Failed to get FILENAME from control file, " & sControlName)
                        End If
                        result = False
                    Else
                        utControlData.filename = ConvertSlashes(utControlData.filename)
                    End If

                    ' ANNOTATION
                    utControlData.annotation = New String(" "c, 128)
                    Dim tmpPtr12 As IntPtr = Marshal.StringToHGlobalAnsi("ANNOTATION")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr12, "", utControlData.annotation, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr12)
                    End Try
                    utControlData.annotation = utControlData.annotation.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        utControlData.annotation = ""
                    End If

                    ' USERNAME
                    utControlData.username = New String(" "c, 128)
                    Dim tmpPtr13 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr13, "", utControlData.username, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr13)
                    End Try
                    utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        If g_sAppType.Value = "WAPI" Then
                            utControlData.username = "DMSWAPI"
                        Else
                            ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                            result = False
                        End If
                    Else
                        If UserExists(utControlData.username) = -1 Then
                            ' User not found in system
                            If g_sAppType.Value = "WAPI" Then
                                utControlData.username = "DMSWAPI"
                            Else
                                utControlData.username = "DMSAPI"
                            End If
                        End If
                    End If

                    ' ACCESS
                    sTmpString = New String(" "c, 128)
                    Dim tmpPtr14 As IntPtr = Marshal.StringToHGlobalAnsi("ACCESS")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr14, "", sTmpString, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr14)
                    End Try
                    sTmpString = sTmpString.Substring(0, iDataLen).Trim()
                    utControlData.access = Conversion.Val(sTmpString)

                    If iDataLen = 0 Then
                        utControlData.access = 9

                    Else
                        If Conversion.Val(sTmpString) > -1 And Conversion.Val(sTmpString) < 10 Then
                            utControlData.access = Conversion.Val(sTmpString)
                        Else
                            utControlData.access = 9
                        End If
                    End If

                    If g_sAppType.Value = "WAPI" Then
                        g_sUsername = "DMSWAPI"
                    Else
                        g_sUsername = utControlData.username
                    End If

                    ' HDBONLY
                    sTmpString = New String(" "c, 128)
                    Dim tmpPtr15 As IntPtr = Marshal.StringToHGlobalAnsi("HDBONLY")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr15, "", sTmpString, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr15)
                    End Try
                    sTmpString = sTmpString.Substring(0, iDataLen).Trim()
                    utControlData.hdbonly = Not (sTmpString = "FALSE")
                    If iDataLen = 0 Then
                        utControlData.hdbonly = False
                    End If

                Case "ADDINDEX", "DELINDEX"
                    ' FILENAME
                    utControlData.filename = New String(" "c, 128)
                    Dim tmpPtr16 As IntPtr = Marshal.StringToHGlobalAnsi("FILENAME")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr16, "", utControlData.filename, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr16)
                    End Try

                    If g_sAppType.Value = "WAPI" Then
                        utControlData.filename = utControlData.filename.Substring(0, iDataLen).Trim()
                    Else
                        utControlData.filename = g_sHistoryRoot & utControlData.filename.Substring(0, iDataLen).Trim()
                    End If

                    If iDataLen = 0 Then
                        ErrorLog("GetControlData", "Failed to get FILENAME from control file, " & sControlName)
                        result = False
                    Else
                        utControlData.filename = ConvertSlashes(utControlData.filename)
                    End If

                    ' USERNAME
                    utControlData.username = New String(" "c, 128)
                    Dim tmpPtr17 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr17, "", utControlData.username, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr17)
                    End Try
                    utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then

                        If g_sAppType.Value = "WAPI" Then
                            utControlData.username = "DMSWAPI"
                        Else
                            ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                            result = False
                        End If

                    Else
                        If UserExists(utControlData.username) = -1 Then
                            ' User not found in system
                            If g_sAppType.Value = "WAPI" Then
                                utControlData.username = "DMSWAPI"
                            Else
                                utControlData.username = "DMSAPI"
                            End If
                        End If

                    End If

                    utControlData.access = 9
                    g_iIndexAccessLevel = utControlData.access
                    g_sUsername = utControlData.username

                    ' EMPTYONLY
                    sTmpString = New String(" "c, 128)
                    Dim tmpPtr18 As IntPtr = Marshal.StringToHGlobalAnsi("EMPTYONLY")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr18, "", sTmpString, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr18)
                    End Try
                    sTmpString = sTmpString.Substring(0, iDataLen).Trim()
                    utControlData.emptyonly = Not (sTmpString = "FALSE")
                    If iDataLen = 0 Then
                        utControlData.emptyonly = True
                    End If

                Case "LOG"
                    ' MESSAGE
                    utControlData.message = New String(" "c, 128)
                    Dim tmpPtr19 As IntPtr = Marshal.StringToHGlobalAnsi("MESSAGE")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr19, "", utControlData.message, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr19)
                    End Try
                    utControlData.message = utControlData.message.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        ErrorLog("GetControlData", "Failed to get MESSAGE from control file, " & sControlName)
                        result = False
                    End If

                    ' USERNAME
                    utControlData.username = New String(" "c, 128)
                    Dim tmpPtr20 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                    Try
                        iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr20, "", utControlData.username, 128, sControlName)
                    Finally
                        Marshal.FreeHGlobal(tmpPtr20)
                    End Try
                    utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                    If iDataLen = 0 Then
                        ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                        result = False
                    Else
                        If UserExists(utControlData.username) = -1 Then
                            ' User not found in system
                            utControlData.username = "DMSAPI"
                        End If

                        ' Get access level from user name
                        '                utControlData.access = UserExists((utControlData.username))
                        '                If (utControlData.access = -1) Then
                        ' User not found!
                        '                    utControlData.username = "DMSAPI"
                        '                    utControlData.access = 9
                        '                End If
                    End If

                    ' Default new items to lowest access level
                    g_iIndexAccessLevel = 9 ' utControlData.access
                    g_sUsername = utControlData.username
            End Select

            Return result

        Catch



            ErrorLog("GetControlData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Sub GetDocLinkData(ByRef sDocLinks As String, ByRef sDocLinksArr() As String)

        Dim iArrCntr As Integer

        Try

            iArrCntr = 1
            For iStrCntr As Integer = 1 To sDocLinks.Length
                If sDocLinks.Substring(iStrCntr - 1, 1) = "|" Then
                    iArrCntr += 1
                Else
                    ReDim Preserve sDocLinksArr(iArrCntr)
                    sDocLinksArr(iArrCntr) = sDocLinksArr(iArrCntr) & sDocLinks.Substring(iStrCntr - 1, 1)
                End If
            Next iStrCntr

        Catch



            ErrorLog("GetDocLinkData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Exit Sub
        End Try


    End Sub

    Function GetIndexListData(ByRef utIndexListData As g_utIndexListData) As Integer

        Dim result As Integer = 0
        Dim sIndexDesc As String = ""
        Dim iStrCntr As Integer

        result = PM_TRUE

        Try

            ' Check if we are at the end of the
            ' journal file
            If FileSystem.EOF(g_iIndexData) Then
                ' Reached end of file
                Return PM_FALSE
            End If

            ' Get the next line from the journal file
            sIndexDesc = FileSystem.LineInput(g_iIndexData)

            utIndexListData.cabinetcode = ""
            utIndexListData.cabinetname = ""
            utIndexListData.drawercode = ""
            utIndexListData.drawername = ""
            utIndexListData.foldercode = ""
            utIndexListData.foldername = ""

            If sIndexDesc.Trim().Length > 1 Then
                iStrCntr = 1

                ' Get cabinet code
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    utIndexListData.cabinetcode = utIndexListData.cabinetcode & sIndexDesc.Substring(iStrCntr - 1, 1)
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get cabinet name
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    utIndexListData.cabinetname = utIndexListData.cabinetname & sIndexDesc.Substring(iStrCntr - 1, 1)
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get drawer code
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    utIndexListData.drawercode = utIndexListData.drawercode & sIndexDesc.Substring(iStrCntr - 1, 1)
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get drawer name
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    utIndexListData.drawername = utIndexListData.drawername & sIndexDesc.Substring(iStrCntr - 1, 1)
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get folder code
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    utIndexListData.foldercode = utIndexListData.foldercode & sIndexDesc.Substring(iStrCntr - 1, 1)
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get folder name
                For iStrCntr = iStrCntr To sIndexDesc.Length
                    utIndexListData.foldername = utIndexListData.foldername & sIndexDesc.Substring(iStrCntr - 1, 1)
                Next iStrCntr
            End If

            Return result

        Catch



            Return PM_ERROR
        End Try

    End Function

    Function GetJournalData(ByRef sJournalData() As String, ByRef iRetryErrors As Integer) As Integer

        Dim result As Integer = 0
        Dim JournalData As Integer
        Dim sFilename, sJournalNumber, sExists As String

        result = True

        Try

            ' Filename of journal file
            If iRetryErrors Then
                sFilename = g_sHistoryRoot & "data\journal.rty"
            Else
                sFilename = g_sHistoryRoot & "data\journal.wrk"
            End If

            ' Find free file number and open
            JournalData = FileSystem.FreeFile()
            FileSystem.FileOpen(JournalData, sFilename, OpenMode.Input)

            ' Read through all of journal file
            While Not FileSystem.EOF(JournalData)
                sJournalNumber = FileSystem.LineInput(JournalData)

                If sJournalNumber.Trim().Length > 15 Then
                    ' Possible error with journal number,
                    ' can't be greater than 8 chars long
                    JournalErrorLog("GetJournalData", "Possible error with journal number, greater than 15 characters")
                    result = False

                    'Close journal file
                    FileSystem.FileClose(JournalData)

                    Return result
                End If

                If g_sCurrentJournalPos = "" Then
                    If sJournalNumber.Trim().Length > 1 Then
                        ReDim Preserve sJournalData(sJournalData.GetUpperBound(0) + 1)
                        sJournalData(sJournalData.GetUpperBound(0)) = sJournalNumber
                    End If
                Else
                    ' If current journal position exists, start at that point

                    'check to see if a retry journal exists
                    sExists = FileSystem.Dir(g_sHistoryRoot & "data\journal.rty", FileAttribute.Normal)

                    If g_sCurrentJournalPos = sJournalNumber.Trim() Then
                        If sJournalNumber.Trim().Length > 1 Then
                            ReDim Preserve sJournalData(sJournalData.GetUpperBound(0) + 1)
                            sJournalData(sJournalData.GetUpperBound(0)) = sJournalNumber
                            g_sCurrentJournalPos = ""
                        End If
                    End If
                End If
            End While

            ' Close journal file
            FileSystem.FileClose(JournalData)

            Return result

        Catch



            JournalErrorLog("GetJournalData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Function GetJournalPos(ByRef sJournalName As String) As String

        Dim result As String = String.Empty
        Dim JournalPos As Integer
        Dim sJournalPos As String = ""

        result = ""

        Try

            ' Find free file number and open
            JournalPos = FileSystem.FreeFile()
            FileSystem.FileOpen(JournalPos, g_sHistoryRoot & "data\" & sJournalName, OpenMode.Input)

            ' Read current journal position
            sJournalPos = FileSystem.LineInput(JournalPos)

            'Close journal file
            FileSystem.FileClose(JournalPos)

            Return sJournalPos.Trim()

        Catch



            JournalErrorLog("GetJournalPos", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return ""
        End Try

    End Function

    Sub GetKeywordsData(ByRef sKeywords As String, ByRef sKeywordsArr() As String)

        Dim iArrCntr As Integer

        Try

            iArrCntr = 1
            For iStrCntr As Integer = 1 To sKeywords.Length
                If sKeywords.Substring(iStrCntr - 1, 1) = "|" Then
                    iArrCntr += 1
                Else
                    ReDim Preserve sKeywordsArr(iArrCntr)
                    sKeywordsArr(iArrCntr) = sKeywordsArr(iArrCntr) & sKeywords.Substring(iStrCntr - 1, 1)
                End If
            Next iStrCntr

        Catch



            ErrorLog("GetKeywordsData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Exit Sub
        End Try


    End Sub

    Function IsLevelEmpty(ByRef iLevel As Integer, ByRef lSearchNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim ssEmpty As DAO.Snapshot
        Dim sSQLQuery As String = ""

        result = False

        Try

            Select Case iLevel
                Case CABINET
                    sSQLQuery = "SELECT drawer_num FROM drawer WHERE cabinet_num = " & lSearchNumber
                Case DRAWER
                    sSQLQuery = "SELECT folder_num FROM folder WHERE drawer_num = " & lSearchNumber
                Case FOLDER
                    sSQLQuery = "SELECT doc_num FROM document WHERE folder_num = " & lSearchNumber
            End Select

            ssEmpty = g_dbDDB.CreateSnapshot(sSQLQuery)
            DAO_DBEngine_definst.FreeLocks()

            If ssEmpty.RecordCount = 0 Then
                result = True
            End If

            ssEmpty.Close()
            ssEmpty = Nothing

            Return result

        Catch



            Return False
        End Try

    End Function

    Sub JournalErrorLog(ByRef FunctionName As String, ByRef ErrorMessage As String)

        FileSystem.PrintLine(fhJournalErrorlog, DateTime.Now.ToString("dd/MM/yy  HH:MM:ss") & " - " & "Func: " & FunctionName & ", " & ErrorMessage)

    End Sub

    Function KeywordExists(ByRef lDocumentNumber As Integer, ByRef lKeywordsNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim ssTmpExists As DAO.Snapshot

        Dim sSQLQuery As String = "SELECT key_num FROM keyword WHERE "
        sSQLQuery = sSQLQuery & "doc_num = " & CStr(lDocumentNumber) & " AND "
        sSQLQuery = sSQLQuery & "key_num = " & CStr(lKeywordsNumber)

        Try

            ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
            DAO_DBEngine_definst.FreeLocks()

            result = (ssTmpExists.RecordCount > 0)

            ssTmpExists.Close()
            Return result

        Catch



            ssTmpExists.Close()
            Return False
        End Try

    End Function

    Function KeywordsExists(ByRef sKeyword As String) As Integer

        Dim result As Integer = 0
        Dim ssTmpExists As DAO.Snapshot

        Dim sSQLQuery As String = "SELECT key_num FROM keywords WHERE "
        sSQLQuery = sSQLQuery & "keyword = '" & sKeyword & "'"

        Try

            ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
            DAO_DBEngine_definst.FreeLocks()

            result = ssTmpExists(0).Value

            ssTmpExists.Close()
            Return result

        Catch



            ssTmpExists.Close()
            Return 0
        End Try

    End Function

    Function OpenErrorLog(ByRef ControlName As String) As Integer

        Dim result As Integer = 0
        result = True

        Try

            fhErrorlog = FileSystem.FreeFile()
            FileSystem.FileOpen(fhErrorlog, g_sHistoryRoot & "tmp\" & ControlName & ".log", OpenMode.Append)

            Return result

        Catch



            Return False
        End Try

    End Function

    Function OpenIndexListData(ByRef sIndexListName As String) As Integer

        Dim result As Integer = 0
        result = True

        Try

            ' Find free file number and open
            g_iIndexData = FileSystem.FreeFile()
            FileSystem.FileOpen(g_iIndexData, sIndexListName, OpenMode.Input)

            Return result

        Catch



            Return False
        End Try

    End Function

    Function OpenJournalBad() As Integer

        Dim result As Integer = 0
        result = True

        Try

            fhJournalBad = FileSystem.FreeFile()
            FileSystem.FileOpen(fhJournalBad, g_sHistoryRoot & "data\journal.bad", OpenMode.Append)

            Return result

        Catch



            Return False
        End Try

    End Function

    Function OpenJournalErrorLog() As Integer

        Dim result As Integer = 0
        result = True

        Try

            fhJournalErrorlog = FileSystem.FreeFile()
            FileSystem.FileOpen(fhJournalErrorlog, g_sHistoryRoot & "data\journal.log", OpenMode.Append)

            Return result

        Catch



            Return False
        End Try

    End Function

    Function ProcessControlData(ByRef sControlName As String) As Integer

        Dim result As Integer = 0
        Dim utControlData As g_utControlData = g_utControlData.CreateInstance()
        Dim utCabinet As DMSDDB.g_utCabinet = DMSDDB.g_utCabinet.CreateInstance()
        Dim utDMSHistData As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
        Dim lCabinetNumber, lDrawerNumber, lFolderNumber, lFolderLinkNumber, lDocumentNumber, lDocumentLinkNumber, lKeywordsNumber As Integer
        Dim sPageName As String = ""
        Dim sTmpString, sTaskDesc As String
        Dim iBadCode, iIndexFlag As Integer
        Dim sLogFileName As String = ""
        Dim utIndexListData As g_utIndexListData = g_utIndexListData.CreateInstance()
        Dim f_IndexPreCheck As Integer

        Dim sActiveVolume As String = ""

        Dim sKeywords(0) As String, sDocLinks(0) As String, sLogMess(0) As String
        Dim utIndexCabinet() As g_utIndexCabinet = ArraysHelper.InitializeArray(Of g_utIndexCabinet)(1)

        result = True

        Try

            ReDim sLogMess(5)

            If Not (GetControlData(sControlName, utControlData)) Then
                If g_sAppType.Value = "WAPI" Then
                    'we should have return a value by now, so just exit
                Else
                    ErrorLog("ProcessControlData", "Failed to get control data")
                End If

                Return False
            End If

            sActiveVolume = GetVolumePath(GetActiveVolume())

            Select Case utControlData.task.ToUpper()
                Case "POPUP"
                    'Add to existing folder ...

                    Dim dbNumericTemp As Double
                    If Double.TryParse(utControlData.cabinetname, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Conversion.Val(utControlData.cabinetname) = 0 Then
                        iBadCode = True
                        ErrorWAPI(ERR_INVALIDCABINET, sControlName)
                    End If
                    Dim dbNumericTemp2 As Double
                    If Double.TryParse(utControlData.drawername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And Conversion.Val(utControlData.drawername) = 0 Then
                        iBadCode = True
                        ErrorWAPI(ERR_INVALIDDRAWER, sControlName)
                    End If
                    Dim dbNumericTemp3 As Double
                    If Double.TryParse(utControlData.foldername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) And Conversion.Val(utControlData.foldername) = 0 Then
                        iBadCode = True
                        ErrorWAPI(ERR_INVALIDFOLDER, sControlName)
                    End If

                    If iBadCode Then
                        Return False
                    End If

                    ' Create Document
                    lDocumentNumber = CreateDocument(CInt(Conversion.Val(utControlData.foldername)), utControlData)

                    If lDocumentNumber = 0 Then
                        ' Failed to create Document
                        ErrorWAPI(ERR_CREATEDOCUMENT, sControlName)
                        Return False
                    End If

                    If PutControlFileVar("DMSWAPI", "Cabinet", utControlData.cabinetname, sControlName) = PM_FALSE Then
                        '???
                    ElseIf PutControlFileVar("DMSWAPI", "Drawer", utControlData.drawername, sControlName) = PM_FALSE Then
                        '???
                    ElseIf PutControlFileVar("DMSWAPI", "Folder", utControlData.foldername, sControlName) = PM_FALSE Then
                        '???
                    ElseIf PutControlFileVar("DMSWAPI", "Document", Conversion.Str(lDocumentNumber), sControlName) = PM_FALSE Then
                        '???
                    End If

                    ' Create Document info
                    If Not (CreateDocInfo(lDocumentNumber, utControlData)) Then
                        ' Failed to create Document info
                        ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                        Return False
                    End If

                    sPageName = CreatePage(lDocumentNumber, utControlData)

                    If sPageName = "" Then
                        ' Failed to create page
                        ErrorWAPI(ERR_CREATEPAGE, sControlName)
                        Return False
                    End If

                    ' Create path for file copy
                    If Not MakePath(sActiveVolume & sPageName) Then
                        ErrorWAPI(ERR_CREATEPATH, sControlName)
                        Return False
                    End If

                    ' Move filename to page name
                    If DOCGeneralFunc.CopyFile(utControlData.filename, sActiveVolume & sPageName & "." & utControlData.doctype) = PM_FALSE Then
                        ErrorWAPI(ERR_COPYFILE, sControlName)
                        Return False
                    End If

                    If AmIExternalData(DOCUMENT, lDocumentNumber) = PM_TRUE Then
                        ' Document was added to a PMB folder, so let PMB know

                        ' Create Link document entry in the History database
                        '
                        ' Fill the structure ready to save away
                        utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, CInt(utControlData.cabinetname))
                        utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, CInt(utControlData.drawername))
                        utDMSHistData.foldercode.Value = GetDDBCode(FOLDER, CInt(utControlData.foldername))
                        utDMSHistData.docref.Value = "000000000"
                        Mid(utDMSHistData.docref.Value, 10 - Conversion.Str(lDocumentNumber).Trim().Length, Conversion.Str(lDocumentNumber).Trim().Length) = Conversion.Str(lDocumentNumber).Trim()
                        utDMSHistData.date_Renamed.Value = DateTime.FromOADate(GetDocDate(lDocumentNumber)).ToString("yyyyMMdd")
                        utDMSHistData.time.Value = DateTime.FromOADate(GetDocDate(lDocumentNumber)).ToString("HHMMss")
                        utDMSHistData.description.Value = utControlData.documentname
                        utDMSHistData.volume.Value = GetActiveVolumeName()
                        '                utDMSHistData.pagefile = StripSlashes(sPageName)
                        utDMSHistData.doctype.Value = utControlData.doctype
                        utDMSHistData.eventtype.Value = utControlData.event_Renamed

                        ' Update the history database
                        If Not (UpdateHDB(ADDDOCUMENT, g_sHistoryRoot, utDMSHistData)) Then
                            ' Failed to create Document history

                            ' ERR_UPDATEHISTORY returns success for DMS and an error code
                            ' to indicate that Updating the Remote History failed
                            ErrorWAPI(ERR_UPDATEHISTORY, sControlName)
                            ' We know this is external but we cant guarantee it is PMB
                            ' If it's not PMB we don't manage the Remote Database anyway

                            '   ProcessControlData = False
                            Return result
                        End If
                    End If

                    ErrorWAPI(ERR_SUCCESS, sControlName)

                Case "ADD"
                    ' Task ADD
                    '
                    ' Check and create Cabinet/Drawer/Folder/Document levels

                    If g_sAppType.Value <> "WAPI" Then
                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("ProcessControlData", "Failed to open log file")
                            Return False
                        End If


                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, ADD"
                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)

                        CloseLogFile(g_sHistoryRoot)
                    End If

                    iBadCode = False
                    ' First we check if there are any naughty internal/external codes
                    Dim dbNumericTemp4 As Double
                    If Double.TryParse(utControlData.cabinetname, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) And Conversion.Val(utControlData.cabinetname) = 0 Then
                        iBadCode = True

                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_INVALIDCABINET, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Invalid Cabinet code found, " & utControlData.cabinetname)
                        End If

                    End If
                    Dim dbNumericTemp5 As Double
                    If Double.TryParse(utControlData.drawername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) And Conversion.Val(utControlData.drawername) = 0 Then
                        iBadCode = True

                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_INVALIDDRAWER, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Invalid Drawer code found, " & utControlData.drawername)
                        End If

                    End If
                    Dim dbNumericTemp6 As Double
                    If Double.TryParse(utControlData.foldername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) And Conversion.Val(utControlData.foldername) = 0 Then
                        iBadCode = True

                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_INVALIDFOLDER, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Invalid Folder code found, " & utControlData.foldername)
                        End If

                    End If

                    If iBadCode Then
                        Return False
                    End If

                    If utControlData.cabinetname.Trim() <> "" Then
                        If utControlData.external Then
                            lCabinetNumber = DDBExists(CABINET, utControlData.cabinetname)
                        Else
                            lCabinetNumber = CInt(Conversion.Val(utControlData.cabinetname))
                        End If

                        If lCabinetNumber = 0 Then
                            ' Create Cabinet
                            lCabinetNumber = CreateCabinet(utControlData)

                            If lCabinetNumber = 0 Then
                                ' Failed to create Cabinet
                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_CREATECABINET, sControlName)
                                Else
                                    ErrorLog("ProcessControlData", "Failed to create Cabinet, " & utControlData.cabinetname)
                                End If

                                Return False
                            End If

                            If g_sAppType.Value <> "WAPI" Then
                                ' Append a log message to todays log file
                                If Not (OpenLogFile(g_sHistoryRoot)) Then
                                    ' Failed to log message
                                    ErrorLog("ProcessControlData", "Failed to open log file")
                                    Return False
                                End If

                                sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet created"
                                sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)

                                CloseLogFile(g_sHistoryRoot)

                            End If

                            If utControlData.drawername.Trim() <> "" Then
                                ' Create Drawer
                                lDrawerNumber = CreateDrawer(lCabinetNumber, utControlData)

                                If lDrawerNumber = 0 Then
                                    ' Failed to create Drawer
                                    If g_sAppType.Value = "WAPI" Then
                                        ErrorWAPI(ERR_CREATEDRAWER, sControlName)
                                    Else
                                        ErrorLog("ProcessControlData", "Failed to create Drawer, " & utControlData.drawername)
                                    End If

                                    Return False
                                End If

                                If g_sAppType.Value <> "WAPI" Then

                                    ' Append a log message to todays log file
                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                        ' Failed to log message
                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                        Return False
                                    End If

                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer created"
                                    sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                    sLogMess(3) = "Drawer - " & utControlData.drawername
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    CloseLogFile(g_sHistoryRoot)
                                End If

                                If utControlData.foldername.Trim() <> "" Then
                                    ' Create Folder
                                    lFolderNumber = CreateFolder(lCabinetNumber, lDrawerNumber, utControlData)

                                    If lFolderNumber = 0 Then
                                        ' Failed to create Folder
                                        If g_sAppType.Value = "WAPI" Then
                                            ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                        Else
                                            ErrorLog("ProcessControlData", "Failed to create Folder, " & utControlData.foldername)
                                        End If
                                        Return False
                                    End If

                                    If g_sAppType.Value <> "WAPI" Then
                                        ' Append a log message to todays log file
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            Return False
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                        sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                        sLogMess(3) = "Drawer - " & utControlData.drawername
                                        sLogMess(4) = "Folder - " & utControlData.foldername
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If

                                    If utControlData.documentname.Trim() <> "" Then
                                        ' Create Document
                                        lDocumentNumber = CreateDocument(lFolderNumber, utControlData)

                                        If lDocumentNumber = 0 Then
                                            ' Failed to create Document
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEDOCUMENT, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create Document, " & utControlData.documentname)
                                            End If

                                            Return False
                                        End If

                                        If g_sAppType.Value <> "WAPI" Then
                                            ' Append a log message to todays log file
                                            If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                ' Failed to log message
                                                ErrorLog("ProcessControlData", "Failed to open log file")
                                                Return False
                                            End If

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Document created"
                                            sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                            sLogMess(3) = "Drawer - " & utControlData.drawername
                                            sLogMess(4) = "Folder - " & utControlData.foldername
                                            sLogMess(5) = "Document - " & utControlData.documentname
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            CloseLogFile(g_sHistoryRoot)
                                        End If

                                        ' Create Document info
                                        If Not (CreateDocInfo(lDocumentNumber, utControlData)) Then
                                            ' Failed to create Document info
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create DocInfo, Document Number : " & lDocumentNumber)
                                            End If

                                            Return False
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            If utControlData.drawername.Trim() <> "" Then
                                If utControlData.external Then
                                    lDrawerNumber = DrawerExists(lCabinetNumber, utControlData.drawername)
                                Else
                                    lDrawerNumber = CInt(Conversion.Val(utControlData.drawername))
                                End If

                                If lDrawerNumber = 0 Then
                                    ' Create Drawer
                                    lDrawerNumber = CreateDrawer(lCabinetNumber, utControlData)

                                    If lDrawerNumber = 0 Then
                                        ' Failed to create Drawer
                                        If g_sAppType.Value = "WAPI" Then
                                            ErrorWAPI(ERR_CREATEDRAWER, sControlName)
                                        Else
                                            ErrorLog("ProcessControlData", "Failed to create Drawer, " & utControlData.drawername)
                                        End If

                                        Return False
                                    End If

                                    If g_sAppType.Value <> "WAPI" Then
                                        ' Append a log message to todays log file
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            Return False
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer created"
                                        sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                        sLogMess(3) = "Drawer - " & utControlData.drawername
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If

                                    If utControlData.foldername.Trim() <> "" Then
                                        ' Create Folder
                                        lFolderNumber = CreateFolder(lCabinetNumber, lDrawerNumber, utControlData)

                                        If lFolderNumber = 0 Then
                                            ' Failed to create Folder
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create Folder, " & utControlData.foldername)
                                            End If

                                            Return False
                                        End If

                                        If g_sAppType.Value <> "WAPI" Then
                                            ' Append a log message to todays log file
                                            If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                ' Failed to log message
                                                ErrorLog("ProcessControlData", "Failed to open log file")
                                                Return False
                                            End If

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                            sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                            sLogMess(3) = "Drawer - " & utControlData.drawername
                                            sLogMess(4) = "Folder - " & utControlData.foldername
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            CloseLogFile(g_sHistoryRoot)
                                        End If

                                        If utControlData.documentname.Trim() <> "" Then
                                            ' Create Document
                                            lDocumentNumber = CreateDocument(lFolderNumber, utControlData)

                                            If lDocumentNumber = 0 Then
                                                ' Failed to create Document
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_CREATEDOCUMENT, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "Failed to create Document, " & utControlData.documentname)
                                                End If

                                                Return False
                                            End If

                                            If g_sAppType.Value <> "WAPI" Then
                                                ' Append a log message to todays log file
                                                If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                    ' Failed to log message
                                                    ErrorLog("ProcessControlData", "Failed to open log file")
                                                    Return False
                                                End If

                                                sLogMess(1) = "DocuMaster API Daemon - DocuMaster Document created"
                                                sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                                sLogMess(3) = "Drawer - " & utControlData.drawername
                                                sLogMess(4) = "Folder - " & utControlData.foldername
                                                sLogMess(5) = "Document - " & utControlData.documentname
                                                iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                CloseLogFile(g_sHistoryRoot)
                                            End If

                                            ' Create Document info
                                            If Not (CreateDocInfo(lDocumentNumber, utControlData)) Then
                                                ' Failed to create Document info
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "Failed to create DocInfo, Document Number : " & lDocumentNumber)
                                                End If

                                                Return False
                                            End If
                                        End If
                                    End If
                                Else
                                    If utControlData.foldername.Trim() <> "" Then
                                        If utControlData.external Then
                                            'if we are using external codes...
                                            lFolderNumber = FolderExistsInDB(lDrawerNumber, utControlData.foldername)
                                        Else
                                            lFolderNumber = CInt(Conversion.Val(utControlData.foldername))
                                        End If

                                        If lFolderNumber = 0 Then
                                            ' Create Folder
                                            lFolderNumber = CreateFolder(lCabinetNumber, lDrawerNumber, utControlData)

                                            If lFolderNumber = 0 Then
                                                ' Failed to create Folder
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "Failed to create Folder, " & utControlData.foldername)
                                                End If

                                                Return False
                                            End If

                                            If g_sAppType.Value <> "WAPI" Then
                                                ' Append a log message to todays log file
                                                If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                    ' Failed to log message
                                                    ErrorLog("ProcessControlData", "Failed to open log file")
                                                    Return False
                                                End If

                                                sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                                sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                                sLogMess(3) = "Drawer - " & utControlData.drawername
                                                sLogMess(4) = "Folder - " & utControlData.foldername
                                                iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                CloseLogFile(g_sHistoryRoot)
                                            End If

                                            If utControlData.documentname.Trim() <> "" Then
                                                ' Create Document
                                                lDocumentNumber = CreateDocument(lFolderNumber, utControlData)

                                                If lDocumentNumber = 0 Then
                                                    ' Failed to create Document
                                                    If g_sAppType.Value = "WAPI" Then
                                                        ErrorWAPI(ERR_CREATEDOCUMENT, sControlName)
                                                    Else
                                                        ErrorLog("ProcessControlData", "Failed to create Document, " & utControlData.documentname)
                                                    End If

                                                    Return False
                                                End If

                                                If g_sAppType.Value <> "WAPI" Then
                                                    ' Append a log message to todays log file
                                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                        ' Failed to log message
                                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                                        Return False
                                                    End If

                                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Document created"
                                                    sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                                    sLogMess(3) = "Drawer - " & utControlData.drawername
                                                    sLogMess(4) = "Folder - " & utControlData.foldername
                                                    sLogMess(5) = "Document - " & utControlData.documentname
                                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                    CloseLogFile(g_sHistoryRoot)
                                                End If

                                                ' Create Document info
                                                If Not (CreateDocInfo(lDocumentNumber, utControlData)) Then
                                                    ' Failed to create Document info
                                                    If g_sAppType.Value = "WAPI" Then
                                                        ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                                                    Else
                                                        ErrorLog("ProcessControlData", "Failed to create DocInfo, Document Number : " & lDocumentNumber)
                                                    End If

                                                    Return False
                                                End If
                                            End If
                                        Else
                                            ' sim
                                            If utControlData.documentname.Trim() <> "" Then
                                                '                                        lDocumentNumber = DocExists(lFolderNumber, (utControlData.documentname))

                                                '                                        If (lDocumentNumber = 0) Then
                                                ' Create Document
                                                lDocumentNumber = CreateDocument(lFolderNumber, utControlData)

                                                If lDocumentNumber = 0 Then
                                                    ' Failed to create Document
                                                    If g_sAppType.Value = "WAPI" Then
                                                        ErrorWAPI(ERR_CREATEDOCUMENT, sControlName)
                                                    Else
                                                        ErrorLog("ProcessControlData", "Failed to create Document, " & utControlData.documentname)
                                                    End If

                                                    Return False
                                                End If

                                                If g_sAppType.Value <> "WAPI" Then
                                                    ' Append a log message to todays log file
                                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                        ' Failed to log message
                                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                                        Return False
                                                    End If

                                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Document created"
                                                    sLogMess(2) = "Cabinet - " & utControlData.cabinetname
                                                    sLogMess(3) = "Drawer - " & utControlData.drawername
                                                    sLogMess(4) = "Folder - " & utControlData.foldername
                                                    sLogMess(5) = "Document - " & utControlData.documentname
                                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                    CloseLogFile(g_sHistoryRoot)
                                                End If

                                                ' Create Document info
                                                If Not (CreateDocInfo(lDocumentNumber, utControlData)) Then
                                                    ' Failed to create Document info
                                                    If g_sAppType.Value = "WAPI" Then
                                                        ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                                                    Else
                                                        ErrorLog("ProcessControlData", "Failed to create DocInfo, Document Number : " & lDocumentNumber)
                                                    End If

                                                    Return False
                                                End If
                                                '                                        End If
                                            Else
                                                ' No Document name supplied
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_NODOCUMENT, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "No document name supplied")
                                                End If

                                                Return False
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    ' We should now have all levels needed to continue

                    If Not (utControlData.hdbonly) Then
                        ' Not selected for histoty database only, so continue
                        '
                        ' Create Keywords

                        GetKeywordsData(utControlData.keywords, sKeywords)

                        For iCntr As Integer = 1 To sKeywords.GetUpperBound(0)
                            If sKeywords(iCntr).Trim() <> "" Then
                                lKeywordsNumber = KeywordsExists(sKeywords(iCntr))

                                If lKeywordsNumber = 0 Then
                                    ' Create default Keyword
                                    lKeywordsNumber = CreateKeywords(sKeywords(iCntr))

                                    If lKeywordsNumber = 0 Then
                                        ' Failed to create default keyword
                                        If g_sAppType.Value = "WAPI" Then
                                            ErrorWAPI(ERR_CREATEKEYWORD, sControlName)
                                        Else
                                            ErrorLog("ProcessControlData", "Failed to create default Keyword, " & sKeywords(iCntr))
                                        End If

                                        Return False
                                    End If
                                End If

                                If Not (KeywordExists(lDocumentNumber, lKeywordsNumber)) Then
                                    If Not (CreateKeyword(lDocumentNumber, lKeywordsNumber, utControlData)) Then
                                        ' Failed to create keyword
                                        If g_sAppType.Value = "WAPI" Then
                                            ErrorWAPI(ERR_CREATEKEYWORD, sControlName)
                                        Else
                                            ErrorLog("ProcessControlData", "Failed to create Keyword, Keyword Number : " & lKeywordsNumber)
                                        End If

                                        Return False
                                    End If
                                End If
                            End If
                        Next iCntr

                        ' Create Annotations

                        If utControlData.annotation <> "" Then
                            If Not (AnnotationExists(lDocumentNumber, utControlData.annotation)) Then
                                If Not (CreateAnnotations(lDocumentNumber, utControlData)) Then
                                    ' Failed to create annotation
                                    If g_sAppType.Value = "WAPI" Then
                                        ErrorWAPI(ERR_CREATEANNOTATION, sControlName)
                                    Else
                                        ErrorLog("ProcessControlData", "Failed to create Annotation, " & utControlData.annotation)
                                    End If

                                    Return False
                                End If
                            End If
                        End If

                        ' Create Page
                        sPageName = CreatePage(lDocumentNumber, utControlData)

                        If sPageName = "" Then
                            ' Failed to create page
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_CREATEPAGE, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Failed to create Page, " & utControlData.filename)
                            End If

                            Return False
                        End If

                        ' Create Document Links, if any

                        GetDocLinkData(utControlData.linkfolder.Trim(), sDocLinks)

                        For iCntr As Integer = 1 To sDocLinks.GetUpperBound(0)
                            If sDocLinks(iCntr).Trim() <> "" Then
                                lFolderLinkNumber = GetDDBNumber(FOLDER, sDocLinks(iCntr).Trim(), sTmpString)

                                If lFolderLinkNumber > 0 Then
                                    lDocumentLinkNumber = DocExists(lFolderLinkNumber, utControlData.documentname)

                                    If lDocumentLinkNumber = 0 Then
                                        ' Create Document
                                        lDocumentLinkNumber = CreateLinkDocument(lFolderNumber, lDocumentNumber, utControlData)

                                        If lDocumentLinkNumber = 0 Then
                                            ' Failed to create Document
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEDOCLINK, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create link Document, " & utControlData.documentname)
                                            End If

                                            Return False
                                        End If

                                        If g_sAppType.Value <> "WAPI" Then
                                            ' Append a log message to todays log file
                                            If Not (OpenLogFile(g_sHistoryRoot)) Then
                                                ' Failed to log message
                                                ErrorLog("ProcessControlData", "Failed to open log file")
                                                Return False
                                            End If

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Link Document created, " & utControlData.documentname
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            CloseLogFile(g_sHistoryRoot)
                                        End If

                                        ' Create Document info
                                        If Not (CreateDocInfo(lDocumentLinkNumber, utControlData)) Then
                                            ' Failed to create Document info
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEDOCINFO, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create link DocInfo, Document Number : " & lDocumentNumber)
                                            End If

                                            Return False
                                        End If
                                    End If

                                    'If g_sAppType <> "WAPI" Then
                                    ' Create Link document entry in the History database
                                    '
                                    ' Fill the structure ready to save away
                                    utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
                                    utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, lDrawerNumber)
                                    utDMSHistData.foldercode.Value = sDocLinks(iCntr).Trim()
                                    utDMSHistData.docref.Value = "000000000"
                                    Mid(utDMSHistData.docref.Value, 10 - Conversion.Str(lDocumentNumber).Trim().Length, Conversion.Str(lDocumentNumber).Trim().Length) = Conversion.Str(lDocumentNumber).Trim()
                                    utDMSHistData.date_Renamed.Value = DateTime.FromOADate(GetDocDate(lDocumentLinkNumber)).ToString("yyyyMMdd")
                                    utDMSHistData.time.Value = DateTime.FromOADate(GetDocDate(lDocumentLinkNumber)).ToString("HHMMss")
                                    utDMSHistData.description.Value = utControlData.documentname
                                    utDMSHistData.volume.Value = GetActiveVolumeName()
                                    '                                utDMSHistData.pagefile = StripSlashes(sPageName)
                                    utDMSHistData.doctype.Value = utControlData.doctype
                                    utDMSHistData.eventtype.Value = utControlData.event_Renamed

                                    ' Update the history database
                                    If Not (UpdateHDB(ADDDOCUMENT, g_sHistoryRoot, utDMSHistData)) Then
                                        ' Failed to create Document history
                                        ErrorLog("ProcessControlData", "Failed to create Link Document History, " & utControlData.documentname)
                                        Return False
                                    End If

                                    ' Append a log message to todays log file
                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                        ' Failed to log message
                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                        Return False
                                    End If

                                    sLogMess(1) = "DocuMaster API Daemon - Local History Link Document created, " & utControlData.documentname
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    CloseLogFile(g_sHistoryRoot)

                                    'End If
                                End If
                            End If
                        Next iCntr

                        'If g_sAppType <> "WAPI" Then
                        ' Create document entry in the History database
                        '
                        ' Fill the structure ready to save away
                        utDMSHistData.cabinetcode.Value = GetDDBCode(CABINET, lCabinetNumber)
                        utDMSHistData.drawercode.Value = GetDDBCode(DRAWER, lDrawerNumber)
                        utDMSHistData.foldercode.Value = GetDDBCode(FOLDER, lFolderNumber)
                        utDMSHistData.docref.Value = "000000000"
                        Mid(utDMSHistData.docref.Value, 10 - Conversion.Str(lDocumentNumber).Trim().Length, Conversion.Str(lDocumentNumber).Trim().Length) = Conversion.Str(lDocumentNumber).Trim()
                        utDMSHistData.date_Renamed.Value = DateTime.FromOADate(GetDocDate(lDocumentNumber)).ToString("yyyyMMdd")
                        utDMSHistData.time.Value = DateTime.FromOADate(GetDocDate(lDocumentNumber)).ToString("HHMMss")
                        utDMSHistData.description.Value = utControlData.documentname
                        utDMSHistData.volume.Value = GetActiveVolumeName()
                        '                    utDMSHistData.pagefile = StripSlashes(sPageName)
                        utDMSHistData.doctype.Value = utControlData.doctype
                        utDMSHistData.eventtype.Value = utControlData.event_Renamed

                        ' Update the history database
                        If Not (UpdateHDB(ADDDOCUMENT, g_sHistoryRoot, utDMSHistData)) Then
                            ' Failed to create Document history
                            ErrorLog("ProcessControlData", "Failed to create Document History, " & utControlData.documentname)
                            Return False
                        End If

                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("ProcessControlData", "Failed to open log file")
                            Return False
                        End If

                        sLogMess(1) = "DocuMaster API Daemon - Local History Document created, " & utControlData.documentname
                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                        CloseLogFile(g_sHistoryRoot)
                        'End If

                        ' Create path for file copy
                        If Not MakePath(sActiveVolume & sPageName) Then
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_CREATEPATH, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Failed to make path, " & g_sDBRoot & sPageName)
                            End If

                            Return False
                        End If

                        ' Move filename to page name
                        If DOCGeneralFunc.CopyFile(utControlData.filename, sActiveVolume & sPageName & "." & utControlData.doctype) = PM_FALSE Then
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_COPYFILE, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Failed to copy file, " & utControlData.filename & " to " & g_sDBRoot & sPageName & "." & utControlData.doctype)
                            End If

                            Return False
                        End If

                        If g_sAppType.Value = "WAPI" Then

                            If PutControlFileVar("DMSWAPI", "DOCUMENT", lDocumentNumber.ToString(), sControlName) = PM_FALSE Then
                                '???
                            End If
                            ErrorWAPI(ERR_SUCCESS, sControlName)

                        Else
                            If KillFile(utControlData.filename) = PM_FALSE Then
                                ErrorLog("ProcessControlData", "Failed to delete file, " & utControlData.filename)
                                Return False
                            End If
                        End If

                    End If

                Case "ADDINDEX"
                    ' Task ADDINDEX
                    '
                    ' Check and create Cabinet/Drawer/Folder levels
                    If g_sAppType.Value <> "WAPI" Then
                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("ProcessControlData", "Failed to open log file")
                            Return False
                        End If

                        sLogFileName = DateTime.Now.ToString("ddMMyy")

                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, ADDINDEX"
                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                    End If

                    ' Open the index file
                    If Not (OpenIndexListData(utControlData.filename)) Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_OPENFILE, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Failed to open the index list data file, " & utControlData.filename)
                        End If

                        Return False
                    End If

                    ' Fill the index cabinet array
                    If FillIndexCabinet(utIndexCabinet) = PM_FALSE Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_INDEXCABINET, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Failed to fill the index cabinet array")
                        End If

                        Return False
                    End If

                    iIndexFlag = PM_TRUE

                    ' Get a line the the index file
                    iIndexFlag = GetIndexListData(utIndexListData)

                    ' Check if the cabinet exists, if not
                    ' we can set the precheck to false
                    '                lCabinetNumber = DDBExists(CABINET, (utIndexListData.cabinetcode))

                    lCabinetNumber = CheckIndexCabinet(utIndexCabinet, utIndexListData.cabinetcode)
                    f_IndexPreCheck = Not (lCabinetNumber = 0)

                    While (iIndexFlag = PM_TRUE)
                        iBadCode = False
                        ' First we check if there are any naughty external codes
                        Dim dbNumericTemp7 As Double
                        If Double.TryParse(utIndexListData.cabinetcode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) And Conversion.Val(utIndexListData.cabinetcode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value <> "WAPI" Then
                                ErrorLog("ProcessControlData", "Invalid Cabinet code found, " & utIndexListData.cabinetcode)
                            End If
                        End If
                        Dim dbNumericTemp8 As Double
                        If Double.TryParse(utIndexListData.drawercode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) And Conversion.Val(utIndexListData.drawercode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value <> "WAPI" Then
                                ErrorLog("ProcessControlData", "Invalid Drawer code found, " & utIndexListData.drawercode)
                            End If
                        End If
                        Dim dbNumericTemp9 As Double
                        If Double.TryParse(utIndexListData.foldercode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) And Conversion.Val(utIndexListData.foldercode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value <> "WAPI" Then
                                ErrorLog("ProcessControlData", "Invalid Folder code found, " & utIndexListData.foldercode)
                            End If
                        End If

                        If iBadCode Then
                            ' Append a log message to todays log file
                            If g_sAppType.Value <> "WAPI" Then
                                sLogMess(1) = "DocuMaster API Daemon - Invalid code found - " & utIndexListData.cabinetcode & "|" & utIndexListData.cabinetname & "|" & utIndexListData.drawercode & "|" & utIndexListData.drawername & "|" & utIndexListData.foldercode & "|" & utIndexListData.foldername
                                iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)
                            End If

                            '                        CloseLogFile g_sHistoryRoot
                        Else
                            If utIndexListData.cabinetname.Trim() <> "" And utIndexListData.cabinetcode.Trim() <> "" Then
                                '                            lCabinetNumber = DDBExists(CABINET, (utIndexListData.CabinetCode))

                                ' Check if the cabinet code exists in the array
                                lCabinetNumber = CheckIndexCabinet(utIndexCabinet, utIndexListData.cabinetcode)

                                If lCabinetNumber <> 0 Then
                                    If f_IndexPreCheck Then
                                        If CabinetChanged(lCabinetNumber, utIndexListData.cabinetname) Then
                                            ' Append a log message to todays log file

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet Changed"
                                            sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            '                                        CloseLogFile g_sHistoryRoot
                                        End If
                                    End If
                                End If

                                If lCabinetNumber = 0 Then
                                    ' Create Cabinet
                                    lCabinetNumber = CreateIndexCabinet(utIndexListData)

                                    If lCabinetNumber = 0 Then
                                        ' Failed to create Cabinet
                                        If g_sAppType.Value = "WAPI" Then
                                            ErrorWAPI(ERR_CREATECABINET, sControlName)
                                        Else
                                            ErrorLog("ProcessControlData", "Failed to create index Cabinet, " & utIndexListData.cabinetname)
                                        End If
                                        result = False

                                        ' Close the index file
                                        If Not (CloseIndexListData()) Then
                                            ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                        End If
                                        Return result
                                    End If

                                    ' Append a log message to todays log file

                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet created"
                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    '                                CloseLogFile g_sHistoryRoot

                                    ' Place the new cabinet code and number into the
                                    ' array so next time round, the function will know
                                    ' that the cabinet has been created.
                                    ReDim Preserve utIndexCabinet(utIndexCabinet.GetUpperBound(0) + 1)
                                    utIndexCabinet(utIndexCabinet.GetUpperBound(0)).cabinetcode = utIndexListData.cabinetcode
                                    utIndexCabinet(utIndexCabinet.GetUpperBound(0)).CabinetNumber = lCabinetNumber

                                    If utIndexListData.drawername.Trim() <> "" And utIndexListData.drawercode.Trim() <> "" Then
                                        ' Create Drawer
                                        lDrawerNumber = CreateIndexDrawer(lCabinetNumber, utIndexListData)

                                        If lDrawerNumber = 0 Then
                                            ' Failed to create Drawer
                                            If g_sAppType.Value = "WAPI" Then
                                                ErrorWAPI(ERR_CREATEDRAWER, sControlName)
                                            Else
                                                ErrorLog("ProcessControlData", "Failed to create index Drawer, " & utIndexListData.drawername)
                                            End If
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        ' Append a log message to todays log file

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer created"
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                        sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        '                                    CloseLogFile g_sHistoryRoot


                                        If utIndexListData.foldername.Trim() <> "" And utIndexListData.foldercode.Trim() <> "" Then
                                            ' Create Folder
                                            lFolderNumber = CreateIndexFolder(lCabinetNumber, lDrawerNumber, utIndexListData)

                                            If lFolderNumber = 0 Then
                                                ' Failed to create Folder
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "Failed to create index Folder, " & utIndexListData.foldername)
                                                End If
                                                result = False

                                                ' Close the index file
                                                If Not (CloseIndexListData()) Then
                                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                                End If
                                                Return result
                                            End If

                                            ' Append a log message to todays log file

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                            sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                            sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                            sLogMess(4) = "Folder - " & utIndexListData.foldercode & ", " & utIndexListData.foldername
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            '                                        CloseLogFile g_sHistoryRoot
                                        End If
                                    End If
                                Else
                                    If utIndexListData.drawername.Trim() <> "" And utIndexListData.drawercode.Trim() <> "" Then
                                        lDrawerNumber = DrawerExists(lCabinetNumber, utIndexListData.drawercode)

                                        If lDrawerNumber <> 0 Then
                                            If f_IndexPreCheck Then
                                                If DrawerChanged(lCabinetNumber, lDrawerNumber, utIndexListData.drawername) Then
                                                    ' Append a log message to todays log file

                                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer changed"
                                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                                    sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                    '                                                CloseLogFile g_sHistoryRoot
                                                End If
                                            End If
                                        End If

                                        If lDrawerNumber = 0 Then
                                            ' Create Drawer
                                            lDrawerNumber = CreateIndexDrawer(lCabinetNumber, utIndexListData)

                                            If lDrawerNumber = 0 Then
                                                ' Failed to create Drawer
                                                If g_sAppType.Value = "WAPI" Then
                                                    ErrorWAPI(ERR_CREATEDRAWER, sControlName)
                                                Else
                                                    ErrorLog("ProcessControlData", "Failed to create index Drawer, " & utIndexListData.drawername)
                                                End If
                                                result = False

                                                ' Close the index file
                                                If Not (CloseIndexListData()) Then
                                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                                End If
                                                Return result
                                            End If

                                            ' Append a log message to todays log file

                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer created"
                                            sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                            sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                            '                                        CloseLogFile g_sHistoryRoot

                                            If utIndexListData.foldername.Trim() <> "" And utIndexListData.foldercode.Trim() <> "" Then
                                                ' Create Folder
                                                lFolderNumber = CreateIndexFolder(lCabinetNumber, lDrawerNumber, utIndexListData)

                                                If lFolderNumber = 0 Then
                                                    ' Failed to create Folder
                                                    If g_sAppType.Value = "WAPI" Then
                                                        ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                                    Else
                                                        ErrorLog("ProcessControlData", "Failed to create index Folder, " & utIndexListData.foldername)
                                                    End If
                                                    result = False

                                                    ' Close the index file
                                                    If Not (CloseIndexListData()) Then
                                                        ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                                    End If
                                                    Return result
                                                End If

                                                ' Append a log message to todays log file

                                                sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                                sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                                sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                                sLogMess(4) = "Folder - " & utIndexListData.foldercode & ", " & utIndexListData.foldername
                                                iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                '                                            CloseLogFile g_sHistoryRoot
                                            End If
                                        Else
                                            If utIndexListData.foldername.Trim() <> "" And utIndexListData.foldercode.Trim() <> "" Then
                                                lFolderNumber = FolderExistsInDB(lDrawerNumber, utIndexListData.foldercode)

                                                If lFolderNumber <> 0 Then
                                                    If f_IndexPreCheck Then
                                                        If FolderChanged(lCabinetNumber, lDrawerNumber, lFolderNumber, utIndexListData.foldername) Then

                                                            ' Append a log message to todays log file
                                                            sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder changed"
                                                            sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                                            sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                                            sLogMess(4) = "Folder - " & utIndexListData.foldercode & ", " & utIndexListData.foldername
                                                            iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                        End If
                                                    End If
                                                End If

                                                If lFolderNumber = 0 Then
                                                    ' Create Folder
                                                    lFolderNumber = CreateIndexFolder(lCabinetNumber, lDrawerNumber, utIndexListData)

                                                    If lFolderNumber = 0 Then
                                                        ' Failed to create Folder
                                                        If g_sAppType.Value = "WAPI" Then
                                                            ErrorWAPI(ERR_CREATEFOLDER, sControlName)
                                                        Else
                                                            ErrorLog("ProcessControlData", "Failed to create index Folder, " & utIndexListData.foldername)
                                                        End If
                                                        result = False

                                                        ' Close the index file
                                                        If Not (CloseIndexListData()) Then
                                                            ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                                        End If
                                                        Return result
                                                    End If

                                                    ' Append a log message to todays log file

                                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode & ", " & utIndexListData.cabinetname
                                                    sLogMess(3) = "Drawer - " & utIndexListData.drawercode & ", " & utIndexListData.drawername
                                                    sLogMess(4) = "Folder - " & utIndexListData.foldercode & ", " & utIndexListData.foldername
                                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        ' MUST commit data here instead of at the end,
                        ' because, can you believe it, its ANOTHER VB limitation !!!
                        g_iTmp = 0
                        g_iRC = CommitDatabase()
                        Select Case g_iRC
                            Case PM_TRUE
                            Case PM_FALSE

                                DAO_DBEngine_definst.Rollback()

                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_COMMITDB, sControlName)
                                Else
                                    sLogMess(1) = "DocuMaster API Daemon - Failed to commit - " & utIndexListData.cabinetcode & "|" & utIndexListData.cabinetname & "|" & utIndexListData.drawercode & "|" & utIndexListData.drawername & "|" & utIndexListData.foldercode & "|" & utIndexListData.foldername
                                    iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)
                                    Interaction.MsgBox("Commit Failed.", MB_ICONEXCLAMATION, "ProcessControlData")
                                End If

                            Case PM_CANCEL
                                ' Failed retrying the commit, must rollback
                                DAO_DBEngine_definst.Rollback()

                                ' Append a log message to todays log file
                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_COMMITDB, sControlName)
                                Else

                                    sLogMess(1) = "DocuMaster API Daemon - Failed to commit - " & utIndexListData.cabinetcode & "|" & utIndexListData.cabinetname & "|" & utIndexListData.drawercode & "|" & utIndexListData.drawername & "|" & utIndexListData.foldercode & "|" & utIndexListData.foldername
                                    iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)
                                End If

                            Case Else

                                DAO_DBEngine_definst.Rollback()

                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_COMMITDB, sControlName)
                                Else
                                    sLogMess(1) = "DocuMaster API Daemon - Failed to commit - " & utIndexListData.cabinetcode & "|" & utIndexListData.cabinetname & "|" & utIndexListData.drawercode & "|" & utIndexListData.drawername & "|" & utIndexListData.foldercode & "|" & utIndexListData.foldername & "Err = " & CStr(g_iRC)
                                    iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)
                                End If
                                '                            MsgBox "Commit Failed. - " & g_iRC%, MB_ICONEXCLAMATION, "ProcessControlData"
                        End Select

                        DAO_DBEngine_definst.BeginTrans()


                        ' Check to see if the log filename needs to
                        ' be changed
                        If (sLogFileName <> DateTime.Now.ToString("ddMMyy")) And g_sAppType.Value <> "WAPI" Then
                            ' Date must have changed
                            '
                            ' We must now close the log file, and reopen
                            ' so it can create the new log filename
                            CloseLogFile(g_sHistoryRoot)

                            If Not (OpenLogFile(g_sHistoryRoot)) Then
                                ' Failed to log message
                                ErrorLog("ProcessControlData", "Failed to open log file")
                                result = False

                                ' Close the index file
                                If Not (CloseIndexListData()) Then
                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                End If
                                Return result
                            End If

                            sLogFileName = DateTime.Now.ToString("ddMMyy")
                        End If

                        ' Get a line from the index file
                        iIndexFlag = GetIndexListData(utIndexListData)

                        'salvo(110796)
                        If g_sAppType.Value <> "WAPI" Then
                            'change the icon so we know its still working...
                            '                        If (frmDMSMain.Icon = frmDMSMain!imgProcess1.Picture) Then
                            '                            frmDMSMain.Icon = frmDMSMain!imgProcess2.Picture
                            '                            ' Let other applications have a go...
                            '                        Else
                            '                            frmDMSMain.Icon = frmDMSMain!imgProcess1.Picture
                            '                            ' Let other applications have a go...
                            '                        End If
                        End If
                        Application.DoEvents()

                    End While

                    ' Close the index file
                    If Not (CloseIndexListData()) Then
                        ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                    End If

                    ' Close the log file
                    CloseLogFile(g_sHistoryRoot)

                    If iIndexFlag = PM_ERROR Then
                        ' A Error accurred getting the index data
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_INDEXDATA, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Failed to get index list data from file, " & utControlData.filename)
                        End If

                        Return False
                    End If

                    If g_sAppType.Value = "WAPI" Then
                        ErrorWAPI(ERR_SUCCESS, sControlName)
                    End If

                Case "DELINDEX"
                    ' Task DELINDEX
                    '
                    ' Delete Cabinet/Drawer/Folder levels

                    If g_sAppType.Value <> "WAPI" Then
                        ' Append a log message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("ProcessControlData", "Failed to open log file")
                            Return False
                        End If

                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, DELINDEX"
                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)

                        CloseLogFile(g_sHistoryRoot)
                    End If

                    ' Get the user's access level
                    g_iUserAccessLevel = UserExists(utControlData.username)

                    If g_iUserAccessLevel = -1 Then
                        g_iUserAccessLevel = 9
                    End If

                    ' Get list from filename
                    '                If Not (GetIndexListData(utControlData.filename, utIndexListData)) Then
                    '                    ErrorLog "ProcessControlData", "Failed to get index list data for file, " & utControlData.filename
                    '                End If

                    ' Open the index file
                    If Not (OpenIndexListData(utControlData.filename)) Then
                        If g_sAppType.Value = "WAPI" Then
                            ErrorWAPI(ERR_OPENFILE, sControlName)
                        Else
                            ErrorLog("ProcessControlData", "Failed to open the index list data file, " & utControlData.filename)
                        End If

                        Return False
                    End If

                    iIndexFlag = PM_TRUE

                    ' Get a line the the index file
                    iIndexFlag = GetIndexListData(utIndexListData)

                    While (iIndexFlag = PM_TRUE)
                        iBadCode = False
                        ' First we check if there are any naughty external codes
                        Dim dbNumericTemp10 As Double
                        If Double.TryParse(utIndexListData.cabinetcode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) And Conversion.Val(utIndexListData.cabinetcode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_INVALIDCABINET, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Invalid Cabinet code found, " & utIndexListData.cabinetcode)
                            End If
                        End If
                        Dim dbNumericTemp11 As Double
                        If Double.TryParse(utIndexListData.drawercode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) And Conversion.Val(utIndexListData.drawercode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_INVALIDDRAWER, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Invalid Drawer code found, " & utIndexListData.drawercode)
                            End If
                        End If
                        Dim dbNumericTemp12 As Double
                        If Double.TryParse(utIndexListData.foldercode, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) And Conversion.Val(utIndexListData.foldercode) = 0 Then
                            iBadCode = True
                            If g_sAppType.Value = "WAPI" Then
                                ErrorWAPI(ERR_INVALIDFOLDER, sControlName)
                            Else
                                ErrorLog("ProcessControlData", "Invalid Folder code found, " & utIndexListData.foldercode)
                            End If
                        End If

                        If iBadCode Then
                            result = False

                            ' Close the index file
                            If Not (CloseIndexListData()) Then
                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                            End If
                            Return result
                        End If

                        If utIndexListData.foldercode <> "" Then
                            ' Get Folder number
                            lFolderNumber = DDBExists(FOLDER, utIndexListData.foldercode)

                            If lFolderNumber = 0 Then
                                ' Failed to get Folder number
                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_GETFOLDER, sControlName)
                                Else
                                    ErrorLog("ProcessControlData", "Failed to get Folder number for code, " & utIndexListData.foldercode)
                                End If
                                result = False

                                ' Close the index file
                                If Not (CloseIndexListData()) Then
                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                End If
                                Return result
                            End If

                            If utControlData.emptyonly Then
                                If IsLevelEmpty(FOLDER, lFolderNumber) Then
                                    ' Delete from Folder down
                                    If Not (DeleteFolder(utIndexListData.cabinetcode, utIndexListData.drawercode, lFolderNumber)) Then
                                        ' Failed to delete folder data.
                                        ' It may well be because of permissions, so
                                        ' no error will be reported.
                                    End If

                                    If g_sAppType.Value <> "WAPI" Then
                                        ' Append a log message to todays log file
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder deleted, " & lFolderNumber
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                        sLogMess(4) = "Folder - " & utIndexListData.foldercode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                Else
                                    ' Empty only flag set to true, but level is not
                                    ' empty, we must log a message
                                    ' Append a log message to todays log file
                                    If g_sAppType.Value = "WAPI" Then

                                        ErrorWAPI(ERR_FULLFOLDER, sControlName)

                                    Else
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - Folder contains data, not deleted"
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                        sLogMess(4) = "Folder - " & utIndexListData.foldercode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                End If
                            Else
                                ' Delete from Folder down
                                If Not (DeleteFolder(utIndexListData.cabinetcode, utIndexListData.drawercode, lFolderNumber)) Then
                                    ' Failed to delete folder data.
                                    ' It may well be because of permissions, so
                                    ' no error will be reported.
                                End If

                                If g_sAppType.Value <> "WAPI" Then
                                    ' Append a log message to todays log file
                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                        ' Failed to log message
                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                        result = False

                                        ' Close the index file
                                        If Not (CloseIndexListData()) Then
                                            ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                        End If
                                        Return result
                                    End If

                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder deleted, " & lFolderNumber
                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                    sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                    sLogMess(4) = "Folder - " & utIndexListData.foldercode
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    CloseLogFile(g_sHistoryRoot)
                                End If
                            End If
                        ElseIf (utIndexListData.drawercode <> "") Then
                            ' Get Drawer number
                            lDrawerNumber = DDBExists(DRAWER, utIndexListData.drawercode)

                            If lDrawerNumber = 0 Then
                                ' Failed to get Drawer number
                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_GETDRAWER, sControlName)
                                Else
                                    ErrorLog("ProcessControlData", "Failed to get Drawer number for code, " & utIndexListData.drawercode)
                                End If
                                result = False

                                ' Close the index file
                                If Not (CloseIndexListData()) Then
                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                End If
                                Return result
                            End If

                            If utControlData.emptyonly Then
                                If IsLevelEmpty(DRAWER, lDrawerNumber) Then
                                    ' Delete from Drawer down
                                    If Not (DeleteDrawer(utIndexListData.cabinetcode, lDrawerNumber)) Then
                                        ' Failed to delete Drawer data.
                                        ' It may well be because of permissions, so
                                        ' no error will be reported.
                                    End If

                                    If g_sAppType.Value <> "WAPI" Then
                                        ' Append a log message to todays log file
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer deleted, " & lDrawerNumber
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                Else
                                    If g_sAppType.Value = "WAPI" Then

                                        ErrorWAPI(ERR_FULLDRAWER, sControlName)

                                    Else
                                        ' Empty only flag set to true, but level is not
                                        ' empty, we must log a message
                                        ' Append a log message to todays log file

                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - Drawer contains data, not deleted"
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                End If
                            Else
                                ' Delete from Drawer down
                                If Not (DeleteDrawer(utIndexListData.cabinetcode, lDrawerNumber)) Then
                                    ' Failed to delete Drawer data.
                                    ' It may well be because of permissions, so
                                    ' no error will be reported.
                                End If

                                If g_sAppType.Value <> "WAPI" Then
                                    ' Append a log message to todays log file
                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                        ' Failed to log message
                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                        result = False

                                        ' Close the index file
                                        If Not (CloseIndexListData()) Then
                                            ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                        End If
                                        Return result
                                    End If

                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer deleted, " & lDrawerNumber
                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                    sLogMess(3) = "Drawer - " & utIndexListData.drawercode
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    CloseLogFile(g_sHistoryRoot)
                                End If
                            End If
                        ElseIf (utIndexListData.cabinetcode <> "") Then
                            ' Get Cabinet number
                            lCabinetNumber = DDBExists(CABINET, utIndexListData.cabinetcode)

                            If lCabinetNumber = 0 Then
                                ' Failed to get Cabinet number
                                If g_sAppType.Value = "WAPI" Then
                                    ErrorWAPI(ERR_GETCABINET, sControlName)
                                Else
                                    ErrorLog("ProcessControlData", "Failed to get Cabinet number for code, " & utIndexListData.cabinetcode)
                                End If
                                result = False

                                ' Close the index file
                                If Not (CloseIndexListData()) Then
                                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                End If
                                Return result
                            End If

                            If utControlData.emptyonly Then
                                If IsLevelEmpty(CABINET, lCabinetNumber) Then
                                    ' Delete from Cabinet down
                                    If Not (DeleteCabinet(lCabinetNumber)) Then
                                        ' Failed to delete Cabinet data.
                                        ' It may well be because of permissions, so
                                        ' no error will be reported.
                                    End If

                                    If g_sAppType.Value <> "WAPI" Then
                                        ' Append a log message to todays log file
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet deleted, " & lCabinetNumber
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                Else
                                    ' Empty only flag set to true, but level is not
                                    ' empty, we must log a message
                                    ' Append a log message to todays log file

                                    If g_sAppType.Value <> "WAPI" Then
                                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                                            ' Failed to log message
                                            ErrorLog("ProcessControlData", "Failed to open log file")
                                            result = False

                                            ' Close the index file
                                            If Not (CloseIndexListData()) Then
                                                ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                            End If
                                            Return result
                                        End If

                                        sLogMess(1) = "DocuMaster API Daemon - Cabinet contains data, not deleted"
                                        sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                        iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                        CloseLogFile(g_sHistoryRoot)
                                    End If
                                End If
                            Else
                                ' Delete from Cabinet down
                                If Not (DeleteCabinet(lCabinetNumber)) Then
                                    ' Failed to delete Cabinet data.
                                    ' It may well be because of permissions, so
                                    ' no error will be reported.
                                End If

                                If g_sAppType.Value <> "WAPI" Then
                                    ' Append a log message to todays log file
                                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                                        ' Failed to log message
                                        ErrorLog("ProcessControlData", "Failed to open log file")
                                        result = False

                                        ' Close the index file
                                        If Not (CloseIndexListData()) Then
                                            ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                                        End If
                                        Return result
                                    End If

                                    sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet deleted, " & lCabinetNumber
                                    sLogMess(2) = "Cabinet - " & utIndexListData.cabinetcode
                                    iPMFunc.LogMessage(LLOG, "DMSAPI", sLogMess)
                                    CloseLogFile(g_sHistoryRoot)
                                End If
                            End If
                        End If

                        ' Get a line the the index file
                        iIndexFlag = GetIndexListData(utIndexListData)
                    End While

                    ' Close the index file
                    If Not (CloseIndexListData()) Then
                        ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                    End If

                    If iIndexFlag = PM_ERROR Then
                        ' A Error accurred getting the index data
                        ErrorLog("ProcessControlData", "Failed to get index list data from file, " & utControlData.filename)
                        Return False
                    End If

                    If g_sAppType.Value = "WAPI" Then
                        ErrorWAPI(ERR_SUCCESS, sControlName)
                    End If

                Case "LOG"
                    ' Task LOG
                    '
                    ' Append a log message to todays log file
                    If Not (OpenLogFile(g_sHistoryRoot)) Then
                        ' Failed to log message
                        ErrorLog("ProcessControlData", "Failed to open log file")
                        Return False
                    End If

                    sLogMess(1) = utControlData.message
                    iPMFunc.LogMessage(LLOG, utControlData.username, sLogMess)
                    CloseLogFile(g_sHistoryRoot)
            End Select

            Return result

        Catch



            If g_sAppType.Value = "WAPI" Then
                ErrorWAPI(ERR_UNDEFINED, sControlName)
            Else
                ErrorLog("ProcessControlData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            End If

            Return False
        End Try

    End Function

    Function ProcessJournalData(ByRef sJournalData() As String, ByRef iRetryErrors As Integer) As Integer

        Dim sControlName As String = ""

        Dim sLogMess(0) As String

        Dim ReturnOK As Integer = True
        Dim iProcessIcon As Integer = True

        Try

            ReDim sLogMess(1)

            ' Process all control files

            For iCntr As Integer = 1 To sJournalData.GetUpperBound(0)
                ' Set Forms Icon - salvo(110796)
                '        If g_sAppType <> "WAPI" Then
                '            If (iProcessIcon) Then
                '                frmDMSMain.Icon = frmDMSMain!imgProcess1.Picture
                '                iProcessIcon = False
                '            Else
                '                frmDMSMain.Icon = frmDMSMain!imgProcess2.Picture
                '                iProcessIcon = True
                '            End If
                '        End If
                Application.DoEvents()

                sControlName = g_sHistoryRoot & "tmp\" & sJournalData(iCntr) & ".ctl"

                ' Open Error log for control file
                If OpenErrorLog(sJournalData(iCntr)) Then
                    ' Update current journal control position
                    UpdateCurrentPos(sJournalData(iCntr))

                    ' Set Transaction processing on DDB
                    DAO_DBEngine_definst.BeginTrans()

                    If ProcessControlData(sControlName) Then
                        ' Issues a Commit on the DDB because all
                        ' appears to be OK.
                        g_iTmp = 0
                        g_iRC = CommitDatabase()
                        Select Case g_iRC
                            Case PM_TRUE
                            Case PM_FALSE
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Commit Failed.", MB_ICONEXCLAMATION, "ProcessJournalData")
                                ReturnOK = False
                            Case PM_CANCEL
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Commit Cancelled.", MB_ICONEXCLAMATION, "ProcessJournalData")
                                ReturnOK = False
                            Case Else
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Commit Failed. - " & g_iRC, MB_ICONEXCLAMATION, "ProcessJournalData")
                                ReturnOK = False
                        End Select

                        ' Delete current control journal position file
                        If Not (DeleteCurrentPos()) Then
                            Debug.WriteLine("Failed to delete journal position file")
                        End If

                        ' Delete control files
                        If Not (DeleteControlFiles(sJournalData(iCntr))) Then
                            ' Failed to delete control files
                            Debug.WriteLine("Failed to delete control files")
                        End If

                        ' Close Error log for control file
                        CloseErrorLog(sJournalData(iCntr))

                        ' Delete control log file, if one exists
                        DeleteErrorLog(sJournalData(iCntr))
                    Else
                        ' Issues a Rollback on the DDB because all
                        ' is not what its seems.
                        DAO_DBEngine_definst.Rollback()

                        ' Update the journal bad file
                        If Not (UpdateJournalBad(sJournalData(iCntr))) Then
                            ' Failed to write bad control file
                            Debug.WriteLine("Failed to write bad control file")
                        End If

                        ' Delete current control journal position file
                        If Not (DeleteCurrentPos()) Then
                            Debug.WriteLine("Failed to delete journal position file")
                        End If

                        ' Append a err message to todays log file
                        If Not (OpenLogFile(g_sHistoryRoot)) Then
                            ' Failed to log message
                            ErrorLog("ProcessControlData", "Failed to open log file")
                        End If

                        sLogMess(1) = "DocuMaster API Daemon - Error occurred in the control file, " & sJournalData(iCntr)
                        iPMFunc.LogMessage(LERR, "DMSAPI", sLogMess)

                        CloseLogFile(g_sHistoryRoot)

                        ReturnOK = False

                        ' Close Error log for control file
                        CloseErrorLog(sJournalData(iCntr))
                    End If
                Else
                    ' Failed to open error log
                    Debug.WriteLine("Failed to open error log file")
                End If

                Application.DoEvents()
            Next iCntr

            If Not (DeleteJournalFile(iRetryErrors)) Then
                ' Failed to delete journal.wrk/rty file
                Debug.WriteLine("Failed to delete the journal.wrk/rty file")
            End If

            Return ReturnOK

        Catch



            ErrorLog("ProcessJournalData", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function
	
	Sub ProcessMain(ByRef iRetryErrors As Integer)
		
		Dim sJournalData(0) As String
		Dim iOK As Integer
		
		
		' Open the Journal error log file
		If Not (OpenJournalErrorLog()) Then
			' Failed to open journal error file
			Debug.WriteLine("Failed to open Journal error log")
			Exit Sub
		End If
		
		
		' Check status of journal work files...
		Dim iStatus As Integer = CheckJournalFile()
		Select Case iStatus
			Case PM_RECOVER
				' Journal.wrk exists, we need to recover from a previous attempt
				If GetJournalData(sJournalData, False) Then 'True = retry ...
					If Not (ProcessJournalData(sJournalData, False)) Then
						' Failed
					End If
				End If
				
			Case PM_NONE
				'No .DMS or .WRK file exists, there's nothing to do !!!
				'unless we need to retry errors...
				If iRetryErrors Then
					' Check if journal.bad exist's
					If CheckJournalBadFile() Then
						' Journal.bad exists
						If RenameJournalBadFile() Then
							' Renamed journal.bad file to journal.rty,
							' do the biz
							'                If Not (OpenJournalBad()) Then
							'                    ' Failed to open journal error file
							'                    Debug.Print "Failed to open Journal error log"
							'                Else
							If GetJournalData(sJournalData, True) Then
								If Not (ProcessJournalData(sJournalData, True)) Then
									' Failed
								End If
							End If
							'                End If
						End If
					End If
				End If
				
			Case PM_ERRRETRY
				'we need to recover processing the .rty file
				If GetJournalData(sJournalData, True) Then 'True = retry ...
					If Not (ProcessJournalData(sJournalData, True)) Then
						' Failed
					End If
				End If
				
			Case PM_NORMAL
				'All is OK, we can process as normal
				
				iOK = True
				If iRetryErrors Then
					' Check if journal.bad exist's
					If CheckJournalBadFile() Then
						' Journal.bad exists
						If Not (RenameJournalBadFile()) Then
							' Failed to rename journal.bad file to journal.rty,
							' see you later
							iOK = False
						End If
						'            If Not (OpenJournalBad()) Then
						'                ' Failed to open journal error file
						'                Debug.Print "Failed to open Journal error log"
						'                iOK = False
						'            End If
					Else
						' No journal.bad exists
						iOK = False
					End If
					
				Else
					' Journal.wrk doesn't exist, we are
					' safe to continue
					If RenameJournalFile() Then
						If Not (CheckLockFile()) Then
							' Failed, something went wrong
							iOK = False
						End If
					End If
				End If
				
				If iOK Then
					' Open the Journal bad file
					If GetJournalData(sJournalData, iRetryErrors) Then
						If Not (ProcessJournalData(sJournalData, iRetryErrors)) Then
							' Failed
						End If
					End If
				End If
				
			Case PM_FALSE
				'OOOOOPS, somthing went wrong
		End Select
		
		' Close the Journal error log file
		CloseJournalErrorLog()
		
		' Close the Journal bad file
		'CloseJournalBad
		
	End Sub
	
	Function Rename(ByRef sFrom As String, ByRef sTo As String) As Integer
		
		Try 
			
			FileSystem.Rename(sFrom, sTo)
			
			Return PM_TRUE
		
		Catch 
			
			
			
			Return PM_FALSE
		End Try
		
	End Function
	
	Function RenameJournalBadFile() As Integer
		
		Dim result As Integer = 0
		result = True
		
		Try 
			
			' Rename journal.bad to journal.rty
			FileSystem.Rename(g_sHistoryRoot & "data\journal.bad", g_sHistoryRoot & "data\journal.rty")
			
			Return result
		
		Catch 
			
			
			
			JournalErrorLog("RenameJournalBadFile", g_sHistoryRoot & "data\journal.bad" & " to " & g_sHistoryRoot & "data\journal.rty. Failed on error " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
			Return False
		End Try
		
	End Function
	
	Function RenameJournalFile() As Integer
		
		Dim result As Integer = 0
		result = True
		
		Try 
			
			' Rename journal.dms to journal.wrk
			If Rename(g_sHistoryRoot & "data\journal.dms", g_sHistoryRoot & "data\journal.wrk") = PM_FALSE Then
				JournalErrorLog("RenameJournalFile", g_sHistoryRoot & "data\journal.dms" & " to " & g_sHistoryRoot & "data\journal.wrk" & "Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
				result = False
			End If
			
			Return result
		
		Catch 
			
			
			
			JournalErrorLog("RenameJournalFile", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
			Return False
		End Try
		
	End Function
	
	'Function StripSlashes(sString As String) As String
	'
	'    Dim iCntr As Integer
	'    Dim sTmpString As String
	'
	'    On Error GoTo ErrStripSlashes
	'
	'    sTmpString = ""
	'
	'    For iCntr = 1 To Len(sString)
	'        If (Mid$(sString, iCntr, 1) <> "\" And Mid$(sString, iCntr, 1) <> "/") Then
	'            sTmpString = sTmpString & Mid$(sString, iCntr, 1)
	'        End If
	'    Next iCntr
	'
	'    StripSlashes = sTmpString
	'
	'    Exit Function
	'
	'ErrStripSlashes:
	'
	'    StripSlashes = ""
	'    Exit Function
	'
	'End Function
	'
	Sub UpdateCurrentPos(ByRef sCurrentControl As String)
		
		Dim JournalPos As Integer
		Dim sJournalPos As String = ""
		
		Try 
			
			' Find free file number and open
			JournalPos = FileSystem.FreeFile()
			
			' Check if file already exists
			sJournalPos = FileSystem.Dir(g_sHistoryRoot & "data\journal.cur", ATTR_NORMAL)
			
			'    If (sJournalPos <> "") Then
			'        Open g_sHistoryRoot & "data\journal.cur" For Append As #JournalPos
			'    Else
			FileSystem.FileOpen(JournalPos, g_sHistoryRoot & "data\journal.cur", OpenMode.Output)
			'    End If
			
			' Write current journal position
			FileSystem.PrintLine(JournalPos, sCurrentControl)
			
			'Close journal file
			FileSystem.FileClose(JournalPos)
		
		Catch 
			
			
			
			JournalErrorLog("UpdateCurrentPos", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
			Exit Sub
		End Try
		
		
	End Sub
	
	Function UpdateJournalBad(ByRef sControlName As String) As Integer
		
		Dim result As Integer = 0
		Dim JournalPos As Integer
		Dim sJournalPos As String = ""
		
		result = True
		
		Try 
			
			' Report that there is a bad control file
			
			If OpenJournalBad() = PM_TRUE Then
				
				' Write current bad control file
				FileSystem.PrintLine(fhJournalBad, sControlName)
				
				CloseJournalBad()
			Else
				
				JournalErrorLog("UpdateJournalBad", "Failed to Open Journal.bad")
				result = False
				
			End If
			
			Return result
		
		Catch 
			
			
			
			JournalErrorLog("UpdateJournalBad", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
			Return False
		End Try
		
	End Function
End Module