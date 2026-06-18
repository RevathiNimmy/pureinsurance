Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10th September 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTCashList"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form Constants for Captions
	Public Const ACInterfaceCaption As Integer = 100
	Public Const ACMainTabTitle0 As Integer = 101
	Public Const ACBankCaption As Integer = 102
	Public Const ACTotalAmountCaption As Integer = 103
	Public Const ACCurrencyCaption As Integer = 104
	Public Const ACTotalItemsCaption As Integer = 105
	Public Const ACReferenceCaption As Integer = 106
	Public Const ACTypeCaption As Integer = 107
	Public Const ACStatusCaption As Integer = 108
	Public Const ACDateCaption As Integer = 109
	
	' Button Constants for Captions
	Public Const ACNavigateCaption As Integer = 200
	Public Const ACHelpCaption As Integer = 201
	Public Const ACCancelCaption As Integer = 202
	Public Const ACOKCaption As Integer = 203
	Public Const ACNextCaption0 As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACNoSelectionTitle As Integer = 304
	Public Const ACNoSelectionDetails As Integer = 305
	
	Public Const ACDrawerLockedTitle As Integer = 306
	Public Const ACDrawerLockedDetails As Integer = 307
	
	Public Const ACNotCreatedTitle As Integer = 308
	Public Const ACNotCreatedDetails As Integer = 309
	
	Public Const ACNotAuthorisedBankingTitle As Integer = 310
	Public Const ACNotAuthorisedBankingDetails As Integer = 311
	
	Public Const ACNoBankingItemsTitle As Integer = 312
	Public Const ACNoBankingItemsDetails As Integer = 313
	
	Public Const ACBankingNotAllowedTitle As Integer = 314
	Public Const ACBankingNotAllowedDetails As Integer = 315
	
	'SMJB 14/10/03: CQ2811 - Added
	Public Const ACDrawerSingleUserLockedTitle As Integer = 316
	Public Const ACDrawerSingleUserLockedDetails As Integer = 317
	
	'sw 10/12/2002 Currency Denom array elements
	Public Const ACCurrencyDenomID As Integer = 0
	Public Const ACCurrDenomDescription As Integer = 1
	Public Const ACCurrDenomValue As Integer = 2
	Public Const ACCurrDenomCode As Integer = 3
	
	'sw 10/12/2002 banking first and last tab
	Public Const ACFirstMediaTypeTab As Integer = 0
	Public Const ACLastMediaTypeTab As Integer = 15
	
	
	
	
	' Menus
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	Public g_iCurrencyID As Integer
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 14000
	
    '2005 Client Manager Security
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oUserAuthorities As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bRaiseCashAuthority As Boolean
	
	Public Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ParamArray ByVal r_vTokens() As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: DisplayMessage
		' PURPOSE: Displays a message based on passed resource file Ids
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 09 October 2002, 16:03:54
		' RETURNS: PMTrue for success
		' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		
		Try
		
		'Get the title from the res file

        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

        'Get the message from the res file

        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		'Replace Tokens in the message
		ReplaceTokens(sMessage, New Object(){r_vTokens})
		
		'Now display the message to the user
		result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
		End Select
		
		Finally
	
		
		End Try
		Return result
	End Function
	
	Public Function ReplaceTokens(ByRef r_sMessage As String, ParamArray ByVal r_vTokens() As Object) As Boolean
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ReplaceTokens
		' PURPOSE: Replace place holders with values
		'          e.g. convert |Username| to "Fred Bloggs"
		' AUTHOR: Paul Cunnigham
		' DATE: 21 October 2002, 10:04:52
		' RETURNS: True for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Boolean = False
		Dim lUpper As Integer
		Dim vToken, vValue As Object
		
		Dim vParams As Object
		
		
		
		'This routine could be called directly like...
		'    ReplaceTokens sMessage, "Usename", m sUserName
		'With the params explicitly listed
		
		'OR by a routine that itself accepts a ParamArray.
		'    ReplaceTokens sMessage, r_vParams
		
		'We need to ensure that we find the 'root' ParamArray as the second
		'calling method would pass Variant(0)(0), Variant(0)(1) into this routine
		'and we need Variant(0), Variant(1)
		

		Try 
			

            If CStr(r_vTokens(0)(0)) <> "" Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Find the 'root' paramarray
                '(i.e. convert  Variant(0)(0) to Variant(0))

                vParams = r_vTokens

                While vParams(0).GetType().Name = "Variant()"
                    If Information.Err().Number <> 0 Then
                        'No params passed at all
                        Information.Err().Clear()
                        Return result
                    End If


                    vParams = vParams(0)

                    If vParams.GetUpperBound(0) = -1 Then ' no params
                        Return result
                    End If
                End While


                'Any params actually passed?
                'We could just have a blank paramarray
                lUpper = r_vTokens.GetUpperBound(0)
                If lUpper <> -1 Then
                    'Loop through the param array
                    For iItem As Integer = 0 To lUpper \ 2
                        'Get the token and its replacement value


                        vToken = vParams(iItem * 2)
                        'This will bomb if developer has passed an odd number of params


                        vValue = vParams(iItem * 2 + 1)

                        'Replace the token with the value


                        r_sMessage = r_sMessage.Replace("|" & CStr(vToken) & "|", CStr(vValue))
                    Next
                End If
            End If
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			
			'----------------------------------------------------------------------------------------
			'Only for Debugging, the code will never execute this line
			'----------------------------------------------------------------------------------------


			

            
			

        Catch exc As System.Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceTokens", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=exc)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select
        End Try
        Return result
    End Function
	
	
	
	
	
	
	
	
	
    'developer guide no.33
    Public Function ListViewPopulate(ByRef r_lvwSource As ListView, ByRef r_vResultArray(,) As Object, ByRef r_bClearResultArray As Boolean, ByRef r_bClearSelectedItem As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ListViewPopulate
        ' PURPOSE: Populates a ListView from an Array
        '          (Ensure that your ListView has the same number of columns as the array)
        ' AUTHOR: Paul Cunnigham
        ' DATE: 21 October 2002, 15:57:43
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oListItem As ListViewItem


        Dim lLowerRow, lUpperRow, lLowerCol, lUpperCol As Integer


        Try

        result = gPMConstants.PMEReturnCode.PMFalse

        '----------------------------------------
        lLowerCol = r_vResultArray.GetLowerBound(0)
        lUpperCol = r_vResultArray.GetUpperBound(0)

        lLowerRow = r_vResultArray.GetLowerBound(1)
        lUpperRow = r_vResultArray.GetUpperBound(1)
        '----------------------------------------

        result = gPMConstants.PMEReturnCode.PMTrue

        'Clear the existing items
        r_lvwSource.Items.Clear()

        'Turn off listview updating
        ListView6Func.ListViewBatchStart(r_lvwSource)

        With r_lvwSource.Items
            'Loop through the results array and populate the listview
            For lRow As Integer = lLowerRow To lUpperRow

                oListItem = .Add(r_vResultArray(lLowerCol, lRow))
                With oListItem
                    For lColumn As Integer = lLowerCol + 1 To lUpperCol
                        ListViewHelper.GetListViewSubItem(oListItem, lColumn).Text = r_vResultArray(lColumn, lRow)
                    Next lColumn
                End With
            Next lRow
        End With

        'Turn on listview updating
        ListView6Func.ListViewBatchEnd()

        'Clear the selected item
        If r_bClearSelectedItem Then
            r_lvwSource.FocusedItem = Nothing
        End If

        'Clear the data from the array as it's now stored in the listview
        If r_bClearResultArray Then
            r_vResultArray = Nothing
        End If

        result = gPMConstants.PMEReturnCode.PMTrue


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
        

        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewPopulate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMError

        End Select

        Finally
        


        End Try
	Return result
    End Function
End Module
