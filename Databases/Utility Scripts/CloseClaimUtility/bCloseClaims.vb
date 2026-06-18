Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public Class Business
	Implements SSP.S4I.Interfaces.IBusiness
	
    ' ***************************************************************** '
	' Class Name: Business
	'
	' Date: 14/07/2000
	'
	' Description: Creatable Bussiness class which contains all the
	'              methods, business rules required for the
	'              SIRFindClaim .
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' ************************************************
	' Added to replace global variables 11/12/2003
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
	
	' SET 01082002 - Removed for scalability
	'Private oComponentServices As PMServerBusinessCS
	
	' Close Database Flag (Private)
	Private m_bCloseDatabase As Boolean
	
	' Current Record Pointer
	Private m_lCurrentRecord As Integer
	
	' Error Code (Private)
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lError As Integer
	
	' Process Mode Properties
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	Public ReadOnly Property PMProductFamily() As Integer
		Get
			
			Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
			
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
	
	Public ReadOnly Property TransactionType() As String
		Get
			
			Return m_sTransactionType
			
		End Get
	End Property
	
	Public ReadOnly Property EffectiveDate() As Date
		Get
			
			Return m_dtEffectiveDate
			
		End Get
	End Property
	
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' Date :15/07/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
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
			
			
			' Set Username and Password
			
			m_lCurrentRecord = 0
			m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
			m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
			m_dtEffectiveDate = DateTime.Now
			
			

			m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Terminate (Standard Method)
	'
	' Description: Entry point for any termination code for this
	'              object.
	'
	' Date :15/07/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Public Function Terminate() As Integer
		
		Dim result As Integer = 0
		Static bTerminated As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If we have already Terminated then exit
			
			If bTerminated Then
				Return result
			Else
				bTerminated = True
			End If
			
			' If this class opened the database, close it
			If m_bCloseDatabase Then
				' Close the Database
				m_lReturn = m_oDatabase.CloseDatabase()
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			' Release reference to PM Data Access Object
			m_oDatabase = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetProcessModes (Standard Method)
	'
	' Description: Set the optional process modes.
	'
	' Date :15/07/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the process modes to the property members.
			

			If Not Information.IsNothing(vTask) Then

				m_iTask = CInt(vTask)
			End If
			

			If Not Information.IsNothing(vNavigate) Then

				m_lNavigate = CInt(vNavigate)
			End If
			

			If Not Information.IsNothing(vProcessMode) Then

				m_lProcessMode = CInt(vProcessMode)
			End If
			

			If Not Information.IsNothing(vTransactionType) Then

				m_sTransactionType = CStr(vTransactionType)
			End If
			

			If Not Information.IsNothing(vEffectiveDate) Then

				m_dtEffectiveDate = CDate(vEffectiveDate)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function

 



    Public Sub New()
        MyBase.New()

        Try

            Dim vDatabase As Object

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Exit Sub

        End Try



    End Sub

    Protected Overrides Sub Finalize()

        Try

            ' Class Terminate

            ' Call Terminate Method in case Calling Object
            ' has forgotten to.
            m_lReturn = Terminate()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub


    '******************************************************************************
    ' Apostrophes
    '
    ' Take a string and replace ' with ''
    '
    '******************************************************************************
    Public Function Apostrophes(ByRef sString As String) As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sString.Length = 0 Then
                Return result
            End If

            sTemp = New StringBuilder("")

            Do While True
                i = (sString.IndexOf("'"c) + 1)

                If i = 0 Then
                    sTemp.Append(sString)
                    Exit Do
                End If

                sTemp.Append(sString.Substring(0, i - 1) & "''")
                sString = sString.Substring(i)
            Loop

            sString = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:="ExtraFunc", vMethod:="Apostrophes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    '**********************************************************************
    ' Function Name:    ClaimBuilderIsEnabled
    ' Author:           Russell Hill
    ' Date:             26/2/2003
    ' Description:      Check if SIROPTClaimsBuilder product option is ON
    '**********************************************************************
    Private Function ClaimBuilderIsEnabled() As Boolean

        Dim result As Boolean = False
        'developer guide no.101
        Dim vResult As Object

        Try

            result = True
            'developer guide no. 98
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=m_iSourceID, r_vUnderwriting:=vResult)


            Return vResult = 1

        Catch
        End Try



        Return False

    End Function


  

    ' ***************************************************************** '
    ' Name:Get Claim Details UW
    '
    ' Description:  SQL Query to Select Claim details for Underwriting Only
    '
    ' Date :15/01/2009
    '
    ' Edit History :
    ' ***************************************************************** '
    Public Function GetClaimDetailsSFU(ByRef r_vResultArray As Object, ByVal sQuery As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetailsSFU"

        Dim lReturn As gPMConstants.PMEReturnCode

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Build parameters

        ' Execute SQL Statement - use array for speed
        lReturn = m_oDatabase.SQLSelect(sSQL:=SQuery, sSQLName:=kFindClaimDetailsUWName, bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' If NO records were found return PMFalse
        If Not Information.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here

Finally_Renamed:

        Return result
        Resume

        Return result
    End Function

    Public Function Update(ByVal ClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(Claimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Status_ID", vValue:=CStr(3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Progress_Status_ID", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimDetails, sSQLName:="", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that the record was updated OK
            If lRecordsAffected > 0 Then
                ' Updated No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetClaimStatus(ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimStatus"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.SQLSelect(sSQL:=kGetClaimStatus, sSQLName:="", bStoredProcedure:=False, vResultArray:=r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End Try
        Return result

    End Function

    Public Function GetClassOfBusiness(ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClassOfBusiness"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.SQLSelect(sSQL:=kGetClassOfBusiness, sSQLName:="", bStoredProcedure:=False, vResultArray:=r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End Try
        Return result

    End Function

    Public Function GetProduct(ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProduct"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.SQLSelect(sSQL:=kGetProduct, sSQLName:="", bStoredProcedure:=False, vResultArray:=r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End Try
        Return result

    End Function

    Public Function GetProgressStatus(ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProgressStatus"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.SQLSelect(sSQL:=kGetProgressStatus, sSQLName:="", bStoredProcedure:=False, vResultArray:=r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End Try
        Return result

    End Function
End Class
