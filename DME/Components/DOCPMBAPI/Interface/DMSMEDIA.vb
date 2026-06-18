Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Module DMSMEDIA
	
	Public g_ssDeviceTypes As DAO.Snapshot
	Public g_dsHotzone As DAO.Dynaset
	Public g_dsLogin As DAO.Dynaset
	
	Public g_lHotDevice As Integer
	
	Public g_lCurrentDevice As Integer
	
	Public Const DL_ERROR As Integer = 0
	Public Const DL_OK As Integer = 1
	Public Const DL_MOUNT As Integer = 2
	Public Const DL_WRITE As Integer = 3
	Public Const DL_LOGIN As Integer = 4
	Public Const DL_NOLOCK As Integer = 5
	
	Public Const MV_OK As Integer = -1 'Return "mount successful"
	Public Const MV_ERROR As Integer = 0 'Return "unknown error"
	Public Const MV_SYNCRO As Integer = 1 'Return "Synchronise devices failed"
	Public Const MV_UNLOCK As Integer = 2 'Return "remove device lock failed"
	Public Const MV_NOVOL As Integer = 3 'Return "no volume in drive"
	Public Const MV_CANCEL As Integer = 4 'Return "the mount operation was canceled half way thru"
	Public Const MV_INMOUNT As Integer = 5 'Return "volume is curently in mount by another user"
	Public Const MV_DEVICE As Integer = 6 'Return "volume already mounted in another device"
	Public Const MV_VOLUME As Integer = 7 'Return "device has a volume mounted in it already"
	Public Const MV_DEVICETABLE As Integer = 8 'Return "error reading device table"
	Public Const MV_NODEVICE As Integer = 9 'Return "the required device was not found in the database"
	Public Const MV_COMMIT As Integer = 10 'CommitTrans failed on mount
	Public Const MV_DEVUPDATE As Integer = 11 'Update device table failed
	Public Const MV_VOLUPDATE As Integer = 12 'Update volume table failed
	
	Public Const VK_CONTROL As Integer = &H11s
	Public Const VK_SHIFT As Integer = &H10s
	
	Public g_sActiveVolume As String = ""
	Public g_sOverflowVolume As String = ""
	Public g_sActiveStoragePath As String = ""
	
	Public g_iInDrag As Integer
	Public g_iMDownPosY As Integer
	Public g_iMDownPosX As Integer
	
	Function DismountVolume(ByRef sDrv As String) As Integer
		Dim result As Integer = 0
		Dim lDev As Integer
		Dim ssLogins As DAO.Snapshot
		Dim sVol As String = ""
		Dim dsDev As DAO.Dynaset
		
		Try 
			
			lDev = GetDeviceMapping(sDrv)
			If lDev = 0 Then
				Interaction.MsgBox("This device needs configuring", MB_ICONEXCLAMATION)
				Return result
			End If
			
			' Check here for this being the active volume
			sVol = GetVolumeName(sDrv)
			If sVol.Trim() = g_sActiveVolume.Trim() Then
				Interaction.MsgBox("This is the Active Volume. Make another Volume active before dismounting", MB_ICONEXCLAMATION)
				Return False
			End If
			
			Select Case SetDeviceLock(lDev)
				Case DL_OK
					
					ssLogins = g_dbDDB.CreateSnapshot("SELECT * FROM login WHERE device_num = " & lDev)
					DAO_DBEngine_definst.FreeLocks()
					
					Do While ssLogins.RecordCount > 0
						
						If Interaction.MsgBox("Device is being used", MB_ICONEXCLAMATION + MB_RETRYCANCEL, "DocuMaster Mount Error") = System.Windows.Forms.DialogResult.Cancel Then
							ssLogins.Close()
							ssLogins = Nothing
							Return False
						End If
						
						ssLogins = g_dbDDB.CreateSnapshot("SELECT * FROM login WHERE device_num = " & lDev)
						DAO_DBEngine_definst.FreeLocks()
					Loop 
					ssLogins.Close()
					ssLogins = Nothing
					
					' if we get here we have exclusive access to this device
					dsDev = g_dbDDB.CreateDynaset("SELECT mount_volume FROM device WHERE device_num = " & lDev)
					DAO_DBEngine_definst.FreeLocks()
					dsDev.LockEdits = False
					
					If dsDev.RecordCount <> 1 Then
						Interaction.MsgBox("ERROR: Reading device table, recs=" & dsDev.RecordCount, MB_ICONEXCLAMATION)
						result = False
						dsDev.Close()
						dsDev = Nothing
						Return result
					Else
						dsDev.Edit()
                        dsDev("mount_volume").Value = ""
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsDev)
						Select Case g_iRC
							Case PM_TRUE
								result = True
							Case PM_FALSE
								Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "DismountVolume")
								result = False
							Case PM_CANCEL
								Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "DismountVolume")
								result = False
							Case PM_DUPLICATEKEY
								Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "DismountVolume")
								result = False
							Case Else
								Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DismountVolume")
								result = False
						End Select
						
						dsDev.Close()
						dsDev = Nothing
						
						Select Case RemoveDeviceLock(lDev)
							Case DL_OK
								result = True
							Case DL_MOUNT
								Interaction.MsgBox("ERROR: Remove device lock, device_num=" & lDev, MB_ICONEXCLAMATION)
								result = False
							Case DL_ERROR
								Interaction.MsgBox("ERROR: Unknown error removing device lock, device_num=" & lDev, MB_ICONEXCLAMATION)
								result = False
						End Select
						
					End If
					
				Case DL_MOUNT
					Interaction.MsgBox("Can not dismount this device, another user is already mounting it", MB_ICONEXCLAMATION)
					result = False
				Case DL_ERROR
					Interaction.MsgBox("ERROR: Unknown error locking this device, device_num=" & lDev, MB_ICONEXCLAMATION)
					result = False
					
			End Select
			Return result
		
		Catch 
			
			
			
			Interaction.MsgBox("ERROR " & Information.Err().Number & " - " & Conversion.ErrorToString(), MB_ICONEXCLAMATION, "Dismount Volume")
			Return False
		End Try
		
	End Function
	
	Function DismountVolumes(ByRef sVolume As String) As Integer
		
		Dim result As Integer = 0
		Dim dsVol, dsDev As DAO.Dynaset
		
		Try 
			
			dsVol = g_dbDDB.CreateDynaset("SELECT * FROM volume WHERE volume_name = """ & sVolume & """")
			DAO_DBEngine_definst.FreeLocks()
			dsVol.LockEdits = False
			
			If dsVol.RecordCount = 0 Then
				result = MV_NOVOL
			Else
				
                dsDev = g_dbDDB.CreateDynaset("SELECT * FROM device WHERE device_num = " & dsVol("device_num").Value)
				DAO_DBEngine_definst.FreeLocks()
				dsDev.LockEdits = False
				
				If dsDev.RecordCount = 0 Then
					result = MV_NODEVICE
				Else
					
					DAO_DBEngine_definst.BeginTrans()
					
					dsVol.Edit()
                    dsVol("device_num").Value = 0
					Select Case UpdateDynaset(dsVol)
						Case PM_TRUE
							
							dsDev.Edit()
                            dsDev("mount_volume").Value = ""
							
							Select Case UpdateDynaset(dsDev)
								Case PM_TRUE
									Select Case CommitDatabase()
										Case PM_TRUE
											result = MV_OK
										Case Else
											result = MV_COMMIT
											DAO_DBEngine_definst.Rollback()
									End Select
									
								Case Else
									result = MV_DEVUPDATE
									DAO_DBEngine_definst.Rollback()
							End Select
							
						Case Else
							result = MV_VOLUPDATE
							DAO_DBEngine_definst.Rollback()
					End Select
				End If
			End If
			
			dsVol.Close()
			dsVol = Nothing
			
			dsDev.Close()
			dsDev = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return Information.Err().Number
		End Try
		
	End Function
	
	Function FillDeviceList() As Integer
		Dim sSQL As String = ""
		
		Try 
			
			sSQL = "SELECT * FROM devicetype ORDER BY device_type"
			g_ssDeviceTypes = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function GetActiveStoragePath() As Integer
		Dim result As Integer = 0
		Dim ssDev As DAO.Snapshot
		Dim sDrv, sRoot, sVol As String
		Dim iDataLen As Integer
		
		Try 
			

            If g_sActiveVolume.Trim() = g_dsHotzone("active_volume").Value.Trim() And g_sActiveStoragePath.Trim() <> "" And Not (Convert.IsDBNull(g_sActiveStoragePath) Or IsNothing(g_sActiveStoragePath)) Then
                g_sOverflowVolume = g_dsHotzone("overflow_volume").Value.Trim()
                result = True
            Else
                g_sActiveVolume = g_dsHotzone("active_volume").Value.Trim()
                g_sOverflowVolume = g_dsHotzone("overflow_volume").Value.Trim()

                ssDev = g_dbDDB.CreateSnapshot("SELECT device_num, dt_num, mount_volume FROM device WHERE mount_volume = '" & g_sActiveVolume & "'")
                DAO_DBEngine_definst.FreeLocks()

                If ssDev.RecordCount = 1 Then ' we've found the device its logicaly mounted on
                    sDrv = GetDriveMapping(ssDev("device_num").Value)

                    If sDrv = "" Then ' drive letter was not found
                        Interaction.MsgBox("ERROR: Device needs configuring (volume=" & g_sActiveVolume & ")", MB_ICONEXCLAMATION)
                        Return False
                    End If

                    ' Got the drive letter, lets check the volume name
                    sVol = GetVolumeName(sDrv)
                    sRoot = GetVolumeRoot(sDrv)

                    If sVol = g_sActiveVolume Then ' this is the right volume
                        g_sActiveStoragePath = sDrv & sRoot
                    Else
                        ' lets go searching the other drives for this volume

                    End If

                End If
            End If
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function GetActiveVolume() As String
		
		
		Dim result As String = String.Empty
		Dim ssVol As DAO.Snapshot = g_dbDDB.CreateSnapshot("SELECT * FROM hotzone")
		DAO_DBEngine_definst.FreeLocks()
		
		If ssVol.RecordCount = 1 Then
            result = ssVol("active_volume").Value
		Else
			result = "(No Default Volume)"
		End If
		
		ssVol.Close()
		ssVol = Nothing
		
		Return result
		
		
		
		Return "(ERROR)"
		
	End Function
	
	Function GetDeviceLocation(ByRef iDev As Integer) As String
		Dim result As String = String.Empty
		Dim ssDev, ssDT As DAO.Snapshot
		Dim sSQL As String = ""
		
		Try 
			
			ssDev = g_dbDDB.CreateSnapshot("SELECT location FROM device WHERE device_num = " & iDev.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDev.RecordCount = 0 Then
				result = "(No Device)"
			Else
                result = ssDev("location").Value.Trim()
			End If
			
			ssDev.Close()
			ssDev = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return "(Device Error)"
		End Try
		
	End Function
	
	Function GetDeviceMapping(ByRef sDrv As String) As Integer
		
		For iTmp As Integer = 1 To g_utDeviceMappings.GetUpperBound(0)
			If g_utDeviceMappings(iTmp).sDrive.Value.ToUpper() = sDrv.ToUpper() Then
				Return g_utDeviceMappings(iTmp).lDevice
			End If
		Next iTmp
		
		Return 0
		
	End Function
	
	Function GetDeviceType(ByRef iDevNum As Integer) As String
		Dim result As String = String.Empty
		Dim ssDev, ssDT As DAO.Snapshot
		Dim sSQL As String = ""
		
		Try 
			
			ssDev = g_dbDDB.CreateSnapshot("SELECT dt_num FROM device WHERE device_num = " & iDevNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If Not (ssDev.RecordCount = 1) Then
				result = ""
				
			Else
				ssDT = g_dbDDB.CreateSnapshot("SELECT device_type FROM devicetype WHERE dt_num = " & ssDev("dt_num").ToString())
				DAO_DBEngine_definst.FreeLocks()
				
				If Not (ssDT.RecordCount = 1) Then
					result = ""
				Else
                    result = ssDT("device_type").Value.Trim()
				End If
				
				ssDT.Close()
				ssDT = Nothing
				
			End If
			
			ssDev.Close()
			ssDev = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetDriveMapping(ByRef lDevice As Integer) As String
		
		For iTmp As Integer = 1 To g_utDeviceMappings.GetUpperBound(0)
			If g_utDeviceMappings(iTmp).lDevice = lDevice Then
				Return g_utDeviceMappings(iTmp).sDrive.Value
			End If
		Next iTmp
		
		Return ""
		
	End Function
	
	Function GetPageVolumeName(ByRef sPageName As String) As String
		
		Dim result As String = String.Empty
		Dim ssSnapShot As DAO.Snapshot
		Dim sSQLQuery As String = ""
		
		result = ""
		
		Try 
			
			sSQLQuery = "SELECT volume_name FROM page WHERE page_name = '" & sPageName.Substring(0, 15) & "'"
			
			ssSnapShot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount = 1 Then
                result = ssSnapShot("volume_name").Value.Trim()
			End If
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetVolumeDevice(ByRef sVol As String) As Integer
		Dim result As Integer = 0
		Dim ssVol As DAO.Snapshot
		Dim sSQL As String = ""
		
		Try 
			sSQL = "SELECT device_num FROM volume WHERE volume_name = """ & sVol & """"
			
			ssVol = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssVol.RecordCount <> 1 Then
				result = 0
			Else
                result = ssVol("device_num").Value
			End If
			
			ssVol.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function GetVolumeName(ByRef sDrv As String) As String
		
		Dim sVol As String = New String(" "c, 129)
		Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("Name")
		Dim iDataLen As Integer = 0
		Try 
			iDataLen = GetPrivateProfileString("Volume", tmpPtr, "", sVol, 128, sDrv & "\volume.dms")
		Finally 
			Marshal.FreeHGlobal(tmpPtr)
		End Try
		sVol = sVol.Substring(0, iDataLen)
		
		If iDataLen = 0 Then
			Return ""
		Else
			Return sVol
		End If
		
	End Function
	
	Function GetVolumePath(ByRef sVolName As String) As String
		Dim result As String = String.Empty
		Dim ssDev As DAO.Snapshot
		Dim sDrv, sRoot As String
		
		Try 
			
			ssDev = g_dbDDB.CreateSnapshot("SELECT device_num FROM device WHERE mount_volume = '" & sVolName & "'")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDev.RecordCount <> 1 Then
				result = ""
			Else
                sDrv = GetDriveMapping(ssDev("device_num").Value)
				If sDrv = "" Then
					'  Message frmDMSMain, "Please wait ... Configuring drives"
					
					ConfigDrives()
					If Not SyncroDevices() Then
						MessageBox.Show("Syncronise drives failed", Application.ProductName)
					End If
					' Message frmDMSMain, ""
					'MsgBox "Device needs configuring", MB_ICONEXCLAMATION
                    sDrv = GetDriveMapping(ssDev("device_num").Value)
					If sDrv = "" Then
						result = ""
					Else
						sRoot = GetVolumeRoot(sDrv)
						result = sDrv.Trim() & sRoot.Trim()
					End If
				Else
					sRoot = GetVolumeRoot(sDrv)
					result = sDrv.Trim() & sRoot.Trim()
				End If
			End If
			
			ssDev.Close()
			ssDev = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetVolumeRoot(ByRef sDrv As String) As String
		Dim sRoot As String = ""
		Dim iDataLen As Integer
		
		Try 
			
			sRoot = New String(" "c, 129)
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("Root")
			Try 
				iDataLen = GetPrivateProfileString("Volume", tmpPtr, "", sRoot, 128, sDrv & "\volume.dms")
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			sRoot = sRoot.Substring(0, iDataLen)
			
			
			If iDataLen = 0 Then
				Return ""
			Else
				If sRoot.EndsWith("\") Or sRoot.EndsWith("/") Then
					Mid(sRoot, sRoot.Length, 1) = " "
				End If
				
				Return sRoot.Trim()
			End If
		
		Catch 
		End Try
		
		
		
		Return ""
		
	End Function
	
	Function InitialiseVolume(ByRef sDrv As String) As Integer
		
		Dim result As Integer = 0
		Dim fVolname As frmVolumeName
		Dim utVolume As DMSDDB.g_utVolume = DMSDDB.g_utVolume.CreateInstance()
		Dim iRetry As Integer
		Dim lDev As Integer
		
		Try 
			
			fVolname = New frmVolumeName()
			
			fVolname.ShowDialog()
			
			lDev = GetDeviceMapping(sDrv)
			
			If lDev = 0 Then
				Return False
			End If
			
			If Not (fVolname.txtVolName.Text.Trim() = "") Then
				utVolume.volume_name.Value = fVolname.txtVolName.Text.Trim()
				utVolume.device_num = lDev
				utVolume.initial_date = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate
				utVolume.initial_user.Value = g_sUsername
                utVolume.rewriteable = fVolname.chk3rewrite.CheckState
				DAO_DBEngine_definst.BeginTrans()
				
				If Not SaveVolumeData(utVolume) Then
					Interaction.MsgBox("ERROR: Failed to save Volume Data", MB_ICONEXCLAMATION)
					fVolname.Close()
					Return False
				End If
				
				iRetry = True
				While iRetry
					Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi("Name")
					Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi(utVolume.volume_name.Value)
					Try 
						If WritePrivateProfileString("Volume", tmpPtr3, tmpPtr4, sDrv & "\volume.dms") = 0 Then
							utVolume.volume_name.Value = Marshal.PtrToStringAnsi(tmpPtr4, utVolume.volume_name.Value.Length)
							
							If Interaction.MsgBox("ERROR: Creating Volume.dms." & Strings.Chr(10).ToString() & "Check Volume is not write protected," & Strings.Chr(10).ToString() & "or that you have access rights", MB_ICONEXCLAMATION + MB_RETRYCANCEL) = System.Windows.Forms.DialogResult.Cancel Then
								DAO_DBEngine_definst.Rollback()
								fVolname.Close()
								Return False
							Else
								iRetry = True
							End If
							
						Else
							utVolume.volume_name.Value = Marshal.PtrToStringAnsi(tmpPtr4, utVolume.volume_name.Value.Length)
							
							iRetry = False
							
						End If
					Finally 
						Marshal.FreeHGlobal(tmpPtr3)
						Marshal.FreeHGlobal(tmpPtr4)
					End Try
				End While
				
				g_iTmp = 0
				g_iRC = CommitDatabase()
				Select Case g_iRC
					Case PM_TRUE
					Case PM_FALSE
						DAO_DBEngine_definst.Rollback()
						Interaction.MsgBox("Commit Failed.", MB_ICONEXCLAMATION, "InitialiseVolume")
					Case PM_CANCEL
						DAO_DBEngine_definst.Rollback()
						Interaction.MsgBox("Commit Cancelled.", MB_ICONEXCLAMATION, "InitialiseVolume")
					Case Else
						DAO_DBEngine_definst.Rollback()
						Interaction.MsgBox("Commit Failed. - " & g_iRC, MB_ICONEXCLAMATION, "InitialiseVolume")
				End Select
				
				result = True
				
			End If
			
			fVolname.Close()
			
			Return result
		
		Catch 
			
			
			
			result = False
			fVolname.Close()
			Return result
		End Try
		
	End Function
	
	Function IsVolumeMounted(ByRef sDrv As String) As Integer
		Dim result As Integer = 0
		Dim ssDev As DAO.Snapshot
		Dim sVol As String = ""
		
		Try 
			
			sVol = GetVolumeName(sDrv)
			
			If sVol = "" Then
				result = False
			Else
				ssDev = g_dbDDB.CreateSnapshot("SELECT device_num, mount_volume FROM device WHERE mount_volume = '" & sVol & "'")
				DAO_DBEngine_definst.FreeLocks()
				
				If ssDev.RecordCount = 0 Then
					result = False
				Else
                    result = ssDev("device_num").Value
				End If
				
				ssDev.Close()
				ssDev = Nothing
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'Function MakePath(sPath$) As Integer
	'Dim iPntr%
	'Dim sDrv$, sNextDir$
	'
	'    On Error GoTo ErrMakePath
	'
	'    sPath$ = Trim(sPath$)
	'
	'    If Mid$(sPath$, 2, 1) <> ":" Then
	'        MsgBox "ERROR: No drive specified '" & sPath$ & "'", MB_ICONEXCLAMATION, "Make Directory"
	'        MakePath = False
	'    ElseIf Len(sPath$) = 3 Then
	'        MakePath = True
	'    Else
	'
	'        ' we need to ignor MkDir errors to build the whole path
	'        On Error Resume Next
	'
	'        sDrv$ = Left$(sPath, 2)
	'        sNextDir$ = Left$(sPath, 3)
	'
	'        For iPntr = 4 To Len(sPath$)
	'            If Mid$(sPath$, iPntr%, 1) <> "\" Then
	'                sNextDir$ = sNextDir$ & Mid$(sPath$, iPntr%, 1)
	'            Else
	'                MkDir (sNextDir$)
	'                'check what the error was...
	'                Select Case Err
	'                    Case 0, 75
	'                    Case Else
	'                        'only display the error if this is an Alpha version
	'                        'If Right(UCase(VERSIONNUMBER), 1) = "A" Then
	'                         MsgBox "ERROR: " & Err & Error, MB_ICONSTOP, "MakePath Error"
	'                        'End If
	'                End Select
	'
	'                sNextDir$ = sNextDir$ & Mid$(sPath$, iPntr%, 1)
	'            End If
	'        Next iPntr%
	'        MakePath = True
	'    End If
	'
	'    'Set error checking back on to stop rest of app screwing up...
	'    On Error GoTo ErrMakePath
	'    Exit Function
	'
	'ErrMakePath:
	'
	'    MakePath = False
	'    Exit Function
	'
	'End Function
	
	Function MountVolume(ByRef sDrv As String) As Integer
		Dim result As Integer = 0
		Dim dsDev As DAO.Dynaset
		Dim lDev As Integer
		Dim sVol As String = ""
		Dim iMntOK, iMountExit As Integer
		Dim lTmp As Integer
		Dim sTmp As String = ""
		
		Try 
			
			lDev = GetDeviceMapping(sDrv)
			
			If lDev > 0 Then
				
				lTmp = IsVolumeMounted(sDrv)
				If lTmp <> 0 Then
					If sDrv.ToUpper() = GetDriveMapping(lTmp).ToUpper() Then
						Return MV_OK
					Else
						' Should we dismount and remount on new device?
						'                MsgBox "The Volume in drive " & sDrv$ & " is already mounted in drive " & GetDriveMapping(lTmp&)
						Return MV_DEVICE
					End If
				End If
				
				Select Case SetDeviceLock(lDev)
					Case DL_MOUNT
						'MsgBox "This device is currently in mount by another user", MB_ICONEXCLAMATION
						Return MV_INMOUNT
					Case DL_ERROR
						'MsgBox "ERROR: Unknown error mounting device, device_num=" & lDev&, MB_ICONEXCLAMATION
						Return MV_ERROR
						
					Case DL_OK
						
						Do 
							iMountExit = True ' initialise so that if all goes well we only run this bit once
							
							iMntOK = True
							
							sVol = GetVolumeName(sDrv)
							If sVol = "" Then
								
								Select Case Interaction.MsgBox("The Volume in drive " & sDrv & "has not been initialised" & Strings.Chr(10).ToString() & "Initialise this volume now", MB_ICONQUESTION + MB_YESNOCANCEL)
									Case System.Windows.Forms.DialogResult.Yes
										If Not InitialiseVolume(sDrv) Then
											Interaction.MsgBox("failed to Initialise Volume in drive " & sDrv, MB_ICONEXCLAMATION)
											iMntOK = False
										Else
											iMntOK = True
										End If
										
									Case System.Windows.Forms.DialogResult.No
										Interaction.MsgBox("This Volume Can not be mounted, select another volume", MB_ICONEXCLAMATION)
										iMntOK = False
										
									Case System.Windows.Forms.DialogResult.Cancel
										dsDev = g_dbDDB.CreateDynaset("SELECT * FROM devicelock WHERE device_num = " & lDev)
										DAO_DBEngine_definst.FreeLocks()
										dsDev.LockEdits = False
										
										If dsDev.RecordCount > 0 Then
											dsDev.Edit()
                                            dsDev("mount").Value = False ' set mount back to free device up
                                            dsDev("user_name").Value = ""
											
											g_iTmp = 0
											g_iRC = UpdateDynaset(dsDev)
											Select Case g_iRC
												Case PM_TRUE
												Case PM_FALSE
													Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "MountVolume")
												Case PM_CANCEL
													Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "MountVolume")
												Case PM_DUPLICATEKEY
													Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "MountVolume")
												Case Else
													Interaction.MsgBox("Update Failed. (ds) -" & g_iRC, MB_ICONEXCLAMATION, "MountVolume")
											End Select
										End If
										
										dsDev.Close()
										dsDev = Nothing
										
										Return MV_CANCEL
										
								End Select
							End If
							
							If iMntOK Then
								
								'If MsgBox("Mount '" & sVol$ & "' in drive " & sDrv$, MB_ICONQUESTION + MB_YESNO) = IDYES Then
								dsDev = g_dbDDB.CreateDynaset("SELECT mount_volume FROM device WHERE device_num = " & lDev)
								DAO_DBEngine_definst.FreeLocks()
								dsDev.LockEdits = False
								
								If dsDev.RecordCount = 1 Then ' 1 device sounds good
									dsDev.Edit()
                                    dsDev("mount_volume").Value = sVol
									
									g_iTmp = 0
									g_iRC = UpdateDynaset(dsDev)
									Select Case g_iRC
										Case PM_TRUE
										Case PM_FALSE
											Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "MountVolume")
											result = MV_ERROR
										Case PM_CANCEL
											Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "MountVolume")
											result = MV_ERROR
										Case PM_DUPLICATEKEY
											Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "MountVolume")
											result = MV_ERROR
										Case Else
											Interaction.MsgBox("Update Failed. (ds) -" & g_iRC, MB_ICONEXCLAMATION, "MountVolume")
											result = MV_ERROR
									End Select
								ElseIf dsDev.RecordCount > 1 Then  ' we should only have one of these
									'MsgBox "ERROR: Reading device table", MB_ICONEXCLAMATION
									result = MV_DEVICETABLE
								Else
									' if RecordCount = 0 then no device exists ?????
									'MsgBox "ERROR: No device was found, device_num = " & lDev&, MB_ICONEXCLAMATION
									result = MV_NODEVICE
								End If
								
								dsDev.Close()
								dsDev = Nothing
								'  End If
							Else
								If Interaction.MsgBox("Do you want to try another Volume in drive " & sDrv, MB_ICONQUESTION + MB_YESNO) = System.Windows.Forms.DialogResult.Yes Then
									If Interaction.MsgBox("Insert another Volume and press OK", MB_ICONINFORMATION + MB_OKCANCEL) = System.Windows.Forms.DialogResult.Cancel Then
										Return IDCANCEL
									Else
										' do it all over again
										iMountExit = False
									End If
								Else
									result = MV_CANCEL
								End If
								
							End If
							
						Loop Until iMountExit
						
				End Select
				
				Select Case RemoveDeviceLock(lDev)
					Case DL_OK
						result = MV_OK
					Case DL_MOUNT
						'MsgBox "ERROR: Dismount failed, device is mounted by another user", MB_ICONEXCLAMATION
						result = MV_UNLOCK
					Case DL_ERROR
						'MsgBox "ERROR: Unknown error dismounting device", MB_ICONEXCLAMATION
						result = MV_ERROR
				End Select
				
			Else
				result = MV_ERROR
				'        ConfigDrives
				'        If SyncroDevices() = False Then
				'            MountVolume = MV_SYNCRO
				'        End If
				'MsgBox "Device needs configuring", MB_ICONEXCLAMATION
			End If
		
		Catch 
		End Try
		
		
		
		
		Return MV_ERROR
		
	End Function
	
	Function MountVolumeDevice(ByRef sVolume As String, ByRef lDevNum As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsVol, dsDev As DAO.Dynaset
		
		Try 
			
			result = MV_ERROR
			
			dsVol = g_dbDDB.CreateDynaset("SELECT * FROM volume WHERE volume_name = """ & sVolume & """")
			DAO_DBEngine_definst.FreeLocks()
			dsVol.LockEdits = False
			
			dsDev = g_dbDDB.CreateDynaset("SELECT * FROM device WHERE device_num = " & lDevNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			dsDev.LockEdits = False
			
			If dsVol.RecordCount = 0 Then
				result = MV_NOVOL
			Else
				If dsDev.RecordCount = 0 Then
					result = MV_NODEVICE
				Else
					

                    If dsDev("mount_volume").Value.Trim() = "" Or Convert.IsDBNull(dsDev("mount_volume")) Or IsNothing(dsDev("mount_volume")) Then
                        DAO_DBEngine_definst.BeginTrans()

                        dsVol.Edit()
                        dsVol("device_num").Value = dsDev("device_num").Value
                        Select Case UpdateDynaset(dsVol)
                            Case PM_TRUE

                                dsDev.Edit()
                                dsDev("mount_volume").Value = dsVol("volume_name").Value
                                Select Case UpdateDynaset(dsDev)
                                    Case PM_TRUE

                                        Select Case CommitDatabase()
                                            Case PM_TRUE
                                                result = MV_OK
                                            Case Else
                                                result = MV_COMMIT
                                                DAO_DBEngine_definst.Rollback()
                                        End Select

                                    Case Else
                                        result = MV_DEVUPDATE
                                        DAO_DBEngine_definst.Rollback()
                                End Select

                            Case Else
                                result = MV_VOLUPDATE
                                DAO_DBEngine_definst.Rollback()
                        End Select
                    Else
                        'device already has a volume in it
                        result = MV_VOLUME
                    End If
				End If
			End If
			
			dsDev.Close()
			dsDev = Nothing
			
			dsVol.Close()
			dsVol = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return Information.Err().Number
		End Try
		
	End Function
	
	Function RemoveDeviceLock(ByRef lDevice As Integer) As Integer
		Dim result As Integer = 0
		Dim dsLock As DAO.Dynaset
		
		Try 
			
			dsLock = g_dbDDB.CreateDynaset("SELECT * FROM devicelock WHERE device_num = " & lDevice.ToString())
			DAO_DBEngine_definst.FreeLocks()
			dsLock.LockEdits = False
			
			If dsLock.RecordCount = 0 Then ' no records means no lock ...OK
				result = DL_OK
				
			ElseIf dsLock.RecordCount > 1 Then  ' more than 1 record is an error
				result = DL_ERROR
				
			Else
                If dsLock("user_name").Value.Trim() = g_sUsername.Trim() Then ' if the user owns this lock ...
                    dsLock.Edit()
                    dsLock("mount").Value = False ' ... then we can remove it
                    dsLock("user_name").Value = ""

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsLock)
                    Select Case g_iRC
                        Case PM_TRUE
                            result = DL_OK
                        Case PM_FALSE
                            Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "RemoveDeviceLock")
                            result = DL_ERROR
                        Case PM_CANCEL
                            Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "RemoveDeviceLock")
                            result = DL_ERROR
                        Case PM_DUPLICATEKEY
                            Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "RemoveDeviceLock")
                            result = DL_ERROR
                        Case Else
                            Interaction.MsgBox("Update Failed. (ds) -" & g_iRC, MB_ICONEXCLAMATION, "RemoveDeviceLock")
                            result = DL_ERROR
                    End Select


                Else
                    ' If the user dosn't own this lock then ...
                    result = DL_MOUNT ' return a mount message (some other buggers got it)

                End If
				
			End If
			
			dsLock.Close()
			dsLock = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SetDeviceLock(ByRef lDevice As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsLock As DAO.Dynaset
		
		Try 
			
			dsLock = g_dbDDB.CreateDynaset("SELECT * FROM devicelock WHERE device_num = " & lDevice.ToString())
			DAO_DBEngine_definst.FreeLocks()
			dsLock.LockEdits = False
			
			If dsLock.RecordCount = 0 Then
				' If no lock record, create one
				dsLock = g_dbDDB.CreateDynaset("devicelock")
				dsLock.AddNew()
				
                dsLock("device_num").Value = lDevice
                dsLock("mount").Value = True
                dsLock("user_name").Value = g_sUsername
				
				g_iTmp = 0
				g_iRC = UpdateDynaset(dsLock)
				Select Case g_iRC
					Case PM_TRUE
						result = DL_OK
					Case PM_FALSE
						Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "SetDeviceLock")
						result = DL_ERROR
					Case PM_CANCEL
						Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "SetDeviceLock")
						result = DL_ERROR
					Case PM_DUPLICATEKEY
						Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "SetDeviceLock")
						result = DL_ERROR
					Case Else
						Interaction.MsgBox("Update Failed. (ds) -" & g_iRC, MB_ICONEXCLAMATION, "SetDeviceLock")
						result = DL_ERROR
				End Select
				
			Else
				' check what lock is currently on, and reset accordingly
				
                If dsLock("mount").Value Then
                    result = DL_MOUNT
                Else
                    dsLock.Edit()
                    dsLock("mount").Value = True
                    dsLock("user_name").Value = g_sUsername

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsLock)
                    Select Case g_iRC
                        Case PM_TRUE
                            result = DL_OK
                        Case PM_FALSE
                            Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "SetDeviceLock")
                            result = DL_ERROR
                        Case PM_CANCEL
                            Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "SetDeviceLock")
                            result = DL_ERROR
                        Case PM_DUPLICATEKEY
                            Interaction.MsgBox("Update Failed. (ds) - Duplicate Key", MB_ICONEXCLAMATION, "SetDeviceLock")
                            result = DL_ERROR
                        Case Else
                            Interaction.MsgBox("Update Failed. (ds) -" & g_iRC, MB_ICONEXCLAMATION, "SetDeviceLock")
                            result = DL_ERROR
                    End Select

                End If
				
			End If
			
			dsLock.Close()
			dsLock = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return DL_ERROR
		End Try
		
	End Function
	
	Function SetHotZone(ByRef sVolume As String) As Integer
		
		
	End Function
	
	Function SyncroDevices() As Integer
		Dim ssDevices As DAO.Snapshot
		Dim sSQL, sDrv As String
		Dim iLen As Integer
		
		Try 
			
			sSQL = "SELECT device_num FROM device ORDER BY device_num"
			ssDevices = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			ReDim g_utDeviceMappings(0)
			
			While Not ssDevices.EOF
				'        sDrv$ = Space$(15)
				'        iLen% = GetPrivateProfileString("DeviceMappings", Format$(ssDevices(0)), "", sDrv$, 15, "pmdms.ini")
				'        sDrv$ = Left$(sDrv$, iLen%)
				
				'Debug.Print sDrv$; ssDevices(0)
				
				If GetIniFileVar("DeviceMappings", ssDevices(0).ToString(), sDrv, False) = PM_TRUE Then 'iLen% > 0 Then
					ReDim Preserve g_utDeviceMappings(g_utDeviceMappings.GetUpperBound(0) + 1)
					g_utDeviceMappings(g_utDeviceMappings.GetUpperBound(0)).sDrive.Value = sDrv.Substring(0, 2)
                    g_utDeviceMappings(g_utDeviceMappings.GetUpperBound(0)).lDevice = ssDevices(0).Value
				End If
				ssDevices.MoveNext()
			End While
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function VolumeExists(ByRef sVolName As String) As Integer
		Dim result As Integer = 0
		Dim ssVol As DAO.Snapshot
		
		Try 
			
			ssVol = g_dbDDB.CreateSnapshot("SELECT device_num FROM volume WHERE volume_name = '" & sVolName.Trim() & "'")
			DAO_DBEngine_definst.FreeLocks()
			
			ssVol.MoveLast()
			If ssVol.RecordCount <> 1 Then
				result = 0
			Else
                result = CInt(ssVol("device_num").Value.Trim())
			End If
			
			ssVol.Close()
			ssVol = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
End Module