Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Module DMSHIST
	
	Public Const ADDCABINET As Integer = 1
	Public Const DELCABINET As Integer = 2
	Public Const MODCABINET As Integer = 3
	Public Const ADDDRAWER As Integer = 4
	Public Const DELDRAWER As Integer = 5
	Public Const MODDRAWER As Integer = 6
	Public Const ADDFOLDER As Integer = 7
	Public Const DELFOLDER As Integer = 8
	Public Const MODFOLDER As Integer = 9
	Public Const ADDDOCUMENT As Integer = 10
	Public Const DELDOCUMENT As Integer = 11
	Public Const MODDOCUMENT As Integer = 12
	
    Public g_dbHistDDB As DAO.Database
	Public g_UpdateHDB As Integer
	
	' Record type containing DMS data for History file.
	' Record Length = 290
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDMSHistData
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public cabinetcode As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public cabinetname As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public drawercode As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public drawername As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public foldercode As FixedLengthString
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public foldername As FixedLengthString
		<VBFixedString(9),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=9)> _
		Public docref As FixedLengthString
		<VBFixedString(8),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=8)> _
		Public date_Renamed As FixedLengthString
		<VBFixedString(6),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=6)> _
		Public time As FixedLengthString
		<VBFixedString(1),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=1)> _
		Public eventtype As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public description As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public volume As FixedLengthString
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public pagefile As FixedLengthString
		<VBFixedString(3),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=3)> _
		Public doctype As FixedLengthString
		Dim hdError As Integer
		Dim create_date As Double
		<VBFixedString(3),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=3)> _
		Public filler As FixedLengthString
		Public Shared Function CreateInstance() As g_utDMSHistData
			Dim result As New g_utDMSHistData
			result.cabinetcode = New FixedLengthString(20)
			result.cabinetname = New FixedLengthString(30)
			result.drawercode = New FixedLengthString(20)
			result.drawername = New FixedLengthString(30)
			result.foldercode = New FixedLengthString(20)
			result.foldername = New FixedLengthString(70)
			result.docref = New FixedLengthString(9)
			result.time = New FixedLengthString(6)
			result.eventtype = New FixedLengthString(1)
			result.description = New FixedLengthString(30)
			result.volume = New FixedLengthString(20)
			result.pagefile = New FixedLengthString(10)
			result.doctype = New FixedLengthString(3)
			result.filler = New FixedLengthString(3)
			Return result
		End Function
	End Structure
	
	' Parameters passed to the DMS History DLL.
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utDMSHistParams
		Dim NewRec As g_utDMSHistData
		<VBFixedString(80),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=80)> _
		Public DMSDir As FixedLengthString
		<VBFixedString(4),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=4)> _
		Public ReturnCode As FixedLengthString
		<VBFixedString(2),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2)> _
		Public FileStatus As FixedLengthString
		Public Shared Function CreateInstance() As g_utDMSHistParams
			Dim result As New g_utDMSHistParams
			result.NewRec = g_utDMSHistData.CreateInstance()
			result.DMSDir = New FixedLengthString(80)
			result.ReturnCode = New FixedLengthString(4)
			result.FileStatus = New FixedLengthString(2)
			Return result
		End Function
	End Structure
	
	' DMS History API routines.

	Declare Sub NewCab Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine NEWCAB must be converted to 32-bit version.

	Declare Sub NewDrw Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine NEWDRW must be converted to 32-bit version.

	Declare Sub NewFld Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine NEWFLD must be converted to 32-bit version.

	Declare Sub NewDoc Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine NEWDOC must be converted to 32-bit version.
	

	Declare Sub DelCab Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine DELCAB must be converted to 32-bit version.

	Declare Sub DelDrw Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine DELDRW must be converted to 32-bit version.

	Declare Sub DelFld Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine DELFLD must be converted to 32-bit version.

	Declare Sub DelDoc Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine DELDOC must be converted to 32-bit version.
	

	Declare Sub ModCab Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine MODCAB must be converted to 32-bit version.

	Declare Sub ModDrw Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine MODDRW must be converted to 32-bit version.

	Declare Sub ModFld Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine MODFLD must be converted to 32-bit version.

	Declare Sub ModDoc Lib "dmshstry.dll" (ByRef A As g_utDMSHistParams)
	'TODO: WARNING Third-party DLL subroutine MODDOC must be converted to 32-bit version.
	
	' Microsoft Windows 3.1 API routines.
	'Declare Function GetModuleHandle% Lib "kernel" (ByVal slpModuleName$)
	Declare Function GetModuleHandle Lib "kernel32"  Alias "GetModuleHandleA"(ByVal lpModuleName As String) As Integer
	Declare Sub FreeLibrary Lib "kernel.dll" (ByVal ihLibModule As Short)
	'TODO: No Win32 API known for subroutine FREELIBRARY.  Convert to different API version.
	
	Function AddHistoryData(ByRef iLevel As Integer, ByRef lNode As Integer) As Integer
		
		Dim result As Integer = 0
		Dim utHist As g_utDMSHistData = g_utDMSHistData.CreateInstance()
		Dim lDraw, lFold, lCab As Integer
		Dim sS As String = ""
		
		Try 
			
			result = True
			
			Select Case iLevel
				Case CABINET
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(ADDCABINET, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case DRAWER
					lCab = GetParentNumber(DRAWER, lDraw)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(ADDDRAWER, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case FOLDER
					lDraw = GetParentNumber(FOLDER, lFold)
					lCab = GetParentNumber(DRAWER, lDraw)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lDraw)
					utHist.foldercode.Value = GetDDBCode(FOLDER, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Or utHist.foldercode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(ADDFOLDER, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case DOCUMENT
					lFold = GetParentNumber(DOCUMENT, lNode)
					lDraw = GetParentNumber(FOLDER, lFold)
					lCab = GetParentNumber(DRAWER, lDraw)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lDraw)
					utHist.foldercode.Value = GetDDBCode(FOLDER, lFold)
					utHist.docref.Value = "000000000"
					Mid(utHist.docref.Value, 10 - Conversion.Str(lNode).Trim().Length, Conversion.Str(lNode).Trim().Length) = Conversion.Str(lNode).Trim()
					utHist.date_Renamed.Value = DateTime.FromOADate(GetDocDate(lNode)).ToString("yyyyMMdd")
					utHist.time.Value = DateTime.FromOADate(GetDocDate(lNode)).ToString("HHMMss")
					utHist.description.Value = GetDDBName(DOCUMENT, lNode)
					
					utHist.doctype.Value = GetDocType(LinkedData(DOCUMENT, lNode))
					utHist.eventtype.Value = GetEventType(LinkedData(DOCUMENT, lNode))
					utHist.volume.Value = GetDocVolume(LinkedData(DOCUMENT, lNode))
					sS = GetDocPageFile(LinkedData(DOCUMENT, lNode))
					If sS.Length = 15 Then
						utHist.pagefile.Value = sS.Substring(1, Math.Min(sS.Length, 2)) & sS.Substring(4, Math.Min(sS.Length, 2)) & sS.Substring(7, Math.Min(sS.Length, 2)) & sS.Substring(10, Math.Min(sS.Length, 2)) & sS.Substring(13, Math.Min(sS.Length, 2))
					Else
						utHist.pagefile.Value = ""
					End If
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Or utHist.foldercode.Value.Trim() = "" Or utHist.docref.Value.Trim() = "0" Or utHist.volume.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(ADDDOCUMENT, g_sHistoryRoot, utHist)) Then
						result = False
					End If
			End Select
			
			Return result
		
		Catch 
			
			
			
			result = False
			Interaction.MsgBox("Add Failed - " & Information.Err().Number, MB_ICONEXCLAMATION, "AddHistoryData")
			Return result
		End Try
		
	End Function
	
	Sub CheckUpdateHDBFlag()
		
		Dim ssUpdateHDB As DAO.Snapshot
		
		Try 
			
			g_UpdateHDB = True
			ssUpdateHDB = g_dbHistDDB.CreateSnapshot("SELECT * FROM system")
			If ssUpdateHDB.RecordCount = 1 Then
                If Not ssUpdateHDB("UpdateHDB").Value Then
                    g_UpdateHDB = False
                    ssUpdateHDB.Close()
                    ssUpdateHDB = Nothing
                End If
			End If
		
		Catch 
			
			
			MessageBox.Show("Read HDB Failed - History Database will be updated", "Read HDB", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End Try
		
	End Sub
	
	Sub CloseHDB()
		
		Try 
			
			' Close Database
			Artinsoft.VB6.DB.TransactionManager.DeEnlist(g_dbHistDDB)
			g_dbHistDDB.Close()
		
		Catch 
		End Try
		
		
		Exit Sub
	End Sub
	
	Function GetNextHDBNumber() As Integer
		
		Dim ssNextNumber As DAO.Snapshot
		Dim sSQLQuery As String = ""
		Dim iNextNumber As Integer
		
		Try 
			
			'   salvo(130996) - Optimise this
			'   sSQLQuery = "SELECT hdb_num FROM historydata ORDER BY hdb_num DESC"
			sSQLQuery = "SELECT MAX(hdb_num) FROM historydata"
			
			ssNextNumber = g_dbHistDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			' Check if any records where found
			If ssNextNumber.RecordCount <> 0 Then
                iNextNumber = (CInt(ssNextNumber(0).Value + 1))
			Else
				iNextNumber = 1
			End If
			
			ssNextNumber.Close()
			ssNextNumber = Nothing
			
			Return iNextNumber
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function OpenHDB() As Integer
		
		Dim result As Integer = 0
		Dim iDataLen As Integer
		Dim sDBName, sDDB, sMsg As String
		Dim iPointerIN As Integer
		
		
		Try 
			
			result = True
			
			' Set Mouse Pointer To HourClass
            'iPointerIN = Cursor.Current
			Cursor.Current = Cursors.WaitCursor
			
			'g_sHistoryRoot = Space$(128)
			'iDataLen = GetPrivateProfileString("APIDaemon", "JournalRoot", "", g_sHistoryRoot, 128, "pmdms.ini")
			'g_sHistoryRoot = Left$(g_sHistoryRoot, iDataLen)
			
			If GetIniFileVar("APIDaemon", "JournalRoot", g_sHistoryRoot, False) = PM_FALSE Then
				'???
			End If
			
			sDDB = g_sDBRoot & "\dmshdb.mdb"

			g_dbHistDDB = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(sDDB)
			
			Select Case dbVersion(g_dbHistDDB)
				Case HDB_VERSION
					'OK...
				Case Is < HDB_VERSION
					Interaction.MsgBox("The DocuMaster Remote Database needs updating. Advise your DocuMaster administrator", MB_ICONSTOP, "HDB Database Version Error")
					Return False
				Case Is > HDB_VERSION
					Interaction.MsgBox("The DocuMaster Remote build application needs updating. Advise your DocuMaster administrator", MB_ICONSTOP, "Application Version Error")
					Return False
			End Select
			

            'Cursor.Current = iPointerIN
			Return result
		
		Catch 
			
			
			
			Select Case (Information.Err().Number)
				Case 3000
					sMsg = "History database is currently open with exclusive access"
					Interaction.MsgBox(sMsg, MB_ICONEXCLAMATION, "HDB Database Open Error")
					
				Case 3024 'can not find database
					sMsg = "Can not open database " & g_sDBRoot & "\dmshdb.mdb" & Strings.Chr(10).ToString() & "Check path is available or Database name is correct in pmdms.ini"
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "HDB Database Open Error")
					
				Case 3049
					sMsg = "Database is damaged. Contact your DocuMaster System Administrator"
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "HDB Database Open Error")
					
				Case Else
					sMsg = "Error " & Information.Err().Number & ": " & Conversion.ErrorToString()
					Interaction.MsgBox(sMsg, MB_ICONEXCLAMATION, "HDB Database Open Error")
			End Select
			
			result = False
			

            'Cursor.Current = iPointerIN
			Return result
			
			
			
			result = False
			Interaction.MsgBox("ERROR: History database repair failed. Log ALL users off and try again", MB_ICONSTOP)
			Return result
		End Try
		
	End Function
	
	Function UpdateHDB(ByRef iTask As Object, ByRef sDMSDir As String, ByRef utNewRecord As g_utDMSHistData) As Integer
		
		Dim result As Integer = 0
		Dim tbDMSHistData As DAO.Table
		Dim lNextHistNumber As Integer
		Dim sLogMess(5) As String
		Dim sTaskDesc As String = ""
		
		result = True
		
		'Salvo (170796) - If we are not updating the HDB, just leave with function true
		If Not g_UpdateHDB Then
			Return result
		End If
		
		Try 
			
			tbDMSHistData = g_dbHistDDB.OpenTable("historydata")
			tbDMSHistData.LockEdits = False
			tbDMSHistData.AddNew()
			
			'    tbDMSHistData("hdb_num") = 0

            tbDMSHistData("task").Value = iTask
            tbDMSHistData("dmsdir").Value = sDMSDir
            tbDMSHistData("cabinetcode").Value = utNewRecord.cabinetcode.Value
            tbDMSHistData("cabinetname").Value = utNewRecord.cabinetname.Value
            tbDMSHistData("drawercode").Value = utNewRecord.drawercode.Value
            tbDMSHistData("drawername").Value = utNewRecord.drawername.Value
            tbDMSHistData("foldercode").Value = utNewRecord.foldercode.Value
            tbDMSHistData("foldername").Value = utNewRecord.foldername.Value
            tbDMSHistData("docref").Value = utNewRecord.docref.Value
            tbDMSHistData("date").Value = utNewRecord.date_Renamed.Value
            tbDMSHistData("time").Value = utNewRecord.time.Value
            tbDMSHistData("eventtype").Value = utNewRecord.eventtype.Value
            tbDMSHistData("description").Value = utNewRecord.description.Value
            tbDMSHistData("volume").Value = utNewRecord.volume.Value
			If utNewRecord.pagefile.Value.Trim() = "" Then
                tbDMSHistData("pagefile").Value = New String(" "c, 10)
			Else
                tbDMSHistData("pagefile").Value = utNewRecord.pagefile.Value
			End If
            tbDMSHistData("doctype").Value = utNewRecord.doctype.Value
            tbDMSHistData("filler").Value = utNewRecord.filler.Value
            tbDMSHistData("hderror").Value = utNewRecord.hdError
            tbDMSHistData("create_date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())
			
			For iCntr As Integer = 1 To UPDATERETRY
				'        lNextHistNumber = GetNextHDBNumber()
				'        If (lNextHistNumber = 0) Then
				'            ' Failed to get next number
				'            UpdateHDB = False
				'            tbDMSHistData.Close
				'            Set tbDMSHistData = Nothing
				'            Exit Function
				'        End If
				
				'        tbDMSHistData("hdb_num") = lNextHistNumber
				
				g_iTmp = 0
				g_iRC = UpdateTable(tbDMSHistData)
				Select Case g_iRC
					Case PM_TRUE
						result = True
						Exit For
					Case PM_FALSE
						Interaction.MsgBox("Update Failed. (tbl)", MB_ICONEXCLAMATION, "UpdateHDB")
						result = False
						Exit For
					Case PM_CANCEL
						Interaction.MsgBox("Update Cancelled. (tbl)", MB_ICONEXCLAMATION, "UpdateHDB")
						result = False
						Exit For
					Case PM_DUPLICATEKEY
					Case PM_DATACHANGED
						Interaction.MsgBox("Data has changed. (tbl)", MB_ICONEXCLAMATION, "UpdateHDB")
						result = False
						Exit For
					Case Else
						Interaction.MsgBox("Update Failed. (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "UpdateHDB")
						result = False
						Exit For
				End Select
			Next iCntr
			
			tbDMSHistData.Close()
			tbDMSHistData = Nothing
			
			If g_iRC = PM_DUPLICATEKEY Then
				Interaction.MsgBox("Update Failed. (tbl) - Failed to get unique number", MB_ICONEXCLAMATION, "UpdateHDB")
				result = False
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
End Module