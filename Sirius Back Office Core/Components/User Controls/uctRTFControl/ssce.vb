Option Strict Off
Option Explicit On
Imports System
Module SSCE
	' Sentry Spelling Checker Engine
	' ssce.vb: VB declarations for Sentry DLL API
	' Copyright (c) 1994-2006 Wintertree Software Inc.
	' www.wintertree-software.com
	'
	' Use, duplication, and disclosure of this file is governed by
	' a license agreement between Wintertree Software Inc. and
	' the licensee.
	'
	' $Id: ssce.bas,v 5.16 2006/07/14 17:53:23 wsi Exp wsi $
	
	Public Const SSCE_MAX_WORD_LEN As Integer = 80
	Public Const SSCE_MAX_WORD_SZ As Integer = (SSCE_MAX_WORD_LEN + 1)
	Public Const SSCE_MAX_SUGGEST_DEPTH As Integer = 100
	
	' Error codes. Note that these are always negative.
	Public Const SSCE_BAD_SESSION_ID_ERR As Integer = (-2)
	Public Const SSCE_WORD_NOT_FOUND_ERR As Integer = (-3)
	Public Const SSCE_FILE_NOT_FOUND_ERR As Integer = (-4)
	Public Const SSCE_UNKNOWN_ACTION_ERR As Integer = (-6)
	Public Const SSCE_BAD_LEXICON_ID_ERR As Integer = (-7)
	Public Const SSCE_BUFFER_TOO_SMALL_ERR As Integer = (-8)
	Public Const SSCE_READ_ONLY_LEXICON_ERR As Integer = (-9)
	Public Const SSCE_OUT_OF_MEMORY_ERR As Integer = (-12)
	Public Const SSCE_UNSUPPORTED_ACTION_ERR As Integer = (-13)
	Public Const SSCE_LEXICON_EXISTS_ERR As Integer = (-14)
	Public Const SSCE_BAD_BLOCK_ID_ERR As Integer = (-16)
	Public Const SSCE_CANCEL_ERR As Integer = (-17)
	Public Const SSCE_INVALID_WORD_ERR As Integer = (-19)
	Public Const SSCE_WORD_OUT_OF_SEQUENCE_ERR As Integer = (-20)
	Public Const SSCE_FILE_READ_ERR As Integer = (-22)
	Public Const SSCE_FILE_WRITE_ERR As Integer = (-23)
	Public Const SSCE_FILE_OPEN_ERR As Integer = (-24)
	Public Const SSCE_BUSY_ERR As Integer = (-25)
	Public Const SSCE_UNKNOWN_LEX_FORMAT_ERR As Integer = (-26)
	
	' Spell-check result masks. Note that these will always result in a
	' positive value.
	Public Const SSCE_OK_RSLT As Integer = &H0s
	Public Const SSCE_MISSPELLED_WORD_RSLT As Integer = &H1s
	Public Const SSCE_AUTO_CHANGE_WORD_RSLT As Integer = &H2s
	Public Const SSCE_CONDITIONALLY_CHANGE_WORD_RSLT As Integer = &H4s
	Public Const SSCE_UNCAPPED_WORD_RSLT As Integer = &H8s
	Public Const SSCE_MIXED_CASE_WORD_RSLT As Integer = &H10s
	Public Const SSCE_MIXED_DIGITS_WORD_RSLT As Integer = &H20s
	Public Const SSCE_END_OF_BLOCK_RSLT As Integer = &H40s
	Public Const SSCE_END_OF_TEXT_RSLT As Integer = SSCE_END_OF_BLOCK_RSLT
	Public Const SSCE_DOUBLED_WORD_RSLT As Integer = &H80s
	
	' Options.
	Public Const SSCE_IGNORE_CAPPED_WORD_OPT As Integer = &H1
	Public Const SSCE_IGNORE_MIXED_CASE_OPT As Integer = &H2
	Public Const SSCE_IGNORE_MIXED_DIGITS_OPT As Integer = &H4
	Public Const SSCE_IGNORE_ALL_CAPS_WORD_OPT As Integer = &H8
	Public Const SSCE_REPORT_UNCAPPED_OPT As Integer = &H10
	Public Const SSCE_REPORT_MIXED_CASE_OPT As Integer = &H20
	Public Const SSCE_REPORT_MIXED_DIGITS_OPT As Integer = &H40
	Public Const SSCE_REPORT_SPELLING_OPT As Integer = &H80
	Public Const SSCE_REPORT_DOUBLED_WORD_OPT As Integer = &H100
	Public Const SSCE_CASE_SENSITIVE_OPT As Integer = &H200
	Public Const SSCE_SPLIT_HYPHENATED_WORDS_OPT As Integer = &H400
	Public Const SSCE_SPLIT_CONTRACTED_WORDS_OPT As Integer = &H800
	Public Const SSCE_SPLIT_WORDS_OPT As Integer = &H1000
	Public Const SSCE_SUGGEST_SPLIT_WORDS_OPT As Integer = &H2000
	Public Const SSCE_SUGGEST_PHONETIC_OPT As Integer = &H4000
	Public Const SSCE_SUGGEST_TYPOGRAPHICAL_OPT As Integer = &H8000
	Public Const SSCE_STRIP_POSSESSIVES_OPT As Integer = &H10000
	Public Const SSCE_IGNORE_NON_ALPHA_WORD_OPT As Integer = &H20000
	Public Const SSCE_IGNORE_DOMAIN_NAME_OPT As Integer = &H40000
	Public Const SSCE_ALLOW_ACCENTED_CAPS_OPT As Integer = &H80000
	Public Const SSCE_IGNORE_HTML_MARKUPS_OPT As Integer = &H200000
	Public Const SSCE_BACKUP_LEX_OPT As Integer = &H400000
	Public Const SSCE_INCLUDE_AMPERSAND_OPT As Integer = &H800000
	Public Const SSCE_INCLUDE_SLASH_OPT As Integer = &H1000000
	Public Const SSCE_LANGUAGE_OPT As Integer = &H80000002
	
	' Lexicon formats:
	Public Const SSCE_COMPRESSED_LEX_FMT As Integer = 0
	Public Const SSCE_TEXT_LEX_FMT As Integer = 1
	
	' Actions associated with words in text lexicons.
	Public Const SSCE_AUTO_CHANGE_ACTION As Integer = 97 ' "a"
	Public Const SSCE_AUTO_CHANGE_PRESERVE_CASE_ACTION As Integer = 65 ' "A"
	Public Const SSCE_CONDITIONAL_CHANGE_ACTION As Integer = 99 ' "c"
	Public Const SSCE_CONDITIONAL_CHANGE_PRESERVE_CASE_ACTION As Integer = 67 ' "C"
	Public Const SSCE_EXCLUDE_ACTION As Integer = 101 ' "e"
	Public Const SSCE_IGNORE_ACTION As Integer = 105 ' "i"
	
	' Language ids:
	Public Const SSCE_ANY_LANG As Integer = 30840 ' "xx"
	Public Const SSCE_AMER_ENGLISH_LANG As Integer = 24941 ' "am"
	Public Const SSCE_BRIT_ENGLISH_LANG As Integer = 25202 ' "br"
	Public Const SSCE_CANADIAN_ENGLISH_LANG As Integer = 25441 ' "ca"
	Public Const SSCE_CATALAN_LANG As Integer = 29539 ' "sc"
	Public Const SSCE_CZECH_LANG As Integer = 25466 ' "cz"
	Public Const SSCE_DANISH_LANG As Integer = 25697 ' "da"
	Public Const SSCE_DUTCH_LANG As Integer = 25717 ' "du"
	Public Const SSCE_FINNISH_LANG As Integer = 26217 ' "fi"
	Public Const SSCE_FRENCH_LANG As Integer = 26226 ' "fr"
	Public Const SSCE_GERMAN_LANG As Integer = 26469 ' "ge"
	Public Const SSCE_GERMAN_NEW_LANG As Integer = 26478 ' "gn"
	Public Const SSCE_GERMAN_OLD_LANG As Integer = 26479 ' "go"
	Public Const SSCE_HUNGARIAN_LANG As Integer = 26741 ' "hu"
	Public Const SSCE_ITALIAN_LANG As Integer = 26996 ' "it"
	Public Const SSCE_NORWEGIAN_BOKMAL_LANG As Integer = 25442 ' "cb"
	Public Const SSCE_NORWEGIAN_NYNORSK_LANG As Integer = 25444 ' "cd"
	Public Const SSCE_POLISH_LANG As Integer = 28780 ' "pl"
	Public Const SSCE_PORTUGUESE_BRAZIL_LANG As Integer = 28770 ' "pb"
	Public Const SSCE_PORTUGUESE_IBERIAN_LANG As Integer = 28783 ' "po"
	Public Const SSCE_RUSSIAN_LANG As Integer = 29301 ' "ru"
	Public Const SSCE_SPANISH_LANG As Integer = 29552 ' "sp"
	Public Const SSCE_SWEDISH_LANG As Integer = 29559 ' "sw"
	
	' Character set types used in compressed lexicons.
	Public Const SSCE_ISO8859_CHARSET As Integer = 1
	Public Const SSCE_UNICODE_CHARSET As Integer = 2
	
	' SSCE_CheckCtrlBackground options.
	Public Const SSCE_BACKGROUND_MARK_COLOR As Integer = &H1
	Public Const SSCE_BACKGROUND_MARK_BOLD As Integer = &H2
	Public Const SSCE_BACKGROUND_MARK_ITALICS As Integer = &H4
	Public Const SSCE_BACKGROUND_MARK_UNDERLINE As Integer = &H8
	Public Const SSCE_BACKGROUND_CONTEXT_MENU As Integer = &H10
	Public Const SSCE_BACKGROUND_CHECK_ALL As Integer = &H20
	Public Const SSCE_BACKGROUND_MARK_STRIKETHROUGH As Integer = &H40

	' Functions in the core Sentry API

	Declare Function SSCE_AddToLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short, ByVal word As String, ByVal action As Short, ByVal otherWord As String) As Short

	Declare Function SSCE_CheckBlock Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal errWord As String, ByVal errWordSz As Short, ByVal otherWord As String, ByVal otherWordSz As Short) As Short

	Declare Function SSCE_CheckString Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByRef cursor As Integer, ByVal errWord As String, ByVal errWordSz As Short, ByVal otherWord As String, ByVal otherWordSz As Short) As Short

	Declare Function SSCE_CheckWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal word As String, ByVal otherWord As String, ByVal otherWordSz As Short) As Short

	Declare Function SSCE_ClearLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short) As Short

	Declare Function SSCE_CloseBlock Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short) As Short

	Declare Function SSCE_CloseLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short) As Short

	Declare Function SSCE_CloseSession Lib "ssce5564.dll" (ByVal sid As Short) As Short

	Declare Sub SSCE_CompressLexAbort Lib "ssce5564.dll" (ByVal sid As Short)

	Declare Function SSCE_CompressLexEnd Lib "ssce5564.dll" (ByVal sid As Short) As Short

	Declare Function SSCE_CompressLexFile Lib "ssce5564.dll" (ByVal sid As Short, ByVal fileName As String, ByRef errLine As Integer) As Short

	Declare Function SSCE_CompressLexInit Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexFileName As String, ByVal suffixFileName As Object, ByVal langId As Short, ByRef errLine As Integer) As Short

	Declare Function SSCE_CountStringWords Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String) As Integer

	Declare Function SSCE_CreateLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal fileName As String, ByVal lang As Short) As Short

	Declare Function SSCE_DelBlockText Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal numChars As Integer) As Short

	Declare Function SSCE_DelBlockWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal delText As String, ByVal delTextSz As Short) As Integer

	Declare Function SSCE_DelFromLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short, ByVal word As String) As Short

	Declare Function SSCE_DelStringText Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByVal cursor As Integer, ByVal numChars As Integer) As Integer

	Declare Function SSCE_DelStringWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByVal cursor As Integer, ByVal delText As String, ByVal delTextSz As Integer) As Integer

	Declare Function SSCE_FindLexWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short, ByVal word As String, ByVal otherWord As String, ByVal otherWordSz As Short) As Short

	Declare Function SSCE_GetBlock Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal block As String, ByVal blkSz As Integer) As Integer

	Declare Function SSCE_GetBlockInfo Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByRef blkLen As Integer, ByRef blkSz As Integer, ByRef curPos As Integer, ByRef wordCount As Integer) As Short

	Declare Function SSCE_GetBlockWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal word As String, ByVal wordSz As Short) As Short

	Declare Function SSCE_GetCharSet Lib "ssce5564.dll" () As Integer

	Declare Function SSCE_GetLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short, ByVal lexBfr As String, ByVal lexBfrSz As Integer) As Integer

	Declare Function SSCE_GetLexInfo Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short, ByRef lexSz As Integer, ByRef lexFormat As Short, ByRef lang As Short) As Short

	Declare Function SSCE_GetOption Lib "ssce5564.dll" (ByVal sid As Short, ByVal opt As Integer) As Integer

	Declare Function SSCE_GetStringWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByVal cursor As Integer, ByVal word As String, ByVal wordSz As Short) As Integer

	Declare Function SSCE_InsertBlockText Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal text As String) As Short

	Declare Function SSCE_InsertStringText Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByVal strSz As Integer, ByVal cursor As Integer, ByVal text As String) As Integer

	Declare Function SSCE_NextBlockWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short) As Short

	Declare Function SSCE_OpenBlock Lib "ssce5564.dll" (ByVal sid As Short, ByVal block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal copyBlock As Short) As Short

	Declare Function SSCE_OpenLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal fileName As String, ByVal memBudget As Integer) As Short

	Declare Function SSCE_OpenSession Lib "ssce5564.dll" () As Short

	Declare Function SSCE_ReplaceBlockWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal word As String) As Short

	Declare Function SSCE_ReplaceStringWord Lib "ssce5564.dll" (ByVal sid As Short, ByVal str As String, ByVal strSz As Integer, ByVal cursor As Integer, ByVal word As String) As Integer

	Declare Function SSCE_SetBlockCursor Lib "ssce5564.dll" (ByVal sid As Short, ByVal blkId As Short, ByVal cursor As Integer) As Short

	Declare Function SSCE_SetCharSet Lib "ssce5564.dll" (ByVal charSet As Integer) As Integer

	Declare Sub SSCE_SetDebugFile Lib "ssce5564.dll" (ByVal debugFile As String)

	Declare Function SSCE_SetOption Lib "ssce5564.dll" (ByVal sid As Short, ByVal opt As Integer, ByVal optVal As Integer) As Integer

	Declare Function SSCE_Suggest Lib "ssce5564.dll" (ByVal sid As Short, ByVal word As String, ByVal depth As Short, ByVal suggBfr As String, ByVal suggBfrSz As Integer, ByRef scores As Short, ByVal nScores As Short) As Short

	Declare Function SSCE_SyncLex Lib "ssce5564.dll" (ByVal sid As Short, ByVal lexId As Short) As Integer

	Declare Sub SSCE_Version Lib "ssce5564.dll" (ByVal version As String, ByVal versionSz As Short)

	' Functions in the Windows API:

	Declare Function SSCE_CheckBlockDlg Lib "ssce5564.dll" (ByVal parent As Integer, ByVal block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal showContext As Short) As Integer

	Declare Function SSCE_CheckBlockDlgTmplt Lib "ssce5564.dll" (ByVal parent As Integer, ByVal block As String, ByVal blkLen As Integer, ByVal blkSz As Integer, ByVal showContext As Short, ByVal clientInst As Integer, ByVal spellDlgTmplt As String, ByVal dictDlgTmplt As String, ByVal optDlgTmplt As String, ByVal newLexDlgTmplt As String) As Integer

	Declare Function SSCE_CheckCtrlBackground Lib "ssce5564.dll" (ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Short

	Declare Function SSCE_CheckCtrlBackgroundClear Lib "ssce5564.dll" (ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer

	Declare Function SSCE_CheckCtrlBackgroundMenu Lib "ssce5564.dll" (ByVal ctrlWin As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer

	Declare Function SSCE_CheckCtrlBackgroundNotify Lib "ssce5564.dll" (ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer

	Declare Function SSCE_CheckCtrlBackgroundRecheckAll Lib "ssce5564.dll" (ByVal ctrlWin As Integer, ByVal options As Integer, ByVal markColor As Integer) As Integer

	Declare Function SSCE_CheckCtrlDlg Lib "ssce5564.dll" (ByVal parent As Integer, ByVal ctrlWin As Integer, ByVal selTextOnly As Short) As Short

	Declare Function SSCE_CheckCtrlDlgTmplt Lib "ssce5564.dll" (ByVal parent As Integer, ByVal ctrlWin As Integer, ByVal selTextOnly As Short, ByVal clientInst As Integer, ByVal spellDlgTmplt As String, ByVal dictDlgTmplt As String, ByVal optDlgTmplt As String, ByVal newLexDlgTmplt As String) As Short

	Declare Function SSCE_EditLexDlg Lib "ssce5564.dll" (ByVal parent As Integer) As Short

	Declare Function SSCE_EditLexDlgTmplt Lib "ssce5564.dll" (ByVal parent As Integer, ByVal clientInst As Integer, ByVal dictDlgTmplt As String, ByVal newLexDlgTmplt As String) As Short

	Declare Function SSCE_GetAutoCorrect Lib "ssce5564.dll" () As Short

	Declare Sub SSCE_GetHelpFile Lib "ssce5564.dll" (ByVal fileName As String, ByVal fileNameSz As Short)

	Declare Sub SSCE_GetIniFile Lib "ssce5564.dll" (ByVal fileName As String, ByVal fileNameSz As Short)

	Declare Function SSCE_GetLexId Lib "ssce5564.dll" (ByVal lexFileName As String) As Short

	Declare Sub SSCE_GetMainLexFiles Lib "ssce5564.dll" (ByVal fileList As String, ByVal fileListSz As Short)

	Declare Sub SSCE_GetMainLexPath Lib "ssce5564.dll" (ByVal path As String, ByVal pathSz As Short)

	Declare Function SSCE_GetMinSuggestDepth Lib "ssce5564.dll" () As Short

	Declare Sub SSCE_GetRegTreeName Lib "ssce5564.dll" (ByVal regTreeName As String, ByVal regTreeNameSz As Short)

	Declare Sub SSCE_GetSelUserLexFile Lib "ssce5564.dll" (ByVal fileName As String, ByVal fileNameSz As Short)

	Declare Function SSCE_GetSid Lib "ssce5564.dll" () As Short

	Declare Sub SSCE_GetStatistics Lib "ssce5564.dll" (ByRef wordsChecked As Integer, ByRef wordsChanged As Integer, ByRef errorsDetected As Integer)

	Declare Sub SSCE_GetStringTableName Lib "ssce5564.dll" (ByVal tableName As String, ByVal tableNameSz As Short)

	Declare Sub SSCE_GetUserLexFiles Lib "ssce5564.dll" (ByVal fileList As String, ByVal fileListSz As Short)

	Declare Sub SSCE_GetUserLexPath Lib "ssce5564.dll" (ByVal path As String, ByVal pathSz As Short)

	Declare Function SSCE_OptionsDlg Lib "ssce5564.dll" (ByVal parent As Integer) As Short

	Declare Function SSCE_OptionsDlgTmplt Lib "ssce5564.dll" (ByVal parent As Integer, ByVal clientInst As Integer, ByVal optDlgTmplt As String) As Short

	Declare Function SSCE_ResetLex Lib "ssce5564.dll" () As Short

	Declare Function SSCE_SetAutoCorrect Lib "ssce5564.dll" (ByVal ac As Short) As Short

	Declare Sub SSCE_SetDialogOrigin Lib "ssce5564.dll" (ByVal X As Short, ByVal Y As Short)

	Declare Function SSCE_SetHelpFile Lib "ssce5564.dll" (ByVal helpFile As String) As Short

	Declare Function SSCE_SetIniFile Lib "ssce5564.dll" (ByVal iniFile As String) As Short

	Declare Function SSCE_SetKey Lib "ssce5564.dll" (ByVal key As Integer) As Short

	Declare Function SSCE_SetMainLexFiles Lib "ssce5564.dll" (ByVal fileList As String) As Short

	Declare Function SSCE_SetMainLexPath Lib "ssce5564.dll" (ByVal path As String) As Short

	Declare Function SSCE_SetMinSuggestDepth Lib "ssce5564.dll" (ByVal depth As Short) As Short

	Declare Function SSCE_SetRegTreeName Lib "ssce5564.dll" (ByVal regTreeName As String) As Short

	Declare Function SSCE_SetSelUserLexFile Lib "ssce5564.dll" (ByVal fileName As String) As Short

	Declare Function SSCE_SetStringTableName Lib "ssce5564.dll" (ByVal tableName As String) As Short

	Declare Function SSCE_SetUserLexFiles Lib "ssce5564.dll" (ByVal fileList As String) As Short

	Declare Function SSCE_SetUserLexPath Lib "ssce5564.dll" (ByVal path As String) As Short
End Module