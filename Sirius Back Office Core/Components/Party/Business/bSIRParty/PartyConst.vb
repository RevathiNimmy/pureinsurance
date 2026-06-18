Option Strict Off
Option Explicit On
Imports System
Module PartyConst
	
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: PartyConst
	'
	' Date:  07/03/2005
	'
	' Description: Party Array constants
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Public Const AC_PARTYPC_INDEXLOWER As Integer = 0
	Public Const AC_PARTYPC_INDEXUPPER As Integer = 63
	
	Public Const AC_PARTYPC_PARTYCNT As Integer = 0
	Public Const AC_PARTYPC_PARTYTITLECODE As Integer = 1
	Public Const AC_PARTYPC_FORENAME As Integer = 2
	Public Const AC_PARTYPC_INITIALS As Integer = 3
	Public Const AC_PARTYPC_EMPLOYMENTSTATUSCODE As Integer = 4
	Public Const AC_PARTYPC_EMPLOYERCNT As Integer = 5
	Public Const AC_PARTYPC_EMPLOYERBUSINESS As Integer = 6
	Public Const AC_PARTYPC_SECONDEMPLOYERBUSINESSSTATUS As Integer = 7
	Public Const AC_PARTYPC_SECONDEMPLOYERBUSINESS As Integer = 8
	Public Const AC_PARTYPC_MARITALSTATUSCODE As Integer = 9
	Public Const AC_PARTYPC_NUMBERCHILDREN As Integer = 10
	Public Const AC_PARTYPC_NATIONALITYID As Integer = 11
	Public Const AC_PARTYPC_COUNTRYOFORIGIN As Integer = 12
	Public Const AC_PARTYPC_MAILSHOT As Integer = 13
	Public Const AC_PARTYPC_PETOWNER As Integer = 14
	Public Const AC_PARTYPC_ACCOMMODATIONTYPECODE As Integer = 15
	Public Const AC_PARTYPC_SHORTNAME As Integer = 16
	Public Const AC_PARTYPC_NAME As Integer = 17
	Public Const AC_PARTYPC_RESOLVED As Integer = 18
	Public Const AC_PARTYPC_ISALSOAGENT As Integer = 19
	Public Const AC_PARTYPC_ISPROSPECT As Integer = 20
	Public Const AC_PARTYPC_AGENTCNT As Integer = 21
	Public Const AC_PARTYPC_CONSULTANTCNT As Integer = 22
	Public Const AC_PARTYPC_FILECODE As Integer = 23
	Public Const AC_PARTYPC_CURRENCYID As Integer = 24
	Public Const AC_PARTYPC_PAYMENTMETHODCODE As Integer = 25
	Public Const AC_PARTYPC_REMINDERTYPEID As Integer = 26
	Public Const AC_PARTYPC_AREAID As Integer = 27
	Public Const AC_PARTYPC_SERVICELEVELID As Integer = 28
	Public Const AC_PARTYPC_CREDITCARDCODE As Integer = 29
	Public Const AC_PARTYPC_PAYMENTTERMCODE As Integer = 30
	Public Const AC_PARTYPC_CCJS As Integer = 31
	Public Const AC_PARTYPC_LIFESTYLEID As Integer = 32
	Public Const AC_PARTYPC_LIFESTYLENAME As Integer = 33
	Public Const AC_PARTYPC_CATEGORY As Integer = 34
	Public Const AC_PARTYPC_DATEOFBIRTH As Integer = 35
	Public Const AC_PARTYPC_GENDER As Integer = 36
	Public Const AC_PARTYPC_OCCUPATIONCODE As Integer = 37
	Public Const AC_PARTYPC_SECONDARYOCCUPATIONCODE As Integer = 38
	Public Const AC_PARTYPC_ISSMOKER As Integer = 39
	Public Const AC_PARTYPC_STATUS As Integer = 40
	Public Const AC_PARTYPC_ABCCOUNT As Integer = 41
	Public Const AC_PARTYPC_STATEMENTS As Integer = 42
	Public Const AC_PARTYPC_RENEWALS As Integer = 43
	Public Const AC_PARTYPC_LASTMODIFIED As Integer = 44
	Public Const AC_PARTYPC_LASTACTIONTYPE As Integer = 45
	Public Const AC_PARTYPC_DATECREATED As Integer = 46
	Public Const AC_PARTYPC_INVARIANTKEY As Integer = 47
	Public Const AC_PARTYPC_SEASONALGIFTID As Integer = 48
	Public Const AC_PARTYPC_CORRESPONDENCETYPEID As Integer = 49
	Public Const AC_PARTYPC_RENEWALSTOPCODEID As Integer = 50
	Public Const AC_PARTYPC_SWIFTPARTYID As Integer = 51
	Public Const AC_PARTYPC_SALUTATION As Integer = 52
	Public Const AC_PARTYPC_SOURCE As Integer = 53
	Public Const AC_PARTYPC_TPSIND As Integer = 54
	Public Const AC_PARTYPC_EMPSIND As Integer = 55
	Public Const AC_PARTYPC_TPPASSWORD As Integer = 56
	'AR20050307 - New FSA parameters
	Public Const AC_PARTYPC_LOYALTYNUMBER As Integer = 57
	Public Const AC_PARTYPC_ALTERNATIVEIDENTIFIER As Integer = 58
	Public Const AC_PARTYPC_MARKETINGSEGMENTIND As Integer = 59
	Public Const AC_PARTYPC_TRADINGNAME As Integer = 60
	Public Const AC_PARTYPC_SUBBRANCHID As Integer = 61
	Public Const AC_PARTYPC_TOBLETTER As Integer = 62
	Public Const AC_PARTYPC_ISFEECLIENT As Integer = 63
End Module