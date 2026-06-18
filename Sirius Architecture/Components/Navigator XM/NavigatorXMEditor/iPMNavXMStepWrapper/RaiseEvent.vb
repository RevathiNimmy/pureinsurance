Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("RaiseEvent_Renamed_NET.RaiseEvent_Renamed")> _
Public NotInheritable Class RaiseEvent_Renamed 
	
	  Implements IDisposable
Private m_lReturn As Integer
	Private m_lStatus As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lPartyCnt As Integer
	Private m_lClaimCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_sInsuranceRef As String = ""
	Private m_sClaimRef As String = ""
	
	' AccountKey
	Private m_lAccountKey As Integer
	Private m_sEventGroupCode As String = ""
	Private m_bDisableEventGroupLookup As Boolean
	
Private Const ACClass As String = "RaiseEvent" 
	
	Public WriteOnly Property AccountKey() As Integer
		Set(ByVal Value As Integer)
			m_lAccountKey = Value
		End Set
	End Property
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	Public WriteOnly Property InsuranceFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	Public WriteOnly Property InsuranceRef() As String
		Set(ByVal Value As String)
			m_sInsuranceRef = Value
		End Set
	End Property
	Public WriteOnly Property ClaimRef() As String
		Set(ByVal Value As String)
			m_sClaimRef = Value
		End Set
	End Property
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	Public WriteOnly Property ClaimCnt() As Integer
		Set(ByVal Value As Integer)
			m_lClaimCnt = Value
		End Set
	End Property
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	Public WriteOnly Property EventGroupCode() As String
		Set(ByVal Value As String)
			m_sEventGroupCode = Value
		End Set
	End Property
	Public WriteOnly Property DisableEventGroupLookup() As Boolean
		Set(ByVal Value As Boolean)
			m_bDisableEventGroupLookup = Value
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public Sub New()
		MyBase.New()
	End Sub
	
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

	
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lReturn = GetObjectManager()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to create ObjectManager", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


	Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
	End Sub

	
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			

            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

Public Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimID

                        m_lClaimCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsReference

                        m_sInsuranceRef = CStr(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        m_sClaimRef = CStr(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                End Select

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ReDim vKeyArray(1, 1)

            '    vKeyArray(PMKeyName, 0) = PMKeyNameQuestionTitle
            '    vKeyArray(PMKeyValue, 0) = m_sQuestionTitle

            '    vKeyArray(PMKeyName, 1) = PMKeyNameQuestionTitle
            '    vKeyArray(PMKeyValue, 1) = m_sQuestionTitle


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object

        'developer guide no.88
        Dim oObject As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBNote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to create iPMBNote.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            m_iTask = gPMConstants.PMEComponentAction.PMAdd


            m_lReturn = oObject.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vProcessMode:=m_lProcessMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If

            ' set up key array
            ReDim vKeyArray(1, 5)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sInsuranceRef

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sClaimRef


            m_lReturn = oObject.SetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If


            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If


            m_lStatus = oObject.Status

            ' not sure if this is required #################################

            m_lReturn = oObject.GetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If


		oObject.Dispose()

            oObject = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
	
	Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' user code here
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
