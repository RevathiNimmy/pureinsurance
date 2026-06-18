Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class FrmDMETransfer
	Inherits System.Windows.Forms.Form
	 ' ***************************************************************** '
	 ' Module Name: FrmDMETransfer
	 '
	 ' Date:  03/12/97
	 '
	 ' Description: DME Transfer Routine.
	 '
	 ' Edit History:
	 ' ***************************************************************** '
	
	Public oDBSirius As dPMDAO.Database
	Public oDBDME As dPMDAO.Database
	Public g_sUsername As String = ""
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	Public g_sCallingAppName As String = ""
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	Public g_oDatabase As Object
	
	Private Sub CmdRun_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdRun.Click
		
        Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
		
		Try 
			
			 ' Disable Run button
			CmdRun.Enabled = False
			
			 ' Open the Sirius database
			oDBSirius = New dPMDAO.Database()
			
            lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iUserID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDBSirius)
			
			 ' Open the DocuMaster database
			oDBDME = New dPMDAO.Database()
			
            lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iUserID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=oDBDME)
			
			 ' Add Index to speed up merge
			sSQL = "IF NOT EXISTS (SELECT * FROM sysindexes where name = 'XIE3folder')" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "BEGIN" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "CREATE NONCLUSTERED INDEX XIE3folder ON DOC_folder(folder_name)" & Strings.Chr(13) & Strings.Chr(10)
			sSQL = sSQL & "END"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLAction(sSQL, "Adding Index in DME", False), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				MessageBox.Show("Failed To Add Index in DME", Application.ProductName)
				
			End If
			
			lReturn = TransferCompany()
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed in TransferCompany", Application.ProductName)
			Else
				lReturn = MergeCompany()
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("Failed in Merge Company", Application.ProductName)
				End If
			End If
			
			If ChkClient.CheckState = CheckState.Checked Then
				
				lReturn = TransferClient()
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					ChkClient.Text = "Failed in TransferClient"
					ChkClient.Enabled = False
				End If
				
				lReturn = MergeClient()
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					ChkClient.Text = "Update Clients Failed"
					ChkClient.Enabled = False
				End If
				
			Else
				ChkClient.Enabled = False
			End If
			
			If ChkPolicies.CheckState = CheckState.Checked Then
				
				lReturn = TransferPolicy()
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					ChkPolicies.Text = "Failed in TransferPolicy"
					ChkPolicies.Enabled = False
				End If
				
				lReturn = MergeDescriptions()
				
				If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
					ChkPolicies.Text = "Merged Descriptions Failed"
					ChkPolicies.Enabled = False
				End If
				
			Else
				ChkPolicies.Enabled = False
			End If
			
			If ChkClaim.CheckState = CheckState.Checked Then
				
				lReturn = TransferClaim()
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					ChkClaim.Text = "Failed in TransferClaim"
					ChkClaim.Enabled = False
				End If
				
				lReturn = MergePolicy()
				
				If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
					ChkClaim.Text = "Update Policies Failed"
					ChkClaim.Enabled = False
				End If
				
			Else
				ChkClaim.Enabled = False
			End If
			
			Label1.Text = "Run Finished"
			Me.Refresh()
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in Run Click: " & Information.Err().Number & ":" & excep.Message, "RunClick", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Environment.Exit(0)
			
		End Try
		
	End Sub
	
	Private Function TransferCompany() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
		Dim lNoOfRecords, lCount As Integer
		Dim sExCode As String = ""
		Dim lFile As Integer
		Dim sFileName, sData As String
		Dim lMaxSourceId As Integer
		Dim sCompanyName As String = ""
		
		Dim sNewline As String = ""
		Dim oAPI As bDOCAPI.API
		Dim vIndexArray As Object
		Dim lCompanyExCode As Integer
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Label1.Text = "Loading Company Records ..."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			 ' Create log file
			sFileName = "C:\DMETransCompany.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 ' Get max number of entries in source table
			sSQL = "SELECT MAX(source_id) FROM source"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Source Count", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Max Source Count.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxSourceId = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxSourceId = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxSourceId
			End If
			
			 ' Build up SQL to get details from source table
			sSQL = "SELECT source_id, description"
			sSQL = sSQL & " FROM source"
			sSQL = sSQL & " ORDER BY source_id"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Company Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Company Details.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			
			 ' If nothing returned then transfer successful
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Me.Label1.Text = "Updating Company Records ..."
			Me.Refresh()
			
			Do While lCount <= lNoOfRecords
				
				 ' Display current Client

				ProgressBar1.Value = CInt(vArray(0, lCount))
				

				sExCode = CStr(vArray(0, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(1, lCount)).Trim(), sCompanyName), gPMConstants.PMEReturnCode)
				
				 ' Check to see if current company exists in DME
				sSQL = "SELECT * FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name = '" & sCompanyName & "'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If Not Information.IsArray(vArray2) Then
					
					sData = sExCode & "|" & sCompanyName
					FileSystem.PrintLine(lFile, sData)
					
				Else
					
					 ' UPDATE DOC_folder with new ex_code
					sSQL = "UPDATE DOC_folder SET ex_code = '" & sExCode & "'"
					sSQL = sSQL & " WHERE folder_name = '" & sCompanyName & "'"
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Company Folder Details", False), gPMConstants.PMEReturnCode)
					
					 ' If SQL fails
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						
						 ' Display error message

						MessageBox.Show("Failed To Update Company Folder Details:" & Strings.Chr(13) & Strings.Chr(10) &  _
						                "Folder - " & CStr(vArray(1, lCount)), Application.ProductName)
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			Me.Label1.Text = "Adding Company Records ..."
			Me.Refresh()
			
			 'Read in any enties that have been logged
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Input)
			
			oAPI = New bDOCAPI.API()
			
			 'Check for errors
			If oAPI Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			lReturn = oAPI.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=oDBDME)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			Do Until FileSystem.EOF(lFile)
				
				sNewline = FileSystem.LineInput(lFile)
				
				If sNewline.Substring(0, 10) <> "Time Taken" Then
					
					lReturn = CType(ParseCommandLine3(sNewline, lCompanyExCode, sCompanyName), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Failed to parse command line : " & sNewline, Application.ProductName)
                        Return gPMConstants.PMEReturnCode.PMFalse
						 'Close error log
						FileSystem.FileClose(lFile)
						Return result
					End If
					
					 'Build Index Array
					ReDim vIndexArray(1, 1)
					

					vIndexArray(0, 0) = lCompanyExCode

					vIndexArray(1, 0) = sCompanyName
					

					lReturn = oAPI.AddIndex(vIndexArray)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Failed to Add Company " & sCompanyName & " to DME", Application.ProductName)
					End If
				End If
			Loop 
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			oAPI = Nothing
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in TransferCompany: " & Information.Err().Number & ":" & excep.Message, "TransferCompany", MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMTrue
			 'Close error log
			FileSystem.FileClose(lFile)
			Return result
			
		End Try
	End Function
	
	Private Function TransferClient() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
        Dim vArray3(,) As Object
		Dim lNoOfRecords, lCount As Integer
		Dim sExCode, sClientCode As String
        Dim lMaxPartyCnt, lFile As Integer
		Dim sFileName, sData As String
		
		Dim sNewline As String = ""
		Dim oAPI As bDOCAPI.API
		Dim vIndexArray As Object
		Dim lCompanyExCode, lPartyExCode As Integer
		Dim sFolderName, sCompanyName As String
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			Me.Label1.Text = "Loading Client Records ..."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			 ' Create log file
			sFileName = "C:\DMETransClient.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 ' Get max number of entries in Party table
			sSQL = "SELECT MAX(party_cnt) FROM party"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Party Count", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Max Party Count.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxPartyCnt = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxPartyCnt = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxPartyCnt
			End If
			
			 ' Build up SQL to get details from Party table
			sSQL = "SELECT party_cnt, shortname, resolved_name"
			sSQL = sSQL & " FROM Party"
			sSQL = sSQL & " ORDER BY party_cnt"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Party Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Party Details.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			
			 ' If nothing returned then transfer successful
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Me.Label1.Text = "Updating Client Records ..."
			Me.Refresh()
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				
				 ' Display current Client

				ProgressBar1.Value = CInt(vArray(0, lCount))
				

				sExCode = CStr(vArray(0, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(1, lCount)).Trim(), sClientCode), gPMConstants.PMEReturnCode)
				
				 ' ICheck to see if current client exists in DME
				sSQL = "SELECT * FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name = '" & sClientCode & "'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray2) Then
					
					sSQL = "SELECT source_id, party_cnt FROM party"
					sSQL = sSQL & " WHERE shortname = '" & sClientCode & "'"
					
					 ' Fire SQL
					lReturn = CType(oDBSirius.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray3), gPMConstants.PMEReturnCode)
					
					 ' Log name of missing client
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray3) Then
						sData = sClientCode
					Else


						sData = CStr(vArray3(0, 0)) & "|" & CStr(vArray3(1, 0)) & "||" & sClientCode
					End If
					
					FileSystem.PrintLine(lFile, sData)
					
				End If
				
				 ' UPDATE DOC_folder with new ex_code
				sSQL = "UPDATE DOC_folder SET ex_code = '" & sExCode & "'"
				sSQL = sSQL & " WHERE folder_name = '" & sClientCode & "'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
				
				 ' If SQL fails
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					 ' Display error message

					MessageBox.Show("Failed To UPDATE DOC_folder Details:" & Strings.Chr(13) & Strings.Chr(10) &  _
					                "Folder - " & CStr(vArray(2, lCount)), Application.ProductName)
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Me.Label1.Text = "Adding Client Records ..."
			Me.Refresh()
			
			 'Read in any enties that have been logged
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Input)
			
			oAPI = New bDOCAPI.API()
			
			 'Check for errors
			If oAPI Is Nothing Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			lReturn = oAPI.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=oDBDME)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			Do Until FileSystem.EOF(lFile)
				
				sNewline = FileSystem.LineInput(lFile)
				
				If sNewline.Substring(0, 10) <> "Time Taken" Then
					
					lReturn = CType(ParseCommandLine(sNewline, lCompanyExCode, lPartyExCode, sExCode, sFolderName), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Failed to parse command line : " & sNewline, Application.ProductName)
						result = gPMConstants.PMEReturnCode.PMFalse
						 'Close error log
						FileSystem.FileClose(lFile)
						Return result
					End If
					
					 ' Get Company description
					sSQL = "SELECT folder_name FROM DOC_folder"
					sSQL = sSQL & " WHERE ex_code = '" & CStr(lCompanyExCode) & "'"
					sSQL = sSQL & " AND folder_level = 0"
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Company Name", False, -1, vArray), gPMConstants.PMEReturnCode)
					
					 ' If SQL fails
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
						
						 ' Display error message
						MessageBox.Show("Failed To Retrieve Company Name For " & sFolderName, Application.ProductName)
					Else
						

						sCompanyName = CStr(vArray(0, 0))
						
						 'Build Index Array
						ReDim vIndexArray(1, 1)
						

						vIndexArray(0, 0) = lCompanyExCode

						vIndexArray(0, 1) = lPartyExCode

						vIndexArray(1, 0) = sCompanyName

						vIndexArray(1, 1) = sFolderName
						

						lReturn = oAPI.AddIndex(vIndexArray)
						
						If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							MessageBox.Show("Failed to Add Party " & sFolderName & " to DME", Application.ProductName)
						End If
					End If
				End If
			Loop 
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			oAPI = Nothing
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in TransferClient: " & Information.Err().Number & ":" & excep.Message, "TransferClient", MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMTrue
			 'Close error log
			FileSystem.FileClose(lFile)
			Return result
			
		End Try
	End Function
	
	Private Function TransferPolicy() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray As Object
        Dim vArray2(,) As Object
        Dim vArray3(,) As Object
		Dim lNoOfRecords, lCount As Integer
		Dim sExCode, sClientCode As String
		Dim lMaxPolicyCnt, lFile As Integer
		Dim sFileName, sData As String
		
		Dim sNewline As String = ""
		Dim oAPI As bDOCAPI.API
		Dim vIndexArray As Object
		Dim lCompanyExCode, lPartyExCode As Integer
		Dim sPartyName, sFolderName, sCompanyName As String
		Dim lCompanyFolderNum As Integer
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			Me.Label1.Text = "Loading Policy Records ..."
			Me.Refresh()
			
			 'Create log file
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMETransPolicy.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 ' Get max number of policies
			sSQL = "SELECT MAX(insurance_folder_cnt) FROM insurance_folder"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Insurance Folder Count", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Max Insurance Folder Count.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxPolicyCnt = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxPolicyCnt = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxPolicyCnt
			End If
			
			 ' Build up SQL to get details from insurance_file
			sSQL = "SELECT o.insurance_folder_cnt, i.insurance_ref"
			sSQL = sSQL & " FROM insurance_folder o, insurance_file i"
			sSQL = sSQL & " WHERE i.insurance_folder_cnt = o.insurance_folder_cnt"
			sSQL = sSQL & " ORDER BY o.insurance_folder_cnt"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Policy Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Policy Details.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			
			 ' If nothing returned then transfer successful
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Me.Label1.Text = "Updating Policy Records ..."
			Me.Refresh()
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				
				 ' Display current policy

				ProgressBar1.Value = CInt(vArray(0, lCount))
				

				sExCode = CStr(vArray(0, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(1, lCount)).Trim(), sClientCode), gPMConstants.PMEReturnCode)
				
				 ' Check if current policy exists in DME
				sSQL = "SELECT * FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name like '" & sClientCode & "   %'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray2) Then
					
					sSQL = "SELECT i.source_id, i.insured_cnt, o.insurance_folder_cnt  FROM insurance_file i, insurance_folder o"
					sSQL = sSQL & " WHERE i.insurance_ref = '" & sClientCode & "'"
					sSQL = sSQL & " AND i.insurance_folder_cnt = o.insurance_folder_cnt"
					
					 ' Fire SQL
					lReturn = CType(oDBSirius.SQLSelect(sSQL, "Checking Policy Details", False, -1, vArray3), gPMConstants.PMEReturnCode)
					
					 ' Log name of missing client
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray3) Then
						sData = sClientCode
					Else



						sData = CStr(vArray3(0, 0)) & "|" & CStr(vArray3(1, 0)) & "|" & CStr(vArray3(2, 0)) & "|" & sClientCode
					End If
					
					FileSystem.PrintLine(lFile, sData)
					
				End If
				
				sSQL = "UPDATE DOC_folder SET ex_code = '" & sExCode & "'"
				sSQL = sSQL & " WHERE folder_name like '" & sClientCode & "   %'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
				
				 ' If SQL fails
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					 ' Display error message

					MessageBox.Show("Failed To UPDATE DOC_folder Details:" & Strings.Chr(13) & Strings.Chr(10) &  _
					                "Folder - " & CStr(vArray(1, lCount)), Application.ProductName)
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Me.Label1.Text = "Adding Policy Records ..."
			Me.Refresh()
			
			 'Read in any enties that have been logged
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Input)
			
			oAPI = New bDOCAPI.API()
			
			 'Check for errors
			If oAPI Is Nothing Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			lReturn = oAPI.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=oDBDME)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			Do Until FileSystem.EOF(lFile)
				
				sNewline = FileSystem.LineInput(lFile)
				
				If sNewline.Substring(0, 10) <> "Time Taken" Then
					
					lReturn = CType(ParseCommandLine(sNewline, lCompanyExCode, lPartyExCode, sExCode, sFolderName), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Failed to Parse Command Line : " & sNewline, Application.ProductName)
						result = gPMConstants.PMEReturnCode.PMFalse
						 'Close error log
						FileSystem.FileClose(lFile)
						Return result
					End If
					
					 ' Get Company description
					sSQL = "SELECT folder_name, folder_num FROM DOC_folder"
					sSQL = sSQL & " WHERE ex_code = '" & CStr(lCompanyExCode) & "'"
					sSQL = sSQL & " AND folder_level = 0"
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Company Name", False, -1, vArray), gPMConstants.PMEReturnCode)
					
					 ' If SQL fails
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
						
						 ' Display error message
						MessageBox.Show("Failed To Retrieve Company Name For " & sFolderName, Application.ProductName)
					Else
						

						sCompanyName = CStr(vArray(0, 0))

						lCompanyFolderNum = CInt(vArray(1, 0))
						
						 ' Get party description
						sSQL = "SELECT shortname FROM party"
						sSQL = sSQL & " WHERE party_cnt = " & CStr(lPartyExCode)
						
						 ' Fire SQL
						lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Party Name", False, -1, vArray), gPMConstants.PMEReturnCode)
						
						 ' If SQL fails
						If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
							
							 ' Display error message
							MessageBox.Show("Failed To Retrieve Party Name For " & sFolderName, Application.ProductName)
						Else
							

							sPartyName = CStr(vArray(0, 0))
							
							 ' Get policy description
							sSQL = "SELECT s.last_trans_description FROM insurance_file i, insurance_file_system s"
							sSQL = sSQL & " WHERE i.insurance_file_cnt = " & sExCode
							sSQL = sSQL & " AND i.insurance_file_cnt = s.insurance_file_cnt"
							
							 ' Fire SQL
							lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Policy Description", False, -1, vArray), gPMConstants.PMEReturnCode)
							
							 ' If SQL fails
							If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
								

								If CStr(vArray) = "" Then
									
									 'Build Index Array
									ReDim vIndexArray(1, 2)
									

									vIndexArray(0, 0) = lCompanyExCode

									vIndexArray(0, 1) = lPartyExCode

									vIndexArray(0, 2) = sExCode
									

									vIndexArray(1, 0) = sCompanyName

									vIndexArray(1, 1) = sPartyName

									vIndexArray(1, 2) = sFolderName
									

									lReturn = oAPI.AddIndex(vIndexArray)
									
									If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
										MessageBox.Show("Failed to Add Policy " & sFolderName & " to DME", Application.ProductName)
									End If
									
								Else
									 ' Display error message
									MessageBox.Show("Failed To Retrieve Policy Description For " & sFolderName, Application.ProductName)
								End If
							Else
								

								sFolderName = sFolderName & "   " & CStr(vArray(0, 0)).Trim()
								
								 'Build Index Array
								ReDim vIndexArray(1, 2)
								

								vIndexArray(0, 0) = lCompanyExCode

								vIndexArray(0, 1) = lPartyExCode

								vIndexArray(0, 2) = sExCode
								

								vIndexArray(1, 0) = sCompanyName

								vIndexArray(1, 1) = sPartyName

                                vIndexArray(1, 2) = sFolderName.Substring(0, 70)
								

								lReturn = oAPI.AddIndex(vIndexArray)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									MessageBox.Show("Failed to Add Policy " & sFolderName & " to DME", Application.ProductName)
								End If
							End If
						End If
					End If
				End If
			Loop 
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			oAPI = Nothing
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in TransferPolicy: " & Information.Err().Number & ":" & excep.Message, "TransferPolicy", MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
			 'Close error log
			FileSystem.FileClose(lFile)
			Return result
			
		End Try
	End Function
	
	Private Function TransferClaim() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
		Dim lNoOfRecords, lCount As Integer
		Dim sExCode, sDescription As String
		Dim lMaxClaimCnt, lFile As Integer
		Dim sFileName, sData As String
		
		Dim sNewline As String = ""
		Dim oAPI As bDOCAPI.API
        Dim vIndexArray(,) As Object
		Dim lCompanyExCode, lPartyExCode As Integer
		Dim sPartyName, sFolderName, sCompanyName As String
		Dim lCompanyFolderNum As Integer
		
		Dim lClaimID As Integer
		Dim sClaimNo As String = ""
		Dim lPartyFolderNum As Integer
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Label1.Text = "Loading Claim Records ..."
			Me.Refresh()
			
			 'Create log file
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMETransClaim.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 ' Get max number of Claims
			sSQL = "SELECT MAX(claim_id) FROM claim"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Claim ID", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Max Claim ID.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxClaimCnt = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxClaimCnt = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxClaimCnt
			End If
			
			 ' Build up SQL to get details from claim table
			sSQL = "SELECT c.claim_id, c.claim_number, c.description, i.insured_cnt"
			sSQL = sSQL & " FROM claim c, insurance_file i"
			sSQL = sSQL & " WHERE c.policy_id = i.insurance_file_cnt"
			sSQL = sSQL & " ORDER BY claim_id"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Claim Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				MessageBox.Show("Failed To Retrieve Claim Details.", Application.ProductName)
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return result
			End If
			
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Me.Label1.Text = "Loading Claim Records ..."
			Me.Refresh()
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				

				If CStr(vArray(1, lCount)).Substring(0, 3) = "000" Then

					sFolderName = "C" & StringsHelper.Format(vArray(1, lCount), "000000000")
				Else

					sFolderName = CStr(vArray(1, lCount))
				End If
				

				sExCode = "C" & StringsHelper.Format(vArray(0, lCount), "000000000")

				lReturn = CType(GEMApostrophes(CStr(vArray(2, lCount)).Trim(), sDescription), gPMConstants.PMEReturnCode)
				
				 ' Display current Claim

				ProgressBar1.Value = CInt(vArray(0, lCount))
				
				 ' Check if current policy exists in DME
				sSQL = "SELECT * FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name like '" & sFolderName & "   %'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray2) Then
					
					 ' Log details of missing claim




					sData = CStr(vArray(0, lCount)) & "|" & CStr(vArray(1, lCount)) & "|" & CStr(vArray(2, lCount)) & "|" & CStr(vArray(3, lCount))
					
					FileSystem.PrintLine(lFile, sData)
					
				End If
				
				sSQL = "UPDATE DOC_folder SET ex_code = '" & sExCode & "'"
				sSQL = sSQL & " WHERE folder_name like '" & sFolderName & "   %'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
				
				 ' If SQL fails
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					 ' Display error message

					MessageBox.Show("Failed To UPDATE DOC_folder Details:" & Strings.Chr(13) & Strings.Chr(10) &  _
					                "Folder - " & CStr(vArray(1, lCount)), Application.ProductName)
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Me.Label1.Text = "Adding Claim Records ..."
			Me.Refresh()
			
			 'Read in any enties that have been logged
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Input)
			
			oAPI = New bDOCAPI.API()
			
			 'Check for errors
			If oAPI Is Nothing Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			lReturn = oAPI.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=oDBDME)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				 'Close error log
				FileSystem.FileClose(lFile)
				Return result
			End If
			
			Do Until FileSystem.EOF(lFile)
				
				sNewline = FileSystem.LineInput(lFile)
				
				If sNewline.Substring(0, 10) <> "Time Taken" Then
					
					lReturn = CType(ParseCommandLine2(sNewline, lClaimID, sClaimNo, sDescription, lPartyExCode), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Failed to Parse Command Line : " & sNewline, Application.ProductName)
						result = gPMConstants.PMEReturnCode.PMFalse
						 'Close error log
						FileSystem.FileClose(lFile)
						Return result
					End If
					
					If sClaimNo.Substring(0, 3) = "000" Then
						sClaimNo = "C" & StringsHelper.Format(sClaimNo, "000000000")
					End If
					
					sFolderName = sClaimNo & "   " & sDescription
					sExCode = "C" & StringsHelper.Format(lClaimID, "000000000")
					
					 ' Get Client details
					sSQL = "SELECT folder_name, folder_num, parent_num FROM DOC_folder"
					sSQL = sSQL & " WHERE ex_code = '" & CStr(lPartyExCode) & "'"
					sSQL = sSQL & " AND folder_level = 1"
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Company Name", False, -1, vArray), gPMConstants.PMEReturnCode)
					
					 ' If SQL fails
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
						
						 ' Display error message
						MessageBox.Show("Failed To Retrieve Party Folder For " & sFolderName, Application.ProductName)
					Else
						

						sPartyName = CStr(vArray(0, 0))

						lPartyFolderNum = CInt(vArray(1, 0))

						lCompanyFolderNum = CInt(vArray(2, 0))
						
						 ' Get Company description
						sSQL = "SELECT folder_name, ex_code FROM DOC_folder"
						sSQL = sSQL & " WHERE folder_num = " & CStr(lCompanyFolderNum)
						sSQL = sSQL & " AND folder_level = 0"
						
						 ' Fire SQL
						lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Company Details", False, -1, vArray), gPMConstants.PMEReturnCode)
						
						 ' If SQL fails
						If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
							
							 ' Display error message
							MessageBox.Show("Failed To Retrieve Company Name For " & sFolderName, Application.ProductName)
						Else
							

							sCompanyName = CStr(vArray(0, 0))

							lCompanyExCode = CInt(vArray(1, 0))
							
							 'Build Index Array
							ReDim vIndexArray(1, 2)
							

							vIndexArray(0, 0) = lCompanyExCode

							vIndexArray(0, 1) = lPartyExCode

							vIndexArray(0, 2) = sExCode
							

							vIndexArray(1, 0) = sCompanyName

							vIndexArray(1, 1) = sPartyName

							vIndexArray(1, 2) = sFolderName
							

							lReturn = oAPI.AddIndex(vIndexArray)
							
							If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
								MessageBox.Show("Failed to Add Policy " & sFolderName & " to DME", Application.ProductName)
								result = gPMConstants.PMEReturnCode.PMFalse
								 'Close error log
								FileSystem.FileClose(lFile)
								Return result
							End If
						End If
					End If
				End If
			Loop 
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			oAPI = Nothing
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in TransferClaim: " & Information.Err().Number & ":" & excep.Message, "TransferClaim", MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
			 'Close error log
			FileSystem.FileClose(lFile)
			Return result
			
		End Try
	End Function
	
	Private Function MergeCompany() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
        Dim vArray3(,) As Object
		Dim lNoOfRecords, lCount, lMaxClientCnt, lFile As Integer
		Dim sFileName, sData As String
        Dim lBaseFolderNum As Integer
		Dim sCompanyName As String = ""
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			Label1.Text = "Loading Company Data ...."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMEMergeCompany.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 'Find Max number of folders
			sSQL = "SELECT MAX(folder_num) FROM DOC_folder"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Max Folder Number", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Max Folder Number.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			lMaxClientCnt = CInt(vArray(0, 0))
			
			 'Find duplicate company
			sSQL = "EXEC spu_DOC_select_duplicate_company"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Duplicate Companies", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Company Details.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'If successfull but no values returned there are no duplicates
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
				ProgressBar1.Maximum = 1
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
				If lNoOfRecords > 0 Then
					ProgressBar1.Maximum = lNoOfRecords
				Else
					ProgressBar1.Maximum = 1
				End If
			End If
			
			Label1.Text = "Merging Company Records ..."
			Me.Refresh()
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				
				ProgressBar1.Value = lCount
				

				lBaseFolderNum = CInt(vArray(1, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(0, lCount)).Trim(), sCompanyName), gPMConstants.PMEReturnCode)
				
				sSQL = "SELECT folder_num FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name = '" & sCompanyName & "'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If Information.IsArray(vArray2) Then

					For lCount2 As Integer = 0 To vArray2.GetUpperBound(1)
						
						sSQL = "SELECT folder_num FROM DOC_folder"

						sSQL = sSQL & " WHERE Parent_num = " & CStr(vArray2(0, lCount2))
						
						 ' Fire SQL
						lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Client Folder Details", False, -1, vArray3), gPMConstants.PMEReturnCode)
						
						If Information.IsArray(vArray3) Then

							For lCount3 As Integer = 0 To vArray3.GetUpperBound(1)
								
								sSQL = "UPDATE DOC_folder SET parent_num = " & lBaseFolderNum

								sSQL = sSQL & " WHERE folder_num = " & CStr(vArray3(0, lCount3))
								
								 ' Fire SQL
								lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									FileSystem.PrintLine(lFile, "Failed to update Company - " & sCompanyName)
									FileSystem.PrintLine(lFile, "")
								End If
								
							Next 
						End If
					Next 
				End If
				
				 'If successful delete other versions of the company
				If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					
					sSQL = "DELETE FROM DOC_folder WHERE folder_name = '" & sCompanyName & "'"
					sSQL = sSQL & " AND folder_num != " & CStr(lBaseFolderNum)
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						FileSystem.PrintLine(lFile, "Failed to Delete Duplicate Companies - " & sCompanyName)
						FileSystem.PrintLine(lFile, "")
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			FileSystem.PrintLine(lFile, "Failed in MergeCompany: " & Information.Err().Number & ":" & excep.Message, MsgBoxStyle.Critical, "MergeCompany")
			 'Close error log
			FileSystem.FileClose(lFile)
            Return gPMConstants.PMEReturnCode.PMFalse
			Return result


			Return result
		End Try
	End Function
	
	Private Function MergeClient() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
        Dim vArray3(,) As Object
		Dim lNoOfRecords, lCount, lMaxClientCnt, lFile As Integer
		Dim sFileName, sData As String
        Dim lBaseFolderNum As Integer
		Dim sClientName As String = ""
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			Label1.Text = "Loading Client Data ...."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMEMergeClient.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 'Find Max number of folders
			sSQL = "SELECT MAX(folder_num) FROM DOC_folder"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Max Folder Number", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Max Folder Number.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			lMaxClientCnt = CInt(vArray(0, 0))
			
			 'Find duplicate clients
			sSQL = "EXEC spu_DOC_select_duplicate_clients"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Duplicate Clients", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Client Details.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'If successfull but no values returned there are no duplicates
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
				ProgressBar1.Maximum = 1
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
				If lNoOfRecords > 0 Then
					ProgressBar1.Maximum = lNoOfRecords
				Else
					ProgressBar1.Maximum = 1
				End If
			End If
			
			Label1.Text = "Merging Client Records ..."
			Me.Refresh()
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				
				ProgressBar1.Value = lCount
				

				lBaseFolderNum = CInt(vArray(1, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(0, lCount)).Trim(), sClientName), gPMConstants.PMEReturnCode)
				
				sSQL = "SELECT folder_num FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name = '" & sClientName & "'"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If Information.IsArray(vArray2) Then

					For lCount2 As Integer = 0 To vArray2.GetUpperBound(1)
						
						sSQL = "SELECT folder_num FROM DOC_folder"

						sSQL = sSQL & " WHERE Parent_num = " & CStr(vArray2(0, lCount2))
						
						 ' Fire SQL
						lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Policy Folder Details", False, -1, vArray3), gPMConstants.PMEReturnCode)
						
						If Information.IsArray(vArray3) Then

							For lCount3 As Integer = 0 To vArray3.GetUpperBound(1)
								
								sSQL = "UPDATE DOC_folder SET parent_num = " & lBaseFolderNum

								sSQL = sSQL & " WHERE folder_num = " & CStr(vArray3(0, lCount3))
								
								 ' Fire SQL
								lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									FileSystem.PrintLine(lFile, "Failed to update Client - " & sClientName)
									FileSystem.PrintLine(lFile, "")
								End If
								
							Next 
						End If
					Next 
				End If
				
				 'If successful delete other versions of the client
				If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					
					sSQL = "DELETE FROM DOC_folder WHERE folder_name = '" & sClientName & "'"
					sSQL = sSQL & " AND folder_num != " & CStr(lBaseFolderNum)
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						FileSystem.PrintLine(lFile, "Failed to Delete Duplicate Clients - " & sClientName)
						FileSystem.PrintLine(lFile, "")
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ChkClient.Text = "Client Records Updated"
			ChkClient.Enabled = False
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			FileSystem.PrintLine(lFile, "Failed in MergeClient: " & Information.Err().Number & ":" & excep.Message, MsgBoxStyle.Critical, "MergeClient")
			 'Close error log
			FileSystem.FileClose(lFile)
			Return gPMConstants.PMEReturnCode.PMFalse


			Return result
		End Try
	End Function
	
	Private Function MergePolicy() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
        Dim vArray3(,) As Object
        Dim lNoOfRecords, lCount, lMaxPolicyCnt, lFile As Integer
		Dim sFileName, sData As String
		Dim lBaseFolderNum, lParentNum As Integer
        Dim sPolicyDesc As String
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Label1.Text = "Loading Policy Data ...."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMEMergePolicy.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 'Find Max number of folders
			sSQL = "SELECT MAX(folder_num) FROM DOC_folder"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Max Folder Number", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Max Folder Number.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			lMaxPolicyCnt = CInt(vArray(0, 0))
			
			 'Find duplicate policies
			sSQL = "EXEC spu_DOC_select_duplicate_policies"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Retrieveing Duplicate Policies", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Policy Details.")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'If successfull but no values returned there are no duplicates
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
				ProgressBar1.Maximum = 1
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
				If lNoOfRecords > 0 Then
					ProgressBar1.Maximum = lNoOfRecords
				Else
					ProgressBar1.Maximum = 1
				End If
			End If
			
			Label1.Text = "Merging Policy Records ..."
			Me.Refresh()
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				
				ProgressBar1.Value = lCount
				

				lParentNum = CInt(vArray(2, lCount))

				lBaseFolderNum = CInt(vArray(1, lCount))

				lReturn = CType(GEMApostrophes(CStr(vArray(0, lCount)).Trim(), sPolicyDesc), gPMConstants.PMEReturnCode)
				
				sSQL = "SELECT folder_num FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name = '" & sPolicyDesc & "'"
				sSQL = sSQL & " AND parent_num = " & CStr(lParentNum)
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If Information.IsArray(vArray2) Then

					For lCount2 As Integer = 0 To vArray2.GetUpperBound(1)
						
						sSQL = "SELECT doc_num FROM DOC_document"

						sSQL = sSQL & " WHERE folder_num = " & CStr(vArray2(0, lCount2))
						
						 ' Fire SQL
						lReturn = CType(oDBDME.SQLSelect(sSQL, "Selecting Policy Folder Details", False, -1, vArray3), gPMConstants.PMEReturnCode)
						
						If Information.IsArray(vArray3) Then

							For lCount3 As Integer = 0 To vArray3.GetUpperBound(1)
								
								sSQL = "UPDATE DOC_document SET folder_num = " & lBaseFolderNum

								sSQL = sSQL & " WHERE doc_num = " & CStr(vArray3(0, lCount3))
								
								 ' Fire SQL
								lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Document Details", False), gPMConstants.PMEReturnCode)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

									FileSystem.PrintLine(lFile, "Failed to update Policy - " & CStr(vArray(0, lCount - 1)))
									FileSystem.PrintLine(lFile, "")
								End If
							Next 
						End If
					Next 
				End If
				
				 'If successful delete other versions of the Policy
				If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					
					sSQL = "DELETE FROM DOC_folder WHERE folder_name = '" & sPolicyDesc & "'"
					sSQL = sSQL & " AND parent_num = " & CStr(lParentNum)
					sSQL = sSQL & " AND folder_num != " & CStr(lBaseFolderNum)
					
					 ' Fire SQL
					lReturn = CType(oDBDME.SQLAction(sSQL, "Deleting Policy Details", False), gPMConstants.PMEReturnCode)
					
					If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						
						FileSystem.PrintLine(lFile, "Failed to Delete Duplicate Policies - " & sPolicyDesc)
						FileSystem.PrintLine(lFile, "")
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			 '    lPolicyCnt = 1
			 '
			 '    'Find duplicates with same ex_code but different decriptions
			 '    Do While lPolicyCnt < lMaxPolicyCnt
			 '
			 '        'Find duplicate policies
			 '        sSql = "SELECT folder_name, folder_num FROM DOC_folder a WHERE"
			 '        sSql = sSql & " (SELECT COUNT(*) FROM DOC_folder b"
			 '        sSql = sSql & " WHERE b.ex_code = a.ex_code AND b.parent_num = a.parent_num"
			 '        sSql = sSql & " AND a.ex_code != '') > 1"
			 '        sSql = sSql & " AND folder_level = 2"
			 '        sSql = sSql & " AND folder_num > " & lPolicyCnt
			 '        sSql = sSql & " ORDER BY folder_num"
			 '
			 '        ' Fire SQL
			 '        lReturn = oDBDME.SQLSelect(sSql, "Retrieveing Duplicate Policies", False, -1, vArray)
			 '
			 '        ' If SQL fails
			 '        If lReturn <> PMTrue Then
			 '
			 '            ' Display error message
			 '            MsgBox "Failed To Retrieve Policy Details."
			 '            MergePolicy = PMFalse
			 '            ' Exit here
			 '            Exit Function
			 '        End If
			 '
			 '        'If successfull but no values returned there are no duplicates left
			 '        If IsArray(vArray) = False Then
			 '            lNoOfRecords = 0
			 '        Else
			 '            lNoOfRecords = UBound(vArray, 2)
			 '        End If
			 '
			 '        Label1.Caption = "Updating Policy :"
			 '        Me.Refresh
			 '
			 '        lCount = 1
			 '
			 '        Do While lCount <= lNoOfRecords
			 '
			 '            sSql = "SELECT parent_num, ex_code FROM DOC_folder"
			 '            sSql = sSql & " WHERE folder_name = (SELECT folder_name"
			 '            sSql = sSql & " WHERE folder_num = " & vArray(1, lCount - 1) & ")"
			 '            sSql = sSql & " AND folder_num = " & vArray(1, lCount - 1)
			 '
			 '            ' Fire SQL
			 '            lReturn = oDBDME.SQLSelect(sSql, "Checking Folder Details", False, -1, vArray2)
			 '
			 '            If IsArray(vArray2) = True Then
			 '
			 '                lParentNum = vArray2(0, 0)
			 '                sExCode = vArray2(1, 0)
			 '
			 '            End If
			 '
			 '            sSql = "SELECT MIN(folder_num) FROM DOC_folder"
			 '            sSql = sSql & " WHERE folder_name = (SELECT folder_name"
			 '            sSql = sSql & " WHERE folder_num = " & vArray(1, lCount - 1) & ")"
			 '            sSql = sSql & " AND parent_num = " & lParentNum
			 '
			 '            ' Fire SQL
			 '            lReturn = oDBDME.SQLSelect(sSql, "Checking Folder Details", False, -1, vArray2)
			 '
			 '            If lReturn <> PMTrue Or IsArray(vArray2) = False Then
			 '
			 '                sData = "Document : " & Trim(vArray(0, lCount - 1))
			 '                Print #lFile, sData
			 '
			 '            Else
			 '
			 '                If vArray2(0, 0) <> "" Then
			 '
			 '                    lBaseFolderNum = vArray2(0, 0)
			 '                    lReturn = GEMApostrophes(Trim(vArray(0, lCount - 1)), sPolicyDesc)
			 '
			 '                    sSql = "SELECT folder_num FROM DOC_folder"
			 '                    sSql = sSql & " WHERE folder_name = '" & sPolicyDesc & "'"
			 '                    sSql = sSql & " AND parent_num = " & lParentNum
			 '
			 '                    ' Fire SQL
			 '                    lReturn = oDBDME.SQLSelect(sSql, "Selecting Folder Details", False, -1, vArray3)
			 '
			 '                    If IsArray(vArray3) = True Then
			 '                        For lCount2 = 0 To UBound(vArray3, 2)
			 '
			 '                            sSql = "SELECT doc_num FROM DOC_document"
			 '                            sSql = sSql & " WHERE folder_num = " & vArray3(0, lCount2)
			 '
			 '                            ' Fire SQL
			 '                            lReturn = oDBDME.SQLSelect(sSql, "Selecting Policy Folder Details", False, -1, vArray4)
			 '
			 '                            If IsArray(vArray4) = True Then
			 '                                For lCount3 = 0 To UBound(vArray4, 2)
			 '
			 '                                    sSql = "UPDATE DOC_document SET folder_num = " & lBaseFolderNum
			 '                                    sSql = sSql & " WHERE doc_num = " & vArray4(0, lCount3)
			 '
			 '                                    ' Fire SQL
			 '                                    lReturn = oDBDME.SQLAction(sSql, "Updating Document Details", False)
			 '
			 '                                    If lReturn <> PMTrue Then
			 '                                        MsgBox "Failed to update Policy - " & vArray(0, lCount - 1)
			 '                                    End If
			 '                                Next
			 '                            End If
			 '                        Next
			 '                    End If
			 '
			 '                    'If successful delete other versions of the Policy
			 '                    If lReturn = PMTrue Then
			 '
			 '                        sSql = "DELETE FROM DOC_folder WHERE ex_code = '" & sExCode & "'"
			 '                        sSql = sSql & " AND parent_num = " & lParentNum
			 '                        sSql = sSql & " AND folder_num != " & lBaseFolderNum
			 '
			 '                        ' Fire SQL
			 '                        lReturn = oDBDME.SQLAction(sSql, "Deleting Policy Details", False)
			 '
			 '                        If lReturn <> PMTrue Then
			 '
			 '                            MsgBox "Failed to Delete Duplicate Policies"
			 '
			 '                        End If
			 '
			 '                    End If
			 '
			 '                End If
			 '
			 '            End If
			 '
			 '            lCount = lCount + 1
			 '
			 '        Loop
			 '
			 '        If lNoOfRecords = 0 Then
			 '            lPolicyCnt = lMaxPolicyCnt
			 '        Else
			 '            lPolicyCnt = vArray(1, lCount - 2)
			 '        End If
			 '
			 '    Loop
			
			ChkClaim.Text = "Policies/Claims Updated"
			ChkClaim.Enabled = False
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			FileSystem.PrintLine(lFile, "Failed in MergePolicy: " & Information.Err().Number & ":" & excep.Message, MsgBoxStyle.Critical, "MergePolicy")
			 'Close error log
			FileSystem.FileClose(lFile)
			Return gPMConstants.PMEReturnCode.PMFalse
			
		End Try
	End Function
	
	Private Function MergeDescriptions() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim vArray2(,) As Object
        Dim lNoOfRecords, lCount, lCount2, lMaxPolicyCnt, lMaxClaimCnt As Integer
		Dim sFileName As String = ""
		Dim lFile As Integer
		Dim sData, sPolicyNo, sClaimNo, sDescription, sFolderDesc, sExCode As String
		
		Dim lTimeTaken, lStart As Integer
		
		Try 
			
			lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Label1.Text = "Loading Policy Descriptions ...."
			Me.Refresh()
			
			lFile = FileSystem.FreeFile()
			
			sFileName = "C:\DMEMergeDescriptions.Log"
			
			FileSystem.FileOpen(lFile, sFileName, OpenMode.Output)
			
			 '**** Update Policy Folders ****'
			
			 'Find Max number of folders
			sSQL = "SELECT MAX(insurance_folder_cnt) FROM insurance_folder"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Insurance Folder Count", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Max Insurance Folder Count.")
				FileSystem.PrintLine(lFile, "")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxPolicyCnt = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxPolicyCnt = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxPolicyCnt
			End If
			
			 'Find policy details
			sSQL = "SELECT i.insurance_ref, s.last_trans_description, o.insurance_folder_cnt, i.source_id"
			sSQL = sSQL & " FROM insurance_folder o, insurance_file i, insurance_file_system s"
			sSQL = sSQL & " WHERE o.insurance_folder_cnt = i.insurance_folder_cnt"
			sSQL = sSQL & " AND i.insurance_file_cnt = s.insurance_file_cnt"
			sSQL = sSQL & " ORDER BY o.insurance_folder_cnt"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Policy Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Policy Details")
				FileSystem.PrintLine(lFile, "")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Label1.Text = "Merging Policy Descriptions ..."
			Me.Refresh()
			
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				

				ProgressBar1.Value = CInt(vArray(2, lCount))
				

				lReturn = CType(GEMApostrophes(CStr(vArray(0, lCount)).Trim(), sPolicyNo), gPMConstants.PMEReturnCode)

				lReturn = CType(GEMApostrophes(CStr(vArray(1, lCount)).Trim(), sDescription), gPMConstants.PMEReturnCode)
				sFolderDesc = (sPolicyNo & "   " & sDescription).Substring(0, 70)
				
				sSQL = "SELECT folder_num, folder_name, ex_code FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name like '" & sPolicyNo & "   %'"
				sSQL = sSQL & " OR folder_name like '" & sPolicyNo & "'"
				sSQL = sSQL & " AND folder_level = 2"
				sSQL = sSQL & " ORDER BY ex_code"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					FileSystem.PrintLine(lFile, "Failed in Checking Folder Details." & Strings.Chr(13) & Strings.Chr(10) & "Policy : " & sPolicyNo)
					FileSystem.PrintLine(lFile, "")
				Else
					
					If Information.IsArray(vArray2) Then
						



						If Not vArray2(2, 0).Equals(vArray2(2, vArray2.GetUpperBound(1))) Then
							sData = "Duplicate Policy : " & sPolicyNo
							FileSystem.PrintLine(lFile, sData)
							sData = "Description : " & sDescription
							FileSystem.PrintLine(lFile, sData)
							FileSystem.PrintLine(lFile, "")
						Else
							lCount2 = 0

							Do While lCount2 <= vArray2.GetUpperBound(1)
								sSQL = "UPDATE DOC_folder SET folder_name = '" & sFolderDesc & "'"

								sSQL = sSQL & " WHERE folder_num = " & CStr(vArray2(0, lCount2))
								sSQL = sSQL & " AND folder_level = 2"
								
								 ' Fire SQL
								lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									FileSystem.PrintLine(lFile, "Failed to update Policy - " & sPolicyNo)
									FileSystem.PrintLine(lFile, "")
								End If
								lCount2 += 1
							Loop 
						End If
					Else
						
						sData = "Missing Policy : " & sPolicyNo
						FileSystem.PrintLine(lFile, sData)
						sData = "Description : " & sDescription
						FileSystem.PrintLine(lFile, sData)
						FileSystem.PrintLine(lFile, "")
						
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			 '**** Update Claims folders ****'
			
			Label1.Text = "Loading Claim Descriptions ...."
			Me.Refresh()
			
			 ' Get max number of Claims
			sSQL = "SELECT MAX(claim_id) FROM claim"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Max Claim ID", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Max Claim ID.")
				FileSystem.PrintLine(lFile, "")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			If CStr(vArray(0, 0)) = "" Then
				lMaxClaimCnt = 0
				ProgressBar1.Maximum = 1
			Else

				lMaxClaimCnt = CInt(vArray(0, 0))
				ProgressBar1.Maximum = lMaxClaimCnt
			End If
			
			 ' Build up SQL to get details from claim table
			sSQL = "SELECT c.claim_id, c.claim_number, c.description, i.insured_cnt"
			sSQL = sSQL & " FROM claim c, insurance_file i"
			sSQL = sSQL & " WHERE c.policy_id = i.insurance_file_cnt"
			sSQL = sSQL & " ORDER BY claim_id"
			
			 ' Fire SQL
			lReturn = CType(oDBSirius.SQLSelect(sSQL, "Retrieveing Claim Details", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Retrieve Claim Details")
				FileSystem.PrintLine(lFile, "")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Label1.Text = "Merging Claim Records ..."
			Me.Refresh()
			
			If Not Information.IsArray(vArray) Then
				lNoOfRecords = 0
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords And lNoOfRecords > 0
				

				ProgressBar1.Value = CInt(vArray(0, lCount))
				

				lReturn = CType(GEMApostrophes(CStr(vArray(1, lCount)).Trim(), sClaimNo), gPMConstants.PMEReturnCode)

				lReturn = CType(GEMApostrophes(CStr(vArray(2, lCount)).Trim(), sDescription), gPMConstants.PMEReturnCode)
				
				 '        If Left$(sClaimNo, 3) = "000" Then
				 '            sClaimNo = "C" & Format(sClaimNo, "000000000")
				 '        End If
				
				sFolderDesc = (sClaimNo & "   " & sDescription).Substring(0, 70)

				sExCode = "C" & StringsHelper.Format(vArray(0, lCount), "000000000")
				
				sSQL = "SELECT folder_num, folder_name, ex_code FROM DOC_folder"
				sSQL = sSQL & " WHERE folder_name like '%   " & sDescription & "'"
				sSQL = sSQL & " AND ex_code = '" & sExCode & "'"
				sSQL = sSQL & " AND folder_level = 2"
				sSQL = sSQL & " ORDER BY ex_code"
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Checking Folder Details", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					FileSystem.PrintLine(lFile, "Failed in Checking Folder Details." & Strings.Chr(13) & Strings.Chr(10) & "Policy : " & sPolicyNo)
					FileSystem.PrintLine(lFile, "")
				Else
					
					If Information.IsArray(vArray2) Then
						



						If Not vArray2(2, 0).Equals(vArray2(2, vArray2.GetUpperBound(1))) Then
							sData = "Duplicate Claim : " & sClaimNo
							FileSystem.PrintLine(lFile, sData)
							sData = "Description : " & sDescription
							FileSystem.PrintLine(lFile, sData)
							FileSystem.PrintLine(lFile, "")
						Else
							lCount2 = 0

							Do While lCount2 <= vArray2.GetUpperBound(1)
								sSQL = "UPDATE DOC_folder SET folder_name = '" & sFolderDesc & "'"

								sSQL = sSQL & " WHERE folder_num = " & CStr(vArray2(0, lCount2))
								sSQL = sSQL & " AND folder_level = 2"
								
								 ' Fire SQL
								lReturn = CType(oDBDME.SQLAction(sSQL, "Updating Folder Details", False), gPMConstants.PMEReturnCode)
								
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									FileSystem.PrintLine(lFile, "Failed to update Claim - " & sClaimNo)
									FileSystem.PrintLine(lFile, "")
								End If
								lCount2 += 1
							Loop 
						End If
					Else
						
						sData = "Missing Claim : " & sClaimNo
						FileSystem.PrintLine(lFile, sData)
						sData = "Description : " & sDescription
						FileSystem.PrintLine(lFile, sData)
						FileSystem.PrintLine(lFile, "")
						
					End If
					
				End If
				
				lCount += 1
				
			Loop 
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			Label1.Text = "Updating Log File ...."
			Me.Refresh()
			
			 'Find other folder discrepancies
			sSQL = "EXEC spu_DOC_find_discrepancies"
			
			 ' Fire SQL
			lReturn = CType(oDBDME.SQLSelect(sSQL, "Find Folder Discrepancies", False, -1, vArray), gPMConstants.PMEReturnCode)
			
			 ' If SQL fails
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				 ' Display error message
				FileSystem.PrintLine(lFile, "Failed To Find Folder Discrepancies")
				FileSystem.PrintLine(lFile, "")
				 'Close error log
				FileSystem.FileClose(lFile)
				 ' Exit here
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 ' If nothing returned then transfer successful
			If Not Information.IsArray(vArray) Then
				
				lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
				sData = "Time Taken = " & lTimeTaken & " Seconds"
				FileSystem.PrintLine(lFile, sData)
				FileSystem.FileClose(lFile)
				
				ChkPolicies.Text = "Policy Descriptions Updated"
				ChkPolicies.Enabled = False
				Return result
			Else

				lNoOfRecords = vArray.GetUpperBound(1)
			End If
			
			lCount = 0
			
			Do While lCount <= lNoOfRecords
				
				 'Find other folder discrepancies
				sSQL = "SELECT folder_name FROM DOC_folder"

				sSQL = sSQL & " WHERE folder_num = " & CStr(vArray(1, lCount))
				
				 ' Fire SQL
				lReturn = CType(oDBDME.SQLSelect(sSQL, "Find Client Name", False, -1, vArray2), gPMConstants.PMEReturnCode)
				
				 ' If SQL fails
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray2) Then
					
					 ' Display error message

					FileSystem.PrintLine(lFile, "Failed To Find Client of Folder Discrepancies - " & CStr(vArray(0, lCount)))
					FileSystem.PrintLine(lFile, "")
					 'Close error log
					FileSystem.FileClose(lFile)
					 ' Exit here
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				


				sData = "Suspect - Client : " & CStr(vArray2(0, 0)) & " - Policy : " & CStr(vArray(0, lCount))
				FileSystem.PrintLine(lFile, sData)
				FileSystem.PrintLine(lFile, "")
				
				lCount += 1
				
			Loop 
			
			lTimeTaken = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
			sData = "Time Taken = " & lTimeTaken & " Seconds"
			FileSystem.PrintLine(lFile, sData)
			
			 'Close error log
			FileSystem.FileClose(lFile)
			
			ProgressBar1.Value = ProgressBar1.Maximum
			
			ChkPolicies.Text = "Policy Descriptions Updated"
			ChkPolicies.Enabled = False
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			FileSystem.PrintLine(lFile, "Failed in MergeDescriptions: " & Information.Err().Number & ":" & excep.Message, MsgBoxStyle.Critical, "MergeDescriptions")
			FileSystem.PrintLine(lFile, "")
			 'Close error log
			FileSystem.FileClose(lFile)
			Return gPMConstants.PMEReturnCode.PMFalse
			
		End Try
	End Function
	

	Private Sub FrmDMETransfer_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim vDatabase As Object
        Dim bCloseDatabase As Boolean
		
		 ' Have we a valid Database Object Reference?
		 '    lReturn = CheckPMProductInstalled(v_lPMProductFamily:=pmePFDocumaster, _
		 ''                                                           r_bInstalled:=bDMEInstalled)
		 '
		 '    If (lReturn <> PMTrue) Or bDMEInstalled = False Then
		 '        End
		 '    End If
		
        Dim lReturn As gPMConstants.PMEReturnCode = CheckDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iUserID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=g_oDatabase, v_vDatabase:=vDatabase)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		ProgressBar1.Value = 0
		ProgressBar1.Minimum = 0
		
		 ' Display the form
		Me.Show()
		
		 'Uncomment for auto running
		 'CmdExit.Enabled = False
		 'Call CmdRun_Click
		 'Call CmdExit_Click
		
	End Sub
	
	Private Sub CmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdExit.Click
		
		oDBSirius = Nothing
		oDBDME = Nothing
		
		 'Exit
		Environment.Exit(0)
		
	End Sub
	
	Public Function GEMApostrophes(ByRef sStringIn As String, ByRef sStringOut As String) As Integer
		
		Dim result As Integer = 0
		Dim i As Integer
		Dim sTemp2 As String = ""
		Dim sTemp As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If sStringIn.Length = 0 Then
				Return result
			End If
			
			sTemp = New StringBuilder("")
			sTemp2 = sStringIn
			
			Do 
				i = (sTemp2.IndexOf("'"c) + 1)
				
				If i = 0 Then
					sTemp.Append(sTemp2)
					Exit Do
				End If
				
				sTemp.Append(sTemp2.Substring(0, i - 1) & "''")
				sTemp2 = sTemp2.Substring(i)
				
				If sTemp2.Substring(0, 1) = "'" Then
					Return result
				End If
				
			Loop 
			
			sStringOut = sTemp.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed in GEMApostrophes: " & Information.Err().Number & ":" & excep.Message, "GEMApostrophes", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return gPMConstants.PMEReturnCode.PMError
			
		End Try
	End Function
	
	Private Function ParseCommandLine(ByRef sTemp As String, ByRef lCompanyExCode As Integer, ByRef lPartyExCode As Integer, ByRef sExCode As String, ByRef sFolderName As String) As Integer
		
		Dim result As Integer = 0
		Dim iPosition As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			lCompanyExCode = CInt(sTemp.Substring(0, iPosition - 1))
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			lPartyExCode = CInt(sTemp.Substring(0, iPosition - 1))
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			If iPosition > 1 Then
				sExCode = sTemp.Substring(0, iPosition - 1)
			End If
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			sFolderName = sTemp
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed to Parse Command line: " & Information.Err().Number & ":" & excep.Message & Strings.Chr(13) & Strings.Chr(10) & Interaction.Command(), "ParseCommandLine", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return gPMConstants.PMEReturnCode.PMFalse
			
		End Try
	End Function
	
	Private Function ParseCommandLine2(ByRef sTemp As String, ByRef lClaimID As Integer, ByRef sClaimNo As String, ByRef sDescription As String, ByRef lPartyExCode As Integer) As Integer
		
		Dim result As Integer = 0
		Dim iPosition As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			lClaimID = CInt(sTemp.Substring(0, iPosition - 1))
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			sClaimNo = sTemp.Substring(0, iPosition - 1)
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			sDescription = sTemp.Substring(0, iPosition - 1)
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			lPartyExCode = CInt(sTemp)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed to Parse Command line 2: " & Information.Err().Number & ":" & excep.Message & Strings.Chr(13) & Strings.Chr(10) & Interaction.Command(), "ParseCommandLine", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return gPMConstants.PMEReturnCode.PMFalse
			
		End Try
	End Function
	
	Private Function ParseCommandLine3(ByRef sTemp As String, ByRef lCompanyExCode As Integer, ByRef sComanyName As String) As Integer
		
		Dim result As Integer = 0
		Dim iPosition As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			lCompanyExCode = CInt(sTemp.Substring(0, iPosition - 1))
			
			sTemp = sTemp.Substring(sTemp.Length - (sTemp.Length - iPosition))
			
			iPosition = (sTemp.IndexOf("|"c) + 1)
			
			sComanyName = sTemp
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			MessageBox.Show("Failed to Parse Command line: " & Information.Err().Number & ":" & excep.Message & Strings.Chr(13) & Strings.Chr(10) & Interaction.Command(), "ParseCommandLine", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return gPMConstants.PMEReturnCode.PMFalse
			
		End Try
	End Function
End Class
