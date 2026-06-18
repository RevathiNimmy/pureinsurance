Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business 
	Implements SSP.S4I.Interfaces.IBusiness
	' *****************************************************************'
	' Class Name: Business
	'
	' Date: 01 August 2006
	'
	' Description: Creatable Business class which contains all the
	'              methods, Business rules required to manipulate
	'              the iPMUPayNowOptions.
	'
	' Edit History:
	' *****************************************************************'
	
	
	
	Private m_sUsername As String = ""
	
	Private m_sPassword As String = ""
	
	Private m_iUserID As Integer
	
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	' ************************************************
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "Business"
	
	' Database Class (Private)
	Private m_oDatabase As dPMDAO.Database
	
	' Close Database Flag (Private)
	Private m_bCloseDatabase As Boolean
	
	' Calling Application Name
	
	' Current Record Pointer
	Private m_lCurrentRecord As Integer
	
	' Error Code (Private)
	Private m_lReturn As gPMConstants.PMEReturnCode
	' Error Code (Private)
	Private m_lError As Integer
	
	' Task
	Private m_iTask As Integer
	
	' Navigate
	Private m_lNavigate As Integer
	
	' Process Mode
	Private m_lProcessMode As Integer
	' Effective
	Private m_dtEffectiveDate As Date
	
	Public ReadOnly Property PMProductFamily() As Integer
		Get
			
			Return gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting
			
		End Get
	End Property
	
	Public ReadOnly Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
	End Property
	
	Public ReadOnly Property Navigate() As Integer
		Get
			
			Return m_lNavigate
			
		End Get
	End Property
	
	Public ReadOnly Property ProcessMode() As Integer
		Get
			
			Return m_lProcessMode
			
		End Get
	End Property
	
	Public ReadOnly Property EffectiveDate() As Date
		Get
			
			Return m_dtEffectiveDate
			
		End Get
	End Property
	
	Public Function GetInsuranceRef(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetInsuranceRef"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileRefSQL, sSQLName:=ACSelectInsuranceFileRefName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, ACSelectInsuranceFileRefSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetInsuranceRef", r_lFunctionReturn:=result, excep:=ex)
		
		Finally
		

		
		End Try
		Return result		
	End Function
	
	
	'Get the Agent Type for A Insurance File
	Public Function GetAgentType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetAgentType"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentCodeForInsuranceFileCntSQL, sSQLName:=ACSelectAgentCodeForInsuranceFileCntName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, ACSelectAgentCodeForInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentType", r_lFunctionReturn:=result, excep:=ex)
		Finally
		

		
		End Try
		Return result
	End Function
	
	'Get the Agent Information from Agent_Cnt
	Public Function GetAgentDetailsFromAgentID(ByVal v_lAgentCnt As Integer, ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetAgentDetailsFromAgentID"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("Agent_Cnt", CStr(v_lAgentCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentInformationFromAgentIDSQL, sSQLName:=ACSelectAgentInformationFromAgentIDName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, ACSelectAgentInformationFromAgentIDSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentDetailsFromAgentID", r_lFunctionReturn:=result, excep:=ex)
		
		Finally
				
		End Try
		Return result		
	End Function
	
	'Get the Unallocated Credits for Agent
	Public Function GetUnallocatedCredits(ByVal v_lInsuranceFileCnt As Integer, ByVal v_bIsClient As Boolean, ByRef r_vResults(,) As Object, Optional ByRef Party_cnt As Integer = 0) As Integer 
		'In case of Multiple Policies InsuranceFileCnt=""
		
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetUnallocatedCredits"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		If v_bIsClient Then
			If m_oDatabase.Parameters.Add("party_type", "CLIENT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
			End If
		Else
			If m_oDatabase.Parameters.Add("party_type", "AGENT", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
			End If
		End If
		If Party_cnt <> 0 Then
			If m_oDatabase.Parameters.Add("party_cnt", CStr(Party_cnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
			End If
		End If
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectUnallocatedCreditForSQL, sSQLName:=ACSelectUnallocatedCreditForName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return result
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetUnallocatedCredits", r_lFunctionReturn:=result, excep:=ex)
		
		Finally
		

		
		End Try
		Return result	
	End Function
	
	
	'Get the AccountID for Agent/Client
	Public Function GetAccountID(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetAccountID"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("Party_Cnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAccountIDSQL, sSQLName:=ACSelectAccountIDName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			gPMFunctions.RaiseError(kMethodName, ACSelectAccountIDSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAccountID", r_lFunctionReturn:=result, excep:=ex)
		

		Finally
		

		
		End Try
		Return result	
	End Function
	
	'Get the AccountID for Agent/Client
	Public Function GetAccountIDFromInsuranceFile(ByVal v_lInsurance_file_cnt As Integer, ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Try
		Dim lReturn As gPMConstants.PMEReturnCode 
		
		Const kMethodName As String = "GetAccountIDFromInsuranceFile"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		
		If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(v_lInsurance_file_cnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		If m_oDatabase.Parameters.Add("CheckforAccountType", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMFalse
			Throw New Exception
		End If
		
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAccountIDFromInsuranceFileSQL, sSQLName:=ACSelectAccountIDFromInsuranceFileName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, CStr(result) & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAccountIDFromInsuranceFile", r_lFunctionReturn:=result, excep:=ex)
		
		Finally
		

		
		End Try
		Return result		
	End Function
	
	Public Sub New()
		MyBase.New()
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'
			' Class Initialise
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error Message
			'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'
			'Exit Sub
		'End Try
		
	End Sub
	Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise
		
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			'
			' *******************************************************************
			' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
			m_sUsername = sUsername
			m_sPassword = sPassword
			m_iUserID = iUserID
			m_sCallingAppName = sCallingAppName
			m_iLanguageID = iLanguageID
			m_iSourceID = iSourceID
			m_iCurrencyID = iCurrencyID
			m_iLogLevel = iLogLevel
			
			
			

			m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set Username and Password
			
			m_lCurrentRecord = 0
			m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
			m_dtEffectiveDate = DateTime.Now
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function GetUserWriteOffLimit(ByRef r_cWriteOffLimit As Decimal) As Integer 
		
		Dim result As Integer = 0 
		Try
		
		Dim lReturn As gPMConstants.PMEReturnCode 
		Dim vResults(,) As Object 
		Dim bHasWriteOffAuthority As Boolean 
		
		Const kMethodName As String = "GetUserWriteOffLimit"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_oDatabase.Parameters.Clear()
		If m_oDatabase.Parameters.Add("User_Id", CStr(m_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserWriteOffLimitSQL, sSQLName:=ACGetUserWriteOffLimitName, bStoredProcedure:=ACGetUserWriteOffLimitStored, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, CStr(result) & " Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		If Information.IsArray(vResults) Then
            bHasWriteOffAuthority = gPMFunctions.ToSafeBoolean(vResults(0, 0))
            If bHasWriteOffAuthority Then
                r_cWriteOffLimit = gPMFunctions.ToSafeCurrency(vResults(1, 0))
            End If
        End If


		Catch ex As Exception

        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetUserWriteOffLimit", r_lFunctionReturn:=result, excep:=ex)

		Finally


		End Try
	        Return result		
    End Function

    ' Start - Sankar - PN 56728
    Public Function GetPaymentDetails(ByVal lInsuranceFileCnt As Integer, ByRef r_lMediaTypeId As Integer, ByRef r_lPartyBankId As Integer) As Integer 

        Dim result As Integer = 0 
        Const kMethodName As String = "GetPaymentDetails"

        Dim vResults(,) As Object 

        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentDetailsOfLivePolicySQL, sSQLName:=ACGetPaymentDetailsOfLivePolicyName, bStoredProcedure:=ACGetPaymentDetailsOfLivePolicyStored, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Information.IsArray(vResults) Then
            r_lPartyBankId = gPMFunctions.ToSafeLong(vResults(0, 0))
            r_lMediaTypeId = gPMFunctions.ToSafeLong(vResults(1, 0))
        End If


        Catch ex As Exception

        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPaymentDetails", r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result	
    End Function
	'End - Sankar - PN 56728
End Class
