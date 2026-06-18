Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTMaintainMediaTypeStatus"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Enum enuSelMediaTypeStatusFields
		enuSelMTSF_CASHLISTITEM_ID = 0
		enuSelMTSV_INSURANCEFILE_ID = 1
		enuSelMTSV_MEDIATYPE_ID = 2
		enuSelMTSV_MEDIATYPE_CODE = 3
		enuSelMTSV_MEDIATYPESTATUS_ID = 4
		enuSelMTSV_MEDIATYPESTATUS_CODE = 5
		enuSelMTSV_DOCUMENT_REF = 6
		enuSelMTSV_BRANCH = 7
		enuSelMTSV_CLIENT_CODE = 8
		enuSelMTSF_CLIENT_NAME = 9
		enuSelMTSV_POLICY_NUMBER = 10
		enuSelMTSV_MEDIATYPE = 11
		enuSelMTSV_MEDIAREFERENCE = 12
		enuSelMTSV_DRAWN_BANK_NAME = 13
		enuSelMTSV_MEDIATYPESTATUS = 14
		enuSelMTSV_MAX_INDEX = enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS
	End Enum
	
	Public Enum enuUpdMediaTypeStatusFields
		enuUpdMTSF_CASHLISTITEM_ID = 0
		enuUpdMTSV_MEDIATYPE_ID = 1
		enuUpdMTSV_MEDIATYPESTATUS_ID = 2
		enuUpdMTSV_COMMENTS = 3
		enuUpdMTSV_USER_ID = 4
		enuUpdMTSV_DATE_MODIFIED = 5
		enuUpdMTSV_INSURANCEFILE_ID = 6
		enuUpdMTSV_DOCUMENT_REF = 7
		enuUpdMTSV_MAX_INDEX = enuUpdMediaTypeStatusFields.enuUpdMTSV_DOCUMENT_REF
	End Enum
	
	' Log Level
	

	Public Sub Main()
		
		' Main entry point for the component
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module