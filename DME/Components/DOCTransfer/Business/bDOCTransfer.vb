Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	 ' ***************************************************************** '
	 ' Module Name: MainModule
	 '
	 ' Date: 24/11/1997
	 '
	 ' Description: Main module containing public variable/constants.
	 '
	 ' Edit History:
	 '
	 ' DN270802 - Change embedded SQL to reflect table changes
	 '
	 ' ***************************************************************** '
	
	
	 ' Main public constant for all functions
	 ' to identify which application this is.
	Public Const ACApp As String = "bDOCTransfer"
	
	 ' Constant for the functions to identify
	 ' which class this is.
	Private Const ACClass As String = "MainModule"
	
	 ' Public source and language ID's from the
	 ' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_sUserName As String = ""
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	Public g_iLogLevel As Integer
	Public g_sCallingAppName As String = ""
	Public g_iCurrencyID As Integer
	
	 ' Public instance of the object manager.
#If PD_EARLYBOUND = 1 Then

	Public g_oObjectManager As bObjectManager.ObjectManager
#Else
	Public g_oObjectManager As bObjectManager.ObjectManager
#End If
	
	 ' Stores the return value for a function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	 ' {* USER DEFINED CODE (Begin) *}
	
	 'Indicates transfer process has been aborted
	Private m_bAbort As Boolean
	
	 'Holds instance of the interface
	Private oDOCTransfer As bDOCTransfer.frmInterface
	
	 'Form statuses
	Public Const ACStarted As Integer = 1
	Public Const ACCancel As Integer = 2
	Public Const ACAbort As Integer = 3
	Public Const ACViewReport As Integer = 4
	
	 'Log file name
	Private Const ACLogFile As String = "c:\DOCTransfer.log"
	 'Log file number
	Private iLogFileNum As Integer
	 'Store log messages
	Private sLogMsg() As String
	
	 'Input database  (ie documaster v2)
#If PD_EARLYBOUND = 1 Then

	Private m_oDBIn As DPMDAO.Database
	Private m_oDBOut As DPMDAO.Database
#Else
	Private m_oDBIn As dPMDAO.Database
	Private m_oDBOut As dPMDAO.Database
#End If
	
	 'Declare the busines objects for writing records
	Private m_oFolder As bDOCFolder.Form
	Private m_oDocument As bDOCDocTrans.Form
	Private m_oPage As bDOCPage.Form
	Private m_oDocInfo As bDOCDocInfo.Form
	Private m_oDocName As bDOCDocName.Form
	Private m_oAnnotation As bDOCAnnotation.Form
	Private m_oDocKeyword As bDOCDocKeyword.Form
	
	 ' {* USER DEFINED CODE (End) *}
	
	Private Function OpenDatabases() As Integer
		
		
		Dim result As Integer = 0
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			 'Instance pmdao for input db
#If PD_EARLYBOUND = 1 Then

			Set m_oDBIn = New DPMDAO.Database
#Else
			m_oDBIn = New dPMDAO.Database()
#End If
			
			 ' Open the input Database
			m_lReturn = CType(NewDatabase(v_sUsername:=g_sUserName, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDBIn), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				MessageBox.Show("Failed to Open Input DB", Application.ProductName)
				Return result
			End If
			
			 'Instance pmdao for output db
#If PD_EARLYBOUND = 1 Then

			Set m_oDBOut = New DPMDAO.Database
#Else
			m_oDBOut = New dPMDAO.Database()
#End If
			
			 'open the output db
			m_lReturn = CType(NewDatabase(v_sUsername:=g_sUserName, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDBOut), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				MessageBox.Show("Failed to Open Output DB", Application.ProductName)
				Return result
			End If
			
			Return result
		
    End Function
    Private Function CloseDatabases() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

             'clsoe input db
            m_lReturn = m_oDBIn.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CloseDatabases()
                MessageBox.Show("Failed to close input database", Application.ProductName)
                Return result
            End If

             'close output db
            m_lReturn = m_oDBOut.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CloseDatabases()
                MessageBox.Show("Failed to close output database", Application.ProductName)
                Return result
            End If

            Return result

	Catch excep As System.Exception



            CloseDatabases()

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CloseDatabases", excep:=excep)
            Return result

        End Try
    End Function

    Public Sub Main()


        Try

             'Get the global object manager
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager()
#End If

             '    Call the initialise method.
            m_lReturn = CType(g_oObjectManager.Initialise(sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

             '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                 ' Failed to call the initialise method.

                 ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                 '   Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Main")

                Exit Sub
            End If

             '     Store the language ID from the object manager
             '     to the public variables, to enable us to use
             '     them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName
            End With


             'Instance the form
            oDOCTransfer = New bDOCTransfer.frmInterface()

             'Show the form
            oDOCTransfer.ShowDialog()

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Main", excep:=excep)
            Exit Sub

        End Try

    End Sub



     'UPGRADE_NOTE: (7001) The following declaration (stripquote) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
     'Private Function stripquote(ByRef namein As String) As Object
     'Dim tmp As String = ""
     'Dim itmp As Integer
     'Try 
     'itmp = (namein.IndexOf("'"c) + 1)
     'If itmp > 0 Then
     '
     'tmp = namein.Substring(0, itmp) & "'" & namein.Substring(namein.Length - (namein.Length - itmp))
     'namein = tmp
     '
     'End If
     '
     'itmp = (namein.IndexOf("|"c) + 1)
     'If itmp > 0 Then
     'Mid(namein, itmp, 1) = "X"
     'End If
     '
     'itmp = (namein.IndexOf(Strings.Chr(34).ToString()) + 1)
     'If itmp > 0 Then
     'Mid(namein, itmp, 1) = "X"
     'End If
     '
     'Catch excep As System.Exception
     '
     'MessageBox.Show(CStr(Information.Err().Number) & excep.Message, Application.ProductName)
     'Exit Function
     '
     'End Try
     '
     'End Function
    Public Sub ProcessCommand()


        Select Case oDOCTransfer.Status
            Case ACStarted
                Transfer()
            Case ACCancel
                 'Unload (oDOCTransfer)
            Case ACAbort
                m_bAbort = True
            Case ACViewReport
                ViewReport()
        End Select

    End Sub
    Private Sub Transfer()


        

             'open error log
            m_lReturn = CType(OpenLog(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

             'Write start message
            ReDim sLogMsg(0)
            sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " Data Transfer Started"
            WriteToLog(sLogMsg)

             'open the databases
            m_lReturn = CType(OpenDatabases(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(GetBusinessObjects(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = GetTotals()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

             'Delete default folders etc in the virgin database
            DeleteVirginData()

             'transfer system table stuff
            TransferSystem()

             'transfer default keywords and doc_names
            TransferKeywordsDocNames()

             'Transfer docs with no parent, as others may be linked to them
            If Not m_bAbort Then
                TransferLinkOwners()
            End If

             'if we haven't aborted then do the cabinets
            If Not m_bAbort Then
                TransferCabinets()
            End If

             'Message finished
            If Not m_bAbort Then
                MessageBox.Show("Finished" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & _
                                "Dont forget to check the transfer log (c:\DOCTransfer.log)", Application.ProductName)
            End If

            m_lReturn = CType(CloseDatabases(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

             'Write end message
            ReDim sLogMsg(0)
            sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " Data Transfer Ended"
            WriteToLog(sLogMsg)

             'close error log
            m_lReturn = CType(CloseLog(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferDrawers
     '
     ' Description: Got thru all cabinets in input DB, writing to
     ' output DB
     '
     ' ***************************************************************** '
    Private Sub TransferDrawers(ByRef lCabinetNum As Integer, ByRef lParentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Dim lFolderNum As Integer 
        Static lCntDone, lCntFailed As Integer


        

             'Get the drawers from input db
            sSQL = "SELECT drawer_num, drawer_name, access_level, "
            sSQL = sSQL & "password, ex_code FROM drawer WHERE "
            sSQL = sSQL & "cabinet_num = " & CStr(lCabinetNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETDRAWERS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDrawers - Failed to select all drawers."
                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'loop thru each drawer record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                    oDOCTransfer.proDraw.Value = lCntDone + lCntFailed

                     'Write a folder record to output DB

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(1, i))), gPMConstants.PMEReturnCode)

                    m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, vFolderName:=vResultArray(1, i), vParentNum:=lParentNum, vExCode:=vResultArray(4, i), vFolderLevel:=DOCDrawer, vAccessLevel:=vResultArray(2, i), vPassword:=vResultArray(3, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                        oDOCTransfer.lblDrawFailed.Text = CStr(lCntFailed)

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDrawers - Failed to Add Drawer "

                        sLogMsg(0) = sLogMsg(0) & CStr(vResultArray(0, i)) & ", '" & CStr(vResultArray(1, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1
                        oDOCTransfer.lblDrawDone.Text = CStr(lCntDone)

                        TransferFolders(CInt(vResultArray(0, i)), lFolderNum)

                    End If

                    Application.DoEvents()

                     'check if transfer is aborted
                    If m_bAbort Then
                        Exit Sub
                    End If

                Next i

            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferAnnotations
     '
     ' Description: Got thru all Annotations in input DB, writing to
     ' output DB, for a document.
     '
     ' ***************************************************************** '
    Private Sub TransferAnnotations(ByRef lDocumentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 


        

             'Get the Annotations from input db
            sSQL = "SELECT ann_text, user_name, create_date "
            sSQL = sSQL & "FROM annotation WHERE doc_num = " & CStr(lDocumentNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETANNOTATIONS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferAnnotations - Failed to select all annotations. " & _
                             "Document Number = " & CStr(lDocumentNum)
                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'loop thru each page record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'Write a page record to output DB.

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(0, i))), gPMConstants.PMEReturnCode)

                    m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lDocumentNum, vAnnText:=vResultArray(0, i), vUsername:=vResultArray(1, i), vCreateDate:=vResultArray(2, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferAnnotations - Failed to Add Annotation for doc "

                        sLogMsg(0) = sLogMsg(0) & CStr(lDocumentNum) & ", '" & CStr(vResultArray(0, i)) & "'"
                        WriteToLog(sLogMsg)

                    End If

                    Application.DoEvents()

                Next i

            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferKeywords
     '
     ' Description: Got thru all keywords in input DB, writing to
     ' output DB, for a document.
     '
     ' ***************************************************************** '
    Private Sub TransferKeywords(ByRef lDocumentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 


        

             'Get the Keywords from input db
            sSQL = "SELECT key_num, user_name, create_date "
            sSQL = sSQL & "FROM keyword WHERE doc_num = " & CStr(lDocumentNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETKEYWORDS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywords - Failed to select all Keywords." & _
                             "Document Number = " & CStr(lDocumentNum)
                WriteToLog(sLogMsg)
                Exit Sub
            End If

            If Information.IsArray(vResultArray) Then

                 'loop thru each page record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'Write a keyword record to output DB.
                    m_lReturn = m_oDocKeyword.DirectAdd(vDocNum:=lDocumentNum, vKeywordID:=vResultArray(0, i), vUsername:=vResultArray(1, i), vCreateDate:=vResultArray(2, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywords - Failed to Add keyword for doc "

                        sLogMsg(0) = sLogMsg(0) & CStr(lDocumentNum) & ", '" & CStr(vResultArray(0, i)) & "'"
                        WriteToLog(sLogMsg)

                    End If

                    Application.DoEvents()

                Next i

            End If


    End Sub

     ' ***************************************************************** '
     ' Name: TransferDocInfo
     '
     ' Description: Transfer the DocInfo for the document
     '
     ' ***************************************************************** '
    Private Sub TransferDocInfo(ByRef lDocumentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 


        

             'Get the DocInfo from input db
            sSQL = "SELECT expiry_date, scan_operator, doc_date, last_user, last_date"
            sSQL = sSQL & " FROM docinfo WHERE doc_num = " & CStr(lDocumentNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETDOCINFO", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDocInfo - Failed to select DocInfo for document " & CStr(lDocumentNum) & "."
                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'Write a page record to output DB.
                m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lDocumentNum, vExpiryDate:=vResultArray(0, 0), vScanUser:=vResultArray(1, 0), vDocDate:=vResultArray(2, 0), vLastUser:=vResultArray(3, 0), vLastDate:=vResultArray(4, 0))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ReDim sLogMsg(0) 
                    sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDocInfo - Failed to Add DocInfo for doc "
                    sLogMsg(0) = sLogMsg(0) & CStr(lDocumentNum)
                    WriteToLog(sLogMsg)

                End If

            End If


    End Sub

     ' ***************************************************************** '
     ' Name: TransferCabinets
     '
     ' Description: Got thru all cabinets in input DB, writing to
     ' output DB
     '
     ' ***************************************************************** '
    Private Sub TransferCabinets()

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Dim lFolderNum As Integer 
        Static lCntDone, lCntFailed As Integer

        


             'Get the cabinets from input db
            sSQL = "SELECT cabinet_num, cabinet_name, access_level, "
            sSQL = sSQL & "password, ex_code FROM cabinet"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETCABINETS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferCabinets - Failed to select all cabinets."
                WriteToLog(sLogMsg)
                Exit Sub
            End If

            If Information.IsArray(vResultArray) Then

                 'loop thru each cabinet record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                    oDOCTransfer.proCab.Value = i

                    oDOCTransfer.lblCabName.Text = CStr(vResultArray(1, i)).Trim()

                     'Write a folder record to output DB.

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(1, i))), gPMConstants.PMEReturnCode)

                    m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, vFolderName:=vResultArray(1, i), vParentNum:=0, vExCode:=vResultArray(4, i), vFolderLevel:=DOCCabinet, vAccessLevel:=vResultArray(2, i), vPassword:=vResultArray(3, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                        oDOCTransfer.lblCabFailed.Text = CStr(lCntFailed)

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferCabinets - Failed to Add Cabinet "

                        sLogMsg(0) = sLogMsg(0) & CStr(vResultArray(0, i)) & ", '" & CStr(vResultArray(1, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1
                        oDOCTransfer.lblCabDone.Text = CStr(lCntDone)

                        TransferDrawers(CInt(vResultArray(0, i)), lFolderNum)

                    End If

                    Application.DoEvents()

                     'check if transfer is aborted
                    If m_bAbort Then
                        Exit Sub
                    End If

                Next i

            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferFolders
     '
     ' Description: Got thru all Folders in input DB, writing to
     ' output DB
     '
     ' ***************************************************************** '
    Private Sub TransferFolders(ByRef lDrawerNum As Integer, ByRef lParentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Dim lFolderNum As Integer 
        Static lCntDone, lCntFailed As Integer


        

             'Get the folders from input db
            sSQL = "SELECT folder_num, folder_name, access_level, "
            sSQL = sSQL & "password, ex_code FROM folder WHERE "
            sSQL = sSQL & "drawer_num = " & CStr(lDrawerNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETFOLDERS", lNumberRecords:=gPMConstants.PMAllRecords, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferFolders - Failed to select all folders."
                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'loop thru each folder record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                    oDOCTransfer.proFold.Value = lCntDone + lCntFailed

                     'Write a folder record to output DB.

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(1, i))), gPMConstants.PMEReturnCode)

                    m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, vFolderName:=vResultArray(1, i), vParentNum:=lParentNum, vExCode:=vResultArray(4, i), vFolderLevel:=DOCFolder, vAccessLevel:=vResultArray(2, i), vPassword:=vResultArray(3, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                        oDOCTransfer.lblFoldFailed.Text = CStr(lCntFailed)

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferFolders - Failed to Add Folder "

                        sLogMsg(0) = sLogMsg(0) & CStr(vResultArray(0, i)) & ", '" & CStr(vResultArray(1, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1
                        oDOCTransfer.lblFoldDone.Text = CStr(lCntDone)

                        TransferDocuments(CInt(vResultArray(0, i)), lFolderNum)

                    End If

                    Application.DoEvents()

                     'check if transfer is aborted
                    If m_bAbort Then
                        Exit Sub
                    End If

                Next i

            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferPages
     '
     ' Description: Got thru all pages in input DB, writing to
     ' output DB, for a document.
     '
     ' ***************************************************************** '
    Private Sub TransferPages(ByRef lDocumentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Static lCntDone, lCntFailed As Integer


        

             'Get the folders from input db
            sSQL = "SELECT page_name, page_type, page_num, "
            sSQL = sSQL & "scan_date FROM page WHERE "
            sSQL = sSQL & "doc_num = " & CStr(lDocumentNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETPAGES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferPages - Failed to select all pages. " & _
                             "Document Number = " & CStr(lDocumentNum)

                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'loop thru each page record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'Write a page record to output DB.
                    m_lReturn = m_oPage.DirectAdd(vPageName:=vResultArray(0, i), vDocNum:=lDocumentNum, vPageNum:=vResultArray(2, i), vPageType:=vResultArray(1, i), vCreateDate:=vResultArray(3, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        lCntFailed += 1

                        ReDim sLogMsg(0) 

                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferPages - Failed to Add Page for doc " & _
                                     lDocumentNum & ", " & CStr(vResultArray(0, i)) & "." & _
                                     CStr(vResultArray(1, i))
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1

                    End If

                    Application.DoEvents()

                Next i

            End If


    End Sub

     ' ***************************************************************** '
     ' Name: TransferDocuments
     '
     ' Description: Got thru all dcouments in input DB, writing to
     ' output DB, for a folder.
     '
     ' ***************************************************************** '
    Private Sub TransferDocuments(ByRef lFolderNum As Integer, ByRef lParentNum As Integer) 

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Static lCntDone, lCntFailed As Integer


        

             'Get the docs from input db
            sSQL = "SELECT doc_num, doc_name, access_level, password, "
            sSQL = sSQL & "create_date, ex_code, link, doc_type FROM document WHERE "
            sSQL = sSQL & "folder_num = " & CStr(lFolderNum)

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETDOCUMENTS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDocuments - Failed to select all documents."
                WriteToLog(sLogMsg)
                Exit Sub
            End If


            If Information.IsArray(vResultArray) Then

                 'loop thru each document record

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                     'oDOCTransfer.proDoc.Value = lCntDone + lCntFailed

                     'Write a folder record to output DB.
                    m_lReturn = m_oDocument.DirectAdd(vDocNum:=vResultArray(0, i), vFolderNum:=lParentNum, vDocName:=vResultArray(1, i), vExCode:=vResultArray(5, i), vDocType:=vResultArray(7, i), vAccessLevel:=vResultArray(2, i), vPassword:=vResultArray(3, i), vZipped:="N", vCreateDate:=vResultArray(4, i), vLink:=vResultArray(6, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                         'oDOCTransfer.lblDocFailed = lCntFailed

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferDocuments - Failed to Add Document "

                        sLogMsg(0) = sLogMsg(0) & CStr(vResultArray(0, i)) & ", '" & CStr(vResultArray(1, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1


                        TransferPages(CInt(vResultArray(0, i)))

                        TransferDocInfo(CInt(vResultArray(0, i)))

                        TransferAnnotations(CInt(vResultArray(0, i)))

                        TransferKeywords(CInt(vResultArray(0, i)))

                    End If

                    Application.DoEvents()

                Next i

            End If


    End Sub

    Private Function OpenLog() As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            iLogFileNum = FileSystem.FreeFile()

            FileSystem.FileOpen(iLogFileNum, "c:\DOCTransfer.log", OpenMode.Output)

            Return result

    End Function
    Private Function CloseLog() As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            FileSystem.FileClose(iLogFileNum)

            Return result

    End Function
    Private Sub WriteToLog(ByRef sMsg() As String)



        

            For Each sMsg_item As String In sMsg

                FileSystem.PrintLine(iLogFileNum, sMsg_item)

            Next sMsg_item


    End Sub
    Private Sub ViewReport()

        


            Dim startInfo As ProcessStartInfo = New ProcessStartInfo("notepad " & ACLogFile)
            startInfo.WindowStyle = ProcessWindowStyle.Normal
            m_lReturn = CType(Process.Start(startInfo).Id, gPMConstants.PMEReturnCode)


    End Sub
     ' ***************************************************************** '
     ' Name: TransferLinkOwners
     '
     ' Description: This transfers all docs with foldernum 0, as these
     ' may be have other docs linked to them
     '
     ' ***************************************************************** '
    Private Sub TransferLinkOwners()

        Dim lTmpFoldNum As Integer
        Dim sSQL As String = ""

        

             'First create a temp folder to house them
            m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lTmpFoldNum, vFolderName:="Temporary Link Owners", vParentNum:=0, vExCode:="", vFolderLevel:=DOCCabinet, vAccessLevel:=9, vPassword:="")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferLinkOwners - Failed to create temp folder"
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If

             'Now transfer docs with parent 0 to this folder
            TransferDocuments(0, lTmpFoldNum)

             'Now change their owning folder back to 0 (must be done this way as cannot
             'insert docs into folder number 0, which does not exist - but can update
             'them in this way)
            sSQL = "UPDATE DOC_document SET folder_num = 0 WHERE folder_num = " & lTmpFoldNum

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="UPDSYS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferLinkOwners - Failed to update link owners to parent folder 0."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


    End Sub

    Private Function GetBusinessObjects() As Integer


        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

             ' Get an instance of the business object via
             ' the public object manager.
            Dim temp_m_oFolder As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oFolder, "bDOCFolder.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oFolder = temp_m_oFolder
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCFolder.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oDocument As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oDocument, "bDOCDocTrans.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oDocument = temp_m_oDocument
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCDocTrans.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oDocInfo As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oDocInfo, "bDOCDocInfo.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oDocInfo = temp_m_oDocInfo
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCDocInfo.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oPage As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oPage, "bDOCpage.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oPage = temp_m_oPage
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCpage.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oDocName As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oDocName, "bDOCDocName.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oDocName = temp_m_oDocName
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCDocName.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oAnnotation As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oAnnotation, "bDOCAnnotation.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oAnnotation = temp_m_oAnnotation
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCAnnotation.Form."
                WriteToLog(sLogMsg)
            End If

            Dim temp_m_oDocKeyword As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oDocKeyword, "bDOCDocKeyword.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oDocKeyword = temp_m_oDocKeyword
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " GetBusinessObjects - Failed to get Instance of bDOCDocKeyword.Form."
                MessageBox.Show(sLogMsg(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteToLog(sLogMsg)
            End If

            Return result

    End Function
    Private Function GetTotals() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse 
        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 
        Dim lTmp1, lTmp2 As Integer 

        

            result = gPMConstants.PMEReturnCode.PMTrue

             'Get the number of cabinets
            sSQL = "SELECT COUNT (cabinet_num) FROM cabinet"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="CABINETS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get Cabinet Count", Application.ProductName)
                Return result
            End If

             'update the progress form
            If Information.IsArray(vResultArray) Then

                oDOCTransfer.lblCabTotal.Text = CStr(vResultArray(0, 0))
                oDOCTransfer.proCab.Minimum = 0

                oDOCTransfer.proCab.Maximum = IIf(CDbl(CDbl(vResultArray(0, 0)) - 1) <= oDOCTransfer.proCab.Minimum, oDOCTransfer.proCab.Minimum + 1, CSng(CDbl(vResultArray(0, 0)) - 1))
            End If


             'Get the number of drawers
            sSQL = "SELECT COUNT (drawer_num) FROM drawer"


            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="DRAWERS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get Drawer Count", Application.ProductName)
                Return result
            End If

             'update the progress form
            If Information.IsArray(vResultArray) Then

                oDOCTransfer.lblDrawTotal.Text = CStr(vResultArray(0, 0))
                oDOCTransfer.proDraw.Minimum = 0

                oDOCTransfer.proDraw.Maximum = IIf(CDbl(CDbl(vResultArray(0, 0)) - 1) <= oDOCTransfer.proDraw.Minimum, oDOCTransfer.proDraw.Minimum + 1, CSng(CDbl(vResultArray(0, 0)) - 1))
            Else
                oDOCTransfer.lblDrawTotal.Text = CStr(0)
                oDOCTransfer.proDraw.Minimum = 0
                oDOCTransfer.proDraw.Maximum = 0
            End If


             'Get the number of folder
            sSQL = "SELECT COUNT (folder_num) FROM folder"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="FOLDERS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get folder Count", Application.ProductName)
                Return result
            End If

             'update the progress form
            If Information.IsArray(vResultArray) Then

                oDOCTransfer.lblFoldTotal.Text = CStr(vResultArray(0, 0))
                oDOCTransfer.proFold.Minimum = 0

                oDOCTransfer.proFold.Maximum = IIf(CDbl(CDbl(vResultArray(0, 0)) - 1) <= oDOCTransfer.proFold.Minimum, oDOCTransfer.proFold.Minimum + 1, CSng(CDbl(vResultArray(0, 0)) - 1))
            Else
                oDOCTransfer.lblFoldTotal.Text = CStr(0)
                oDOCTransfer.proFold.Minimum = 0
                oDOCTransfer.proFold.Maximum = 0
            End If

             'Get the number of keywords
            sSQL = "SELECT COUNT (key_num) FROM keywords"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="KEYWORDS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get keyword Count", Application.ProductName)
                Return result
            End If

            If Information.IsArray(vResultArray) Then

                lTmp1 = CInt(vResultArray(0, 0))
            Else
                lTmp1 = 0
            End If

             'Get the number of keywords and document names
            sSQL = "SELECT COUNT (doc_name) FROM docnames"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="DOCNAMES", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not get doc name Count", Application.ProductName)
                Return result
            End If

            If Information.IsArray(vResultArray) Then

                lTmp2 = CInt(vResultArray(0, 0))
            Else
                lTmp2 = 0
            End If

             'update the progress form
            If lTmp1 + lTmp2 > 0 Then
                oDOCTransfer.lblOtherTotal.Text = CStr(lTmp1 + lTmp2)
                oDOCTransfer.proOther.Minimum = 0
                oDOCTransfer.proOther.Maximum = IIf(lTmp1 + lTmp2 - 1 <= oDOCTransfer.proOther.Minimum, oDOCTransfer.proOther.Minimum + 1, lTmp1 + lTmp2 - 1)
            Else
                oDOCTransfer.lblOtherTotal.Text = CStr(0)
                oDOCTransfer.proOther.Minimum = 0
                oDOCTransfer.proOther.Maximum = 1
            End If

            Return result

    End Function

     ' ***************************************************************** '
     ' Name: TransferSystem
     '
     ' Description: Transfer stuff in system table
     '
     ' ***************************************************************** ''
    Private Sub TransferSystem()

        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 

        

             'Get the details from input db
            sSQL = "SELECT doc_date, expiry_date, admin_level, next_page FROM system"

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL, sSQLName:="GETSYSTEM", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferSystem - Failed to get system details."
                WriteToLog(sLogMsg)
                Exit Sub
            End If

            If Information.IsArray(vResultArray) Then


                sSQL = "UPDATE DOC_system SET doc_date = " & CStr(vResultArray(0, 0)) & _
                       ", expiry_date = " & CStr(vResultArray(1, 0)) & _
                       ", admin_level = " & CStr(vResultArray(2, 0)) & _
                       ", next_page = '" & CStr(vResultArray(3, 0)) & "'"

                m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="UPDSYS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ReDim sLogMsg(0) 
                    sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferSystem - Failed to update system details."
                    WriteToLog(sLogMsg)
                End If

            End If

             'check if transfer is aborted
            If m_bAbort Then
                Exit Sub
            End If


    End Sub
     ' ***************************************************************** '
     ' Name: TransferKeywordsDocNames
     '
     ' Description: Got thru all keywords and dcoument names in input DB
     ' , writing to output DB
     '
     ' ***************************************************************** ''
    Private Sub TransferKeywordsDocNames()

        Dim sSQL As New StringBuilder 
        Dim vResultArray(,) As Object 
        Dim i As Integer 
        Static lCntDone, lCntFailed As Integer
        Dim lTmp As Integer 
        Dim lTmpCnt As Integer 

        


             'Transfer the keywords

             'First delete any default keywords in the new virgin database
            sSQL = New StringBuilder("DELETE FROM DOC_keyword")
            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="DELKW", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'Get the keywords from input db
            sSQL = New StringBuilder("SELECT key_num, keyword, deleted FROM keywords")

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GETKEYWORDS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to select all keywords."
                WriteToLog(sLogMsg)
                Exit Sub
            End If

            If Information.IsArray(vResultArray) Then

                 'Switch insert identity on
                m_lReturn = CType(m_oDBOut.SQLAction(sSQL:="SET IDENTITY_INSERT keyword ON", sSQLName:="I_I_ON", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                 'log if errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ReDim sLogMsg(0) 
                    sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to set identity inserts on for keywords."
                    WriteToLog(sLogMsg)
                    Exit Sub
                End If

                 'loop thru each keyword record

                For i = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                    oDOCTransfer.proOther.Value = i

                     'Write a keyword record to output DB.

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(1, i))), gPMConstants.PMEReturnCode)


                    sSQL = New StringBuilder("INSERT INTO DOC_keyword (keyword_id, keyword, deleted)" & _
                           " VALUES (" & CStr(vResultArray(0, i)) & _
                           ", '" & CStr(vResultArray(1, i)).Trim() & "', '")


                     'sp todo - currently pmdao always returns "True" from a Yes/No field
                     'until fixed, set as 'N', as jsut means deleted keywords will
                     'be reinstated
                     '            If (CBool(vResultArray(2, 0)) = False) Then
                    sSQL.Append("N")
                     '            Else
                     '                sSQL$ = sSQL$ & "Y"
                     '            End If

                    sSQL.Append("')")

                    m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="I_I_ON", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                        oDOCTransfer.lblOtherFailed.Text = CStr(lCntFailed)

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to Add Keyword "

                        sLogMsg(0) = sLogMsg(0) & CStr(vResultArray(0, i)) & ", '" & CStr(vResultArray(1, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1
                        oDOCTransfer.lblOtherDone.Text = CStr(lCntDone)

                    End If

                    Application.DoEvents()

                     'check if transfer is aborted
                    If m_bAbort Then
                        Exit Sub
                    End If

                Next i

                lTmpCnt = i

                 'Switch insert identity off
                m_lReturn = CType(m_oDBOut.SQLAction(sSQL:="SET IDENTITY_INSERT keyword OFF", sSQLName:="I_I_OFF", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                 'log if errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ReDim sLogMsg(0) 
                    sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to set identity inserts off for keywords."
                    WriteToLog(sLogMsg)
                    Exit Sub
                End If

            End If


             'Transfer the Document Names

             'First delete any default document names in the new virgin database
            sSQL = New StringBuilder("DELETE FROM DOC_doc_name")
            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="DELDN", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'Get the doc names from input db
            sSQL = New StringBuilder("SELECT doc_name FROM docnames")

            m_lReturn = CType(m_oDBIn.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GETDOCNAMES", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0) 
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to select all doc names."
                WriteToLog(sLogMsg)
                Exit Sub
            End If

            If Information.IsArray(vResultArray) Then

                 'loop thru each document name record

                For i = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                     'update progress bar
                    oDOCTransfer.proOther.Value = lTmpCnt + i

                     'Write a document name record to output DB.

                    m_lReturn = CType(ValidateSQL(CStr(vResultArray(0, i))), gPMConstants.PMEReturnCode)

                    m_lReturn = m_oDocName.DirectAdd(vDocNameID:=lTmp, vDocName:=vResultArray(0, i))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                         'update progress form
                        lCntFailed += 1
                        oDOCTransfer.lblOtherFailed.Text = CStr(lCntFailed)

                        ReDim sLogMsg(0) 
                        sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " TransferKeywordsDocNames - Failed to Add Doc Name"

                        sLogMsg(0) = sLogMsg(0) & ", '" & CStr(vResultArray(0, i)) & "'"
                        WriteToLog(sLogMsg)
                    Else

                        lCntDone += 1
                        oDOCTransfer.lblOtherDone.Text = CStr(lCntDone)

                    End If

                    Application.DoEvents()

                     'check if transfer is aborted
                    If m_bAbort Then
                        Exit Sub
                    End If

                Next i

            End If


    End Sub

     ' ***************************************************************** '
     ' Name: DeleteVirginData
     '
     ' Description: Empty the virgin dme database
     '
     ' ***************************************************************** ''
    Private Sub DeleteVirginData()

        Dim sSQL As String = ""


        

             'Delete any folders in the new virgin database
            sSQL = "DELETE FROM DOC_folder"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELFOLD", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all folders."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default documents in the new virgin database
            sSQL = "DELETE FROM DOC_document"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELDOC", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all docs."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default keywords in the new virgin database
            sSQL = "DELETE FROM DOC_doc_info"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELDI", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all doc info."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default keywords in the new virgin database
            sSQL = "DELETE FROM DOC_page"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELPAGE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all default page."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default keywords in the new virgin database
            sSQL = "DELETE FROM DOC_annotation"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELKW", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all default annotations."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default keywords in the new virgin database
            sSQL = "DELETE FROM DOC_doc_keyword"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELDKW", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all doc keywords."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default keywords in the new virgin database
            sSQL = "DELETE FROM DOC_keyword"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELKW", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all default keywords."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


             'Delete any default document names in the new virgin database
            sSQL = "DELETE FROM DOC_doc_name"

            m_lReturn = CType(m_oDBOut.SQLAction(sSQL:=sSQL, sSQLName:="DELDN", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

             'log if errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReDim sLogMsg(0)
                sLogMsg(0) = DateTimeHelper.ToString(DateTime.Now) & " DeleteVirginData - Failed to delete all default doc names."
                WriteToLog(sLogMsg)
                m_bAbort = True
                Exit Sub
            End If


    End Sub
End Module

