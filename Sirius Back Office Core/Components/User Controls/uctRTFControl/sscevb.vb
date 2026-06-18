Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module SSCEVB
	' Sentry Spelling Checker Engine
	' SSCEVB: Helper functions and subroutines to make Sentry easier
	' to use from Visual Basic.
	'
	' Copyright (c) 1997-2006 Wintertree Software Inc.
	' www.wintertree-software.com
	'
	' Use, duplication, and disclosure of this file is governed by
	' a license agreement between Wintertree Software Inc. and
	' the licensee.
	'
	' $Id: sscevb.bas,v 5.16 2006/07/14 17:53:23 wsi Exp wsi $
	
	
	Private Function CStringToVBString(ByRef cstring As String) As String
		
		Dim nullPos As Integer = (cstring.IndexOf(Strings.Chr(0).ToString()) + 1)
		If nullPos <> 0 Then
			Return cstring.Substring(0, nullPos - 1)
		Else
			Return cstring
		End If
	End Function
	
	Sub SSCEVB_GetSelUserLexFile(ByRef fileName As String)
		
		Dim fn As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetSelUserLexFile(fn, CShort(fn.Length))
		fileName = CStringToVBString(fn)
	End Sub
	
	Sub SSCEVB_GetMainLexFiles(ByRef fileList As String)
		
		Dim fl As String = New String(Strings.Chr(0).ToString(), 512)
		SSCE_GetMainLexFiles(fl, CShort(fl.Length))
		fileList = CStringToVBString(fl)
	End Sub
	
	Function SSCEVB_CheckBlockDlg(ByVal parent As Integer, ByRef block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal showContext As Integer) As Integer
		
		' 1 extra character is required to hold the terminating null.
		' VB automatically appends a terminating null for parameters
		' in DLL functions typed "byval string", so it's safe to
		' specify the block size as 1 larger than the block length.
		If blkSz = blkLen Then
			blkSz += 1
		End If
		Dim n As Integer = SSCE_CheckBlockDlg(parent, block, blkLen, blkSz, CShort(showContext))
		If n >= 0 Then
			block = block.Substring(0, n)
		End If
		Return n
	End Function
	
	Function SSCEVB_CheckBlockDlgTmplt(ByVal parent As Integer, ByRef block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal showContext As Integer, ByVal clientInst As Integer, ByVal spellDlgTmplt As String, ByVal dictDlgTmplt As String, ByVal optDlgTmplt As String, ByVal newLexDlgTmplt As String) As Integer
		
		' 1 extra character is required to hold the terminating null.
		' VB automatically appends a terminating null for parameters
		' in DLL functions typed "byval string", so it's safe to
		' specify the block size as 1 larger than the block length.
		If blkSz = blkLen Then
			blkSz += 1
		End If
		Dim n As Integer = SSCE_CheckBlockDlgTmplt(parent, block, blkLen, blkSz, CShort(showContext), clientInst, spellDlgTmplt, dictDlgTmplt, optDlgTmplt, newLexDlgTmplt)
		If n >= 0 Then
			block = block.Substring(0, n)
		End If
		Return n
	End Function
	
	Function SSCEVB_CheckCtrlBackground(ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer
		Return SSCE_CheckCtrlBackground(ctrlWin, options, markColor)
	End Function
	
	Function SSCEVB_CheckCtrlBackgroundClear(ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer
		Return SSCE_CheckCtrlBackgroundClear(ctrlWin, options, markColor)
	End Function
	
	Function SSCEVB_CheckCtrlBackgroundMenu(ByVal ctrlWin As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer
		Return SSCE_CheckCtrlBackgroundMenu(ctrlWin, X, Y, options, markColor)
	End Function
	
	Function SSCEVB_CheckCtrlBackgroundNotify(ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer
		Return SSCE_CheckCtrlBackgroundNotify(ctrlWin, options, markColor)
	End Function
	
	Function SSCEVB_CheckCtrlBackgroundRecheckAll(ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer
		Return SSCE_CheckCtrlBackgroundRecheckAll(ctrlWin, options, markColor)
	End Function
	
	Function SSCEVB_CheckCtrlDlg(ByVal parent As Integer, ByVal ctrlWin As Integer, ByVal selTextOnly As Integer) As Integer
		Return SSCE_CheckCtrlDlg(parent, ctrlWin, CShort(selTextOnly))
	End Function
	
	Function SSCEVB_CheckCtrlDlgTmplt(ByVal parent As Integer, ByVal ctrlWin As Integer, ByVal selTextOnly As Integer, ByVal clientInst As Integer, ByVal spellDlgTmplt As String, ByVal dictDlgTmplt As String, ByVal optDlgTmplt As String, ByVal newLexDlgTmplt As String) As Integer
		Return SSCE_CheckCtrlDlgTmplt(parent, ctrlWin, CShort(selTextOnly), clientInst, spellDlgTmplt, dictDlgTmplt, optDlgTmplt, newLexDlgTmplt)
	End Function
	
	Function SSCEVB_EditLexDlg(ByVal parent As Integer) As Integer
		Return SSCE_EditLexDlg(parent)
	End Function
	
	Function SSCEVB_EditLexDlgTmplt(ByVal parent As Integer, ByVal clientInst As Integer, ByVal dictDlgTmplt As String, ByVal newLexDlgTmplt As String) As Integer
		Return SSCE_EditLexDlgTmplt(parent, clientInst, dictDlgTmplt, newLexDlgTmplt)
	End Function
	
	Function SSCEVB_GetAutoCorrect() As Integer
		Return SSCE_GetAutoCorrect()
	End Function
	
	Function SSCEVB_GetMinSuggestDepth() As Integer
		Return SSCE_GetMinSuggestDepth()
	End Function
	
	Sub SSCEVB_GetHelpFile(ByRef fileName As String)
		
		Dim fn As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetHelpFile(fn, CShort(fn.Length))
		fileName = CStringToVBString(fn)
	End Sub
	
	Sub SSCEVB_GetIniFile(ByRef fileName As String)
		
		Dim fn As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetIniFile(fn, CShort(fn.Length))
		fileName = CStringToVBString(fn)
	End Sub
	
	Function SSCEVB_GetLexId(ByVal lexFileName As String) As Integer
		Return SSCE_GetLexId(lexFileName)
	End Function
	
	Sub SSCEVB_GetMainLexPath(ByRef path As String)
		
		Dim p As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetMainLexPath(p, CShort(p.Length))
		path = CStringToVBString(p)
	End Sub
	
	Function SSCEVB_GetSid() As Integer
		Return SSCE_GetSid()
	End Function
	
	Sub SSCEVB_GetStatistics(ByRef wordsChecked As Integer, ByRef wordsChanged As Integer, ByRef errorsDetected As Integer)
		SSCE_GetStatistics(wordsChecked, wordsChanged, errorsDetected)
	End Sub
	
	Sub SSCEVB_GetRegTreeName(ByRef regTreeName As String)
		
		Dim tn As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetRegTreeName(tn, CShort(tn.Length))
		regTreeName = CStringToVBString(tn)
	End Sub
	
	Sub SSCEVB_GetStringTableName(ByRef tableName As String)
		
		Dim tn As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetStringTableName(tn, CShort(tn.Length))
		tableName = CStringToVBString(tn)
	End Sub
	
	Sub SSCEVB_GetUserLexPath(ByRef path As String)
		
		Dim p As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetUserLexPath(p, CShort(p.Length))
		path = CStringToVBString(p)
	End Sub
	
	Function SSCEVB_OpenBlock(ByVal sid As Integer, ByVal block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal copyBlock As Integer) As Integer
		If blkSz = blkLen Then
			' 1 extra character is required to hold the terminating null.
			' VB automatically appends a terminating null for parameters
			' in DLL functions typed "byval string", so it's safe to
			' specify the block size as 1 larger than the block length.
			blkSz = blkLen + 1
		End If
		Return SSCE_OpenBlock(CShort(sid), block, blkLen, blkSz, CShort(copyBlock))
	End Function
	
	Function SSCEVB_GetLexInfo(ByVal sid As Integer, ByVal lexId As Integer, ByRef lexSz As Integer, ByRef lexFormat As Integer, ByRef lang As Integer) As Integer
		Return SSCE_GetLexInfo(CShort(sid), CShort(lexId), lexSz, CShort(lexFormat), CShort(lang))
	End Function
	
	Function SSCEVB_CloseBlock(ByVal sid As Integer, ByVal blkId As Integer) As Integer
		Return SSCE_CloseBlock(CShort(sid), CShort(blkId))
	End Function
	
	Function SSCEVB_CheckWord(ByVal sid As Integer, ByVal word As String, ByRef otherWord As String) As Integer
		
		Dim result As Integer = 0
		Dim ow As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_CheckWord(CShort(sid), word, ow, CShort(ow.Length))
		otherWord = CStringToVBString(ow)
		Return result
	End Function
	
	Function SSCEVB_CheckBlock(ByVal sid As Integer, ByVal blkId As Integer, ByRef errWord As String, ByRef otherWord As String) As Integer
		
		Dim result As Integer = 0
		Dim ew As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		Dim ow As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_CheckBlock(CShort(sid), CShort(blkId), ew, CShort(ew.Length), ow, CShort(ow.Length))
		errWord = CStringToVBString(ew)
		otherWord = CStringToVBString(ow)
		Return result
	End Function
	
	Function SSCEVB_AddToLex(ByVal sid As Integer, ByVal lexId As Integer, ByVal word As String, ByVal action As Integer, ByVal otherWord As String) As Integer
		Return SSCE_AddToLex(CShort(sid), CShort(lexId), word, CShort(action), otherWord)
	End Function
	
	Function SSCEVB_ClearLex(ByVal sid As Integer, ByVal lexId As Integer) As Integer
		Return SSCE_ClearLex(CShort(sid), CShort(lexId))
	End Function
	
	Function SSCEVB_CloseLex(ByVal sid As Integer, ByVal lexId As Integer) As Integer
		Return SSCE_CloseLex(CShort(sid), CShort(lexId))
	End Function
	
	Function SSCEVB_CloseSession(ByVal sid As Integer) As Integer
		Return SSCE_CloseSession(CShort(sid))
	End Function
	
	Sub SSCEVB_CompressLexAbort(ByVal sid As Integer)
		SSCE_CompressLexAbort(CShort(sid))
	End Sub
	
	Function SSCEVB_CompressLexEnd(ByVal sid As Integer) As Integer
		Return SSCE_CompressLexEnd(CShort(sid))
	End Function
	
	Function SSCEVB_CompressLexFile(ByVal sid As Integer, ByVal fileName As String, ByRef errLine As Integer) As Integer
		Return SSCE_CompressLexFile(CShort(sid), fileName, errLine)
	End Function
	
	Function SSCEVB_CompressLexInit(ByVal sid As Integer, ByVal lexFileName As String, ByVal suffixFileName As String, ByVal langId As Integer, ByRef errLine As Integer) As Integer
		Return SSCE_CompressLexInit(CShort(sid), lexFileName, suffixFileName, CShort(langId), errLine)
	End Function
	
	Function SSCEVB_CreateLex(ByVal sid As Integer, ByVal fileName As String, ByVal lang As Integer) As Integer
		Return SSCE_CreateLex(CShort(sid), fileName, CShort(lang))
	End Function
	
	Function SSCEVB_DelBlockText(ByVal sid As Integer, ByVal blkId As Integer, ByVal numChars As Integer) As Integer
		Return SSCE_DelBlockText(CShort(sid), CShort(blkId), numChars)
	End Function
	
	Function SSCEVB_DelBlockWord(ByVal sid As Integer, ByVal blkId As Integer, ByRef delText As String) As Integer
		
		Dim result As Integer = 0
		Dim dt As String = New String(Strings.Chr(0).ToString(), 256)
		result = SSCE_DelBlockWord(CShort(sid), CShort(blkId), dt, CShort(dt.Length))
		delText = CStringToVBString(dt)
		Return result
	End Function
	
	Function SSCEVB_DelFromLex(ByVal sid As Integer, ByVal lexId As Integer, ByVal word As String) As Integer
		Return SSCE_DelFromLex(CShort(sid), CShort(lexId), word)
	End Function
	
	Function SSCEVB_GetBlock(ByVal sid As Integer, ByVal blkId As Integer, ByRef block As String) As Integer
		Dim blkLen, blkSz As Integer
		
		SSCE_GetBlockInfo(CShort(sid), CShort(blkId), blkLen, blkSz, 0, 0)
		Dim b As String = New String(Strings.Chr(0).ToString(), blkSz)
		SSCE_GetBlock(CShort(sid), CShort(blkId), b, blkSz)
		block = b.Substring(0, blkLen)
		Return blkLen
	End Function
	
	Function SSCEVB_GetBlockInfo(ByVal sid As Integer, ByVal blkId As Integer, ByRef blkLen As Integer, ByRef blkSz As Integer, ByRef curPos As Integer, ByRef wordCount As Integer) As Integer
		Return SSCE_GetBlockInfo(CShort(sid), CShort(blkId), blkLen, blkSz, curPos, wordCount)
	End Function
	
	Function SSCEVB_GetBlockWord(ByVal sid As Integer, ByVal blkId As Integer, ByRef word As String) As Integer
		
		Dim result As Integer = 0
		Dim w As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_GetBlockWord(CShort(sid), CShort(blkId), w, CShort(w.Length))
		word = CStringToVBString(w)
		Return result
	End Function
	
	Function SSCEVB_GetLex(ByVal sid As Integer, ByVal lexId As Integer, ByRef lexBfr As String) As Integer
		Dim lexBfrSz As Integer
		
		SSCE_GetLexInfo(CShort(sid), CShort(lexId), lexBfrSz, 0, 0)
		Dim lb As String = New String(Strings.Chr(0).ToString(), lexBfrSz)
		SSCE_GetLex(CShort(sid), CShort(lexId), lb, lb.Length)
		lexBfr = lb.Substring(0, lexBfrSz)
		Return lexBfrSz
	End Function
	
	Function SSCEVB_GetCharSet() As Integer
		Return SSCE_GetCharSet()
	End Function
	
	Function SSCEVB_GetOption(ByVal sid As Integer, ByVal opt As Integer) As Integer
		Return SSCE_GetOption(CShort(sid), opt)
	End Function
	
	Function SSCEVB_InsertBlockText(ByVal sid As Integer, ByVal blkId As Integer, ByVal text As String) As Integer
		Return SSCE_InsertBlockText(CShort(sid), CShort(blkId), text)
	End Function
	
	Function SSCEVB_NextBlockWord(ByVal sid As Integer, ByVal blkId As Integer) As Integer
		Return SSCE_NextBlockWord(CShort(sid), CShort(blkId))
	End Function
	
	Function SSCEVB_OpenLex(ByVal sid As Integer, ByVal fileName As String, ByVal memBudget As Integer) As Integer
		Return SSCE_OpenLex(CShort(sid), fileName, memBudget)
	End Function
	
	Function SSCEVB_OpenSession() As Integer
		Return SSCE_OpenSession()
	End Function
	
	Function SSCEVB_OptionsDlg(ByVal parent As Integer) As Integer
		Return SSCE_OptionsDlg(parent)
	End Function
	
	Function SSCEVB_OptionsDlgTmplt(ByVal parent As Integer, ByVal clientInst As Integer, ByVal optDlgTmplt As String) As Integer
		Return SSCE_OptionsDlgTmplt(parent, clientInst, optDlgTmplt)
	End Function
	
	Function SSCEVB_ReplaceBlockWord(ByVal sid As Integer, ByVal blkId As Integer, ByVal word As String) As Integer
		Return SSCE_ReplaceBlockWord(CShort(sid), CShort(blkId), word)
	End Function
	
	Function SSCEVB_ResetLex() As Integer
		Return SSCE_ResetLex()
	End Function
	
	Function SSCEVB_SetAutoCorrect(ByVal autoCorrect As Integer) As Integer
		Return SSCE_SetAutoCorrect(CShort(autoCorrect))
	End Function
	
	Function SSCEVB_SetMinSuggestDepth(ByVal depth As Integer) As Integer
		Return SSCE_SetMinSuggestDepth(CShort(depth))
	End Function
	
	Function SSCEVB_SetBlockCursor(ByVal sid As Integer, ByVal blkId As Integer, ByVal cursor As Integer) As Integer
		Return SSCE_SetBlockCursor(CShort(sid), CShort(blkId), cursor)
	End Function
	
	Function SSCEVB_SetCharSet(ByVal charSet As Integer) As Integer
		Return SSCE_SetCharSet(charSet)
	End Function
	
	Sub SSCEVB_SetDebugFile(ByVal debugFile As String)
		SSCE_SetDebugFile(debugFile)
	End Sub
	
	Sub SSCEVB_SetDialogOrigin(ByVal X As Integer, ByVal Y As Integer)
		SSCE_SetDialogOrigin(CShort(X), CShort(Y))
	End Sub
	
	Function SSCEVB_SetHelpFile(ByVal helpFile As String) As Integer
		Return SSCE_SetHelpFile(helpFile)
	End Function
	
	Function SSCEVB_SetIniFile(ByVal iniFile As String) As Integer
		Return SSCE_SetIniFile(iniFile)
	End Function
	
	Function SSCEVB_SetMainLexFiles(ByVal fileList As String) As Integer
		Return SSCE_SetMainLexFiles(fileList)
	End Function
	
	Function SSCEVB_SetMainLexPath(ByVal path As String) As Integer
		Return SSCE_SetMainLexPath(path)
	End Function
	
	Function SSCEVB_SetOption(ByVal sid As Integer, ByVal opt As Integer, ByVal optVal As Integer) As Integer
		Return SSCE_SetOption(CShort(sid), opt, optVal)
	End Function
	
	Function SSCEVB_SetRegTreeName(ByVal regTreeName As String) As Integer
		Return SSCE_SetRegTreeName(regTreeName)
	End Function
	
	Function SSCEVB_SetSelUserLexFile(ByRef fileName As String) As Integer
		Return SSCE_SetSelUserLexFile(fileName)
	End Function
	
	Function SSCEVB_SetStringTableName(ByVal tableName As String) As Integer
		Return SSCE_SetStringTableName(tableName)
	End Function
	
	Function SSCEVB_SetUserLexFiles(ByVal fileList As String) As Integer
		Return SSCE_SetUserLexFiles(fileList)
	End Function
	
	Function SSCEVB_SetUserLexPath(ByVal path As String) As Integer
		Return SSCE_SetUserLexPath(path)
	End Function
	
	Function SSCEVB_Suggest(ByVal sid As Integer, ByVal word As String, ByVal depth As Integer, ByRef suggestions() As String, ByRef scores() As Integer, ByVal nSuggestions As Integer) As Integer
		Dim result As Integer = 0
		Dim e As Integer
		
		Dim suggBfr As String = New String(Strings.Chr(0).ToString(), nSuggestions * SSCE_MAX_WORD_SZ)
		result = SSCE_Suggest(CShort(sid), word, CShort(depth), suggBfr, suggBfr.Length, CShort(scores(0)), CShort(nSuggestions))
		
		' Copy each null-terminated suggestion to the suggestions array.
		Dim s As Integer = 1
		Dim i As Integer = 0
		Dim done As Boolean = False
		While Not done
			e = Strings.InStr(s, suggBfr, Strings.Chr(0).ToString())
			If e <> 0 And e > s Then
				suggestions(i) = Mid(suggBfr, s, e - s)
				i += 1
				s = e + 1
			Else
				done = True
			End If
		End While
		Return result
	End Function
	
	Sub SSCEVB_Version(ByRef version As String)
		
		Dim vs As String = New String(Strings.Chr(0).ToString(), 20)
		SSCE_Version(vs, CShort(vs.Length))
		version = CStringToVBString(vs)
	End Sub
	
	Public Sub SSCEVB_GetUserLexFiles(ByRef files As String)
		
		Dim p As String = New String(Strings.Chr(0).ToString(), 256)
		SSCE_GetUserLexFiles(p, CShort(p.Length))
		files = CStringToVBString(p)
	End Sub
	
	Public Function SSCEVB_SetKey(ByVal key As Integer) As Integer
		Return SSCE_SetKey(key)
	End Function
	
	Public Function SSCEVB_CheckString(ByVal sid As Integer, ByVal str As String, ByRef cursor As Integer, ByRef errWord As String, ByRef otherWord As String) As Integer
		
		Dim result As Integer = 0
		Dim w As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		Dim ow As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_CheckString(CShort(sid), str, cursor, w, CShort(w.Length), ow, CShort(ow.Length))
		errWord = CStringToVBString(w)
		otherWord = CStringToVBString(ow)
		Return result
	End Function
	
	Public Function SSCEVB_DelStringText(ByVal sid As Integer, ByRef str As String, ByVal cursor As Integer, ByVal numChars As Integer) As Integer
		Dim result As Integer = 0
		result = SSCE_DelStringText(CShort(sid), str, cursor, numChars)
		str = CStringToVBString(str)
		Return result
	End Function
	
	Public Function SSCEVB_DelStringWord(ByVal sid As Integer, ByRef str As String, ByVal cursor As Integer, ByRef delText As String) As Integer
		
		Dim result As Integer = 0
		Dim dt As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ * 2)
		result = SSCE_DelStringWord(CShort(sid), str, cursor, dt, dt.Length)
		delText = CStringToVBString(dt)
		str = CStringToVBString(str)
		Return result
	End Function
	
	Public Function SSCEVB_GetStringWord(ByVal sid As Integer, ByRef str As String, ByVal cursor As Integer, ByRef word As String) As Integer
		
		Dim result As Integer = 0
		Dim w As String = New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_GetStringWord(CShort(sid), str, cursor, w, CShort(w.Length))
		word = CStringToVBString(w)
		Return result
	End Function
	
	Public Function SSCEVB_InsertStringText(ByVal sid As Integer, ByRef str As String, ByVal cursor As Integer, ByVal text As String) As Integer
		
		Dim result As Integer = 0
		Dim s As String = str & New String(Strings.Chr(0).ToString(), text.Length + 1)
		result = SSCE_InsertStringText(CShort(sid), s, s.Length, cursor, text)
		str = CStringToVBString(s)
		Return result
	End Function
	
	Public Function SSCEVB_ReplaceStringWord(ByVal sid As Integer, ByRef str As String, ByVal cursor As Integer, ByVal word As String) As Integer
		
		Dim result As Integer = 0
		Dim s As String = str & New String(Strings.Chr(0).ToString(), SSCE_MAX_WORD_SZ)
		result = SSCE_ReplaceStringWord(CShort(sid), s, s.Length, cursor, word)
		str = CStringToVBString(s)
		Return result
	End Function
	
	Public Function SSCEVB_CountStringWords(ByVal sid As Integer, ByRef str As String) As Integer
		Return SSCE_CountStringWords(CShort(sid), str)
	End Function
	
	Public Function SSCEVB_SyncLex(ByVal sid As Integer, ByVal lexId As Integer) As Integer
		Return SSCE_SyncLex(CShort(sid), CShort(lexId))
	End Function
End Module