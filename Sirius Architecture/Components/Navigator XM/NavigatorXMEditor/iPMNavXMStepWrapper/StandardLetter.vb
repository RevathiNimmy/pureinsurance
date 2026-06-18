Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("StandardLetter_NET.StandardLetter")> _
Public NotInheritable Class StandardLetter 
	
	  Implements IDisposable
Private m_lReturn As Integer
	Private m_lStatus As Integer
	
	' Step variables
	Private m_lPartyCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	'Private m_lClaimCnt As Long
	Private m_sDocumentRef As String = ""
	Private m_lEventCnt As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sNavigatorTitle As String = ""
	
	Private m_lDocumentTemplateId As Integer
	'Private m_sDocumentTemplateCode As String
	'Private m_sDocumentTemplateDescription As String
	Private m_lDocumentTypeId As Integer
	'Private m_sDocumentTypeCode As String
	'Private m_sDocumentTypeDescription As String
	
	Private m_lMode As Integer
	Private m_bFromBatchTrans As Boolean
	
Private Const ACClass As String = "StandardLetter" 
	
	' Step properties
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
			
			result = gPMConstants.PMEReturnCode.PMfalse
			
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

Public Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMfalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                Select Case gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow))).Trim()
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsuranceFolderCnt = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameEventCnt

                        m_lEventCnt = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.ACTKeyNameDocumentRef

                        m_sDocumentRef = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDocumentTemplateId

                        m_lDocumentTemplateId = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDocumentTypeId

                        m_lDocumentTypeId = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDocTemplateMode

                        m_lMode = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameFromBatchTrans

                        m_bFromBatchTrans = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMfalse

            ' user code here


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMfalse

            '    ReDim vKeyArray(0 To 1, 0 To 0)
            '    vKeyArray(PMKeyName, 0) = 'PMKeyNameInsFileCnt
            '    vKeyArray(PMKeyValue, 0) =' m_lInsuranceFileCnt


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
        'developer guide no.88
        Dim oObject As Object
        Dim oDocTemplate As iPMBFindDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMfalse

            If m_lDocumentTemplateId = 0 Then

                oDocTemplate = New iPMBFindDocTemplate.Interface_Renamed()

                m_lReturn = CType(oDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDocTemplate = Nothing
                    Return result
                End If

                oDocTemplate.CallingAppName = ACApp

                m_lReturn = oDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDocTemplate = Nothing
                    Return result
                End If

                oDocTemplate.Mode = 1

                If m_lDocumentTypeId > 0 Then
                    oDocTemplate.DocumentTypeId = m_lDocumentTypeId
                End If

                m_lReturn = oDocTemplate.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDocTemplate = Nothing
                    Return result
                End If

                m_lDocumentTemplateId = oDocTemplate.DocumentTemplateId
                m_lDocumentTypeId = oDocTemplate.DocumentTypeId

                oDocTemplate.Dispose()
                oDocTemplate = Nothing

            End If


            If m_lDocumentTemplateId > 0 Then

                Dim temp_oObject As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oObject = temp_oObject

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to create iPMBDocTemplate.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return result
                End If


                m_lReturn = oObject.SetProcessModes(vTransactionType:=m_sTransactionType, vProcessMode:=m_lProcessMode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oObject = Nothing
                    Return result
                End If

                Dim vKeyArray(1, 8) As Object


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsFolderCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFolderCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameEventCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lEventCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameDocumentRef

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sDocumentRef


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameDocumentTemplateId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lDocumentTemplateId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameDocumentTypeId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lDocumentTypeId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameDocTemplateMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lMode


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameFromBatchTrans

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_bFromBatchTrans


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


                m_lReturn = oObject.GetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oObject = Nothing
                    Return result
                End If


		oObject.Dispose()

                oObject = Nothing

                Return gPMConstants.PMEReturnCode.PMTrue

            Else
                Return gPMConstants.PMEReturnCode.PMfalse
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Class
