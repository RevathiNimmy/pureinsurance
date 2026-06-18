Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module PMERRORS
	
	Public Const ERR_SUCCESS As Integer = 0
	Public Const ERR_D_SUCCESS As String = "None"
	
	Public Const ERR_FILENAME As Integer = 1
	Public Const ERR_D_FILENAME As String = "File not found"
	
	Public Const ERR_CANCEL As Integer = 2
	Public Const ERR_D_CANCEL As String = "Cancelled at user request"
	
	Public Const ERR_INDEXCABINET As Integer = 50
	Public Const ERR_D_INDEXCABINET As String = "Failed to create Indexed Cabinet array"
	
	Public Const ERR_OPENDDB As Integer = 100
	Public Const ERR_D_OPENDDB As String = "Failed to Open DMS Database"
	
	Public Const ERR_LOGIN As Integer = 101
	Public Const ERR_D_LOGIN As String = "Failed to Login to DMS Database"
	
	Public Const ERR_LOGOUT As Integer = 102
	Public Const ERR_D_LOGOUT As String = "Failed to Logout of DMS Database"
	
	Public Const ERR_COMMITDB As Integer = 110
	Public Const ERR_D_COMMITDB As String = "Failed to commit changes to the Database"
	
	Public Const ERR_SYNCRODEVICES As Integer = 120
	Public Const ERR_D_SYNCRODEVICES As String = "Failed to create volume/device list"
	
	Public Const ERR_NOTASK As Integer = 200
	Public Const ERR_D_NOTASK As String = "No TASK in control file"
	
	Public Const ERR_BADTASK As Integer = 201
	Public Const ERR_D_BADTASK As String = "Invalid TASK in control file"
	
	Public Const ERR_NOFILENAME As Integer = 202
	Public Const ERR_D_NOFILENAME As String = "File name missing from control file"
	
	Public Const ERR_NOCABINET As Integer = 210
	Public Const ERR_D_NOCABINET As String = "No Cabinet name/number was supplied"
	
	Public Const ERR_NODRAWER As Integer = 211
	Public Const ERR_D_NODRAWER As String = "No Drawer name/number was supplied"
	
	Public Const ERR_NOFOLDER As Integer = 212
	Public Const ERR_D_NOFOLDER As String = "No Folder name/number was supplied"
	
	Public Const ERR_NODOCUMENT As Integer = 213
	Public Const ERR_D_NODOCUMENT As String = "No Document name was supplied"
	
	Public Const ERR_INDEXDATA As Integer = 220
	Public Const ERR_D_INDEXDATA As String = "Failed to read Add/Del Index data"
	
	Public Const ERR_INVALIDCABINET As Integer = 301
	Public Const ERR_D_INVAILDCABINET As String = "Invalid Cabinet number"
	
	Public Const ERR_INVALIDDRAWER As Integer = 302
	Public Const ERR_D_INVALIDDRAWER As String = "Invalid Drawer number"
	
	Public Const ERR_INVALIDFOLDER As Integer = 303
	Public Const ERR_D_INVALIDFOLDER As String = "Invaild Folder number"
	
	Public Const ERR_INVALIDDOCUMENT As Integer = 304
	Public Const ERR_D_INVALIDDOCUMENT As String = "Invaild Document Number"
	
	Public Const ERR_CREATECABINET As Integer = 310
	Public Const ERR_D_CREATECABINET As String = "Failed to create Cabinet"
	
	Public Const ERR_CREATEDRAWER As Integer = 311
	Public Const ERR_D_CREATEDRAWER As String = "Failed to create Drawer"
	
	Public Const ERR_CREATEFOLDER As Integer = 313
	Public Const ERR_D_CREATEFOLDER As String = "Failed to create Folder"
	
	Public Const ERR_CREATEDOCUMENT As Integer = 314
	Public Const ERR_D_CREATEDOCUMENT As String = "Failed to create Document"
	
	Public Const ERR_CREATEPAGE As Integer = 315
	Public Const ERR_D_CREATEPAGE As String = "Failed to create Document Pages"
	
	Public Const ERR_CREATEDOCINFO As Integer = 316
	Public Const ERR_D_CREATEDOCINFO As String = "Failed to create Document Information"
	
	Public Const ERR_CREATEANNOTATION As Integer = 317
	Public Const ERR_D_CREATEANNOTATION As String = "Failed to create Document Annotations"
	
	Public Const ERR_CREATEKEYWORD As Integer = 318
	Public Const ERR_D_CREATEKEYWORD As String = "Failed to create Document kewywords"
	
	Public Const ERR_CREATEDOCLINK As Integer = 319
	Public Const ERR_D_CREATEDOCLINK As String = "Failed to create Document links"
	
	Public Const ERR_GETCABINET As Integer = 320
	Public Const ERR_D_GETCABINET As String = "Failed to find the Cabinet number"
	
	Public Const ERR_GETDRAWER As Integer = 321
	Public Const ERR_D_GETDRAWER As String = "Failed to find the Drawer number"
	
	Public Const ERR_GETFOLDER As Integer = 322
	Public Const ERR_D_GETFOLDER As String = "Failed to find the Folder number"
	
	Public Const ERR_FULLCABINET As Integer = 330
	Public Const ERR_D_FULLCABINET As String = "Cabinet contains data - not deleted"
	
	Public Const ERR_FULLDRAWER As Integer = 331
	Public Const ERR_D_FULLDRAWER As String = "Drawer contains data - not deleted"
	
	Public Const ERR_FULLFOLDER As Integer = 332
	Public Const ERR_D_FULLFOLDER As String = "Folder contains data - not deleted"
	
	Public Const ERR_UPDATEHISTORY As Integer = 350
	Public Const ERR_D_UPDATEHISTORY As String = "Failed to update Remote Database"
	
	Public Const ERR_CREATEPATH As Integer = 401
	Public Const ERR_D_CREATEPATH As String = "Failed to create File Path"
	
	Public Const ERR_COPYFILE As Integer = 402
	Public Const ERR_D_COPYFILE As String = "Failed to copy file"
	
	Public Const ERR_OPENFILE As Integer = 403
	Public Const ERR_D_OPENFILE As String = "Failed to open file"
	
	Public Const ERR_UNDEFINED As Integer = 2001
	Public Const ERR_D_UNDEFINED As String = "Undefined error in control file"
	
	Sub ErrorWAPI(ByRef iError As Integer, ByRef sControl As String)
		
		Dim sErrString As String = ""
		
		Try 
			
			If PutControlFileVar("DMSWAPI", "ReturnCode", "1", sControl) = PM_FALSE Then
				'we cant return the return code
			End If
			
			Select Case iError
				Case ERR_SUCCESS
					sErrString = ERR_D_SUCCESS
					
					If PutControlFileVar("DMSWAPI", "ReturnCode", "0", sControl) = PM_FALSE Then
						'we cant return the return code !!!
					End If
					
				Case ERR_FILENAME
					sErrString = ERR_D_FILENAME
					
				Case ERR_CANCEL
					sErrString = ERR_D_CANCEL
					
				Case ERR_INDEXCABINET
					sErrString = ERR_D_INDEXCABINET
					
				Case ERR_OPENDDB
					sErrString = ERR_D_OPENDDB
					
				Case ERR_LOGIN
					sErrString = ERR_D_LOGIN
					
				Case ERR_LOGOUT
					sErrString = ERR_D_LOGOUT
					
				Case ERR_COMMITDB
					sErrString = ERR_D_COMMITDB
					
				Case ERR_SYNCRODEVICES
					sErrString = ERR_D_SYNCRODEVICES
					
				Case ERR_NOTASK
					sErrString = ERR_D_NOTASK
					
				Case ERR_BADTASK
					sErrString = ERR_D_BADTASK
					
				Case ERR_NOFILENAME
					sErrString = ERR_D_NOFILENAME
					
				Case ERR_NOCABINET
					sErrString = ERR_D_NOCABINET
					
				Case ERR_NODRAWER
					sErrString = ERR_D_NODRAWER
					
				Case ERR_NOFOLDER
					sErrString = ERR_D_NOFOLDER
					
				Case ERR_NODOCUMENT
					sErrString = ERR_D_NODOCUMENT
					
				Case ERR_INDEXDATA
					sErrString = ERR_D_INDEXDATA
					
				Case ERR_INVALIDCABINET
					sErrString = ERR_D_INVAILDCABINET
					
				Case ERR_INVALIDDRAWER
					sErrString = ERR_D_INVALIDDRAWER
					
				Case ERR_INVALIDFOLDER
					sErrString = ERR_D_INVALIDFOLDER
					
				Case ERR_INVALIDDOCUMENT
					sErrString = ERR_D_INVALIDDOCUMENT
					
				Case ERR_CREATECABINET
					sErrString = ERR_D_CREATECABINET
					
				Case ERR_CREATEDRAWER
					sErrString = ERR_D_CREATEDRAWER
					
				Case ERR_CREATEFOLDER
					sErrString = ERR_D_CREATEFOLDER
					
				Case ERR_CREATEDOCUMENT
					sErrString = ERR_D_CREATEDOCUMENT
					
				Case ERR_CREATEPAGE
					sErrString = ERR_D_CREATEPAGE
					
				Case ERR_CREATEDOCINFO
					sErrString = ERR_D_CREATEDOCINFO
					
				Case ERR_CREATEANNOTATION
					sErrString = ERR_D_CREATEKEYWORD
					
				Case ERR_CREATEDOCLINK
					sErrString = ERR_D_CREATEDOCLINK
					
				Case ERR_GETCABINET
					sErrString = ERR_D_GETCABINET
					
				Case ERR_GETDRAWER
					sErrString = ERR_D_GETDRAWER
					
				Case ERR_GETFOLDER
					sErrString = ERR_D_GETFOLDER
					
				Case ERR_FULLCABINET
					sErrString = ERR_D_FULLCABINET
					
				Case ERR_FULLDRAWER
					sErrString = ERR_D_FULLDRAWER
					
				Case ERR_FULLFOLDER
					sErrString = ERR_D_FULLFOLDER
					
				Case ERR_UPDATEHISTORY
					sErrString = ERR_D_UPDATEHISTORY
					
					If PutControlFileVar("DMSWAPI", "ReturnCode", "0", sControl) = PM_FALSE Then
						'we cant return the return code !!!
					End If
					
				Case ERR_CREATEPATH
					sErrString = ERR_D_CREATEPATH
					
				Case ERR_COPYFILE
					sErrString = ERR_D_COPYFILE
					
				Case ERR_OPENFILE
					sErrString = ERR_D_OPENFILE
					
				Case Else
					sErrString = ERR_D_UNDEFINED
			End Select
			
			If PutControlFileVar("DMSWAPI", "ErrorCode", Conversion.Str(iError), sControl) = PM_FALSE Then
				'we cant return the error code...
			End If
			If PutControlFileVar("DMSWAPI", "ErrorMsg", sErrString, sControl) = PM_FALSE Then
				'we cant return the error message.
			End If
		
		Catch 
		End Try
		
		
		
		Exit Sub
		
	End Sub
End Module