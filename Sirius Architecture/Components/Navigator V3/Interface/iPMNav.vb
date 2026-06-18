Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02/10/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	'RFC160299 - Data Capture Process Type Added.
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMNavigator"
	
	Public Const ACHelpFileLocation As String = "\..\..\Common\Help\Navigator.hlp"
	
	' Public constants for the icon indexes.
	Public Const ACIconProcess As String = "Process"
	
	Public Const ACIconSubMap As String = "SubMap"
	Public Const ACIconSubMapGrey As String = "SubMapGrey"
	
	Public Const ACIconStepFind As String = "StepFind"
	Public Const ACIconStepFindTick As String = "StepFindTick"
	Public Const ACIconStepFindCross As String = "StepFindCross"
	Public Const ACIconStepFindGrey As String = "StepFindGrey"
	
	Public Const ACIconStepDataForm As String = "StepDataForm"
	Public Const ACIconStepDataFormTick As String = "StepDataFormTick"
	Public Const ACIconStepDataFormCross As String = "StepDataFormCross"
	Public Const ACIconStepDataFormGrey As String = "StepDataFormGrey"
	
	Public Const ACIconStepNoForm As String = "StepNoForm"
	Public Const ACIconStepNoFormTick As String = "StepNoFormTick"
	Public Const ACIconStepNoFormCross As String = "StepNoFormCross"
	Public Const ACIconStepNoFormGrey As String = "StepNoFormGrey"
	
	Public Const ACIconStepDecision As String = "StepDecision"
	Public Const ACIconStepDecisionTick As String = "StepDecisionTick"
	Public Const ACIconStepDecisionCross As String = "StepDecisionCross"
	Public Const ACIconStepDecisionGrey As String = "StepDecisionGrey"
	
	' Public constant for the process index value.
	Public Const ACProcessIndex As String = "P"
	
	' Public constants for the summary frame indexes.
	Public Const ACSummMainSummIndex As String = "MS"
	Public Const ACSummProcessSummIndex As String = "PS"
	Public Const ACSummMapInstIndex As String = "MI"
	Public Const ACSummMapSummIndex As String = "MS"
	
	' Public constants for the status messages.
	Public Const ACStatusMessageNone As Integer = 0
	Public Const ACStatusMessageNewProcess As Integer = 1
	Public Const ACStatusMessageBuildRoadmap As Integer = 2
	Public Const ACStatusMessageProcessStep As Integer = 3
	
	' Registry Constants
	' RDC 13062002 now in gPMConstants
	'Public Const ACNavRegSubKey As String = "Navigator"
	Public Const ACNavRegMainHeight As String = "MainHeight"
	Public Const ACNavRegMainWidth As String = "MainWidth"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACProcessTitle As Integer = 101
	Public Const ACSummaryTitle As Integer = 102
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACRestartButton As Integer = 203
	Public Const ACCloseButton As Integer = 204
	Public Const ACContinueButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACDeleteDetailsTitle As Integer = 304
	Public Const ACDeleteDetails As Integer = 305
	Public Const ACStatusNewProcess As Integer = 306
	Public Const ACStatusBuildRoadmap As Integer = 307
	Public Const ACStatusProcessStep As Integer = 308
	
	Public Const ACConfirmCloseHeader As Integer = 309
	Public Const ACConfirmCloseMsg As Integer = 310
	
	Public Const ACUserDriven As Integer = 311
	Public Const ACNavigatorDriven As Integer = 312
	Public Const ACNavigable As Integer = 313
	Public Const ACDataCapture As Integer = 319
	
	Public Enum ACENavProcessType
		aceProcTypeNavDriven = 0
		aceProcTypeUserDriven = 1
		aceProcTypeNavigable = 2
		aceProcTypeDataCapture = 3
	End Enum
	
	Public Enum ACEProcStatus
		aceProcStatusBlank = 0
		aceProcStatusInProgress = 314
		aceProcStatusNavigate = 315
		aceProcStatusComplete = 316
		aceProcStatusIncomplete = 317
		aceProcStatusError = 318
	End Enum
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oNavBusiness As bPMNavigator.Business
	
	Sub Main_Renamed()
		
	End Sub
	
	' ***************************************************************** '
	' Name: UpdateKeyValuesFromArray
	'
	' Description:
	'
	' v_colKeysToUpdate is the list of Keys which need to be updated.
	' v_colKeysToBeUpdated is the collection of Keys which will have
	' their values updated. Note: If the Key does not already exist in the
	' collection it will be added.
	' v_vKeyValueFrom is the Key Values which the Collection will be
	' updated from.
	' ***************************************************************** '
	Public Function UpdateKeyValuesFromArray(ByVal v_colKeysToUpdate As iPMNavigator.Keys, ByVal v_colKeysToBeUpdated As iPMNavigator.Keys, ByVal v_vKeyValueFrom( ,  ) As Object) As Integer
		
		Dim result As Integer = 0
		Dim oKey As iPMNavigator.Key
		Dim sKeyName As String = ""
        Dim vKeyValue As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Key Value Changes
			'    ' If there are Keys to Update, but NO Key Value Array
			'    ' then Log an Error and exit.
			'    If (v_colKeysToUpdate.Count > 0) _
			''    And (IsArray(v_vKeyValueFrom) = False) Then
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Key Value Array is Empty", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="UpdateKeyValuesFromArray"
			'        UpdateKeyValuesFromArray = PMFalse
			'        Exit Function
			'    End If
			
			' For each Key which needs to be updated
			For	Each oKey2 As iPMNavigator.Key In v_colKeysToUpdate
				oKey = oKey2
				
				' Get the Key Name
				sKeyName = oKey.KeyName
				
				' Init the Key Value to empty
				vKeyValue = String.Empty
				
				' If we have a Key Value Array
				If Information.IsArray(v_vKeyValueFrom) Then
					
					' Get the Key Value from the Array
					For lRow As Integer = v_vKeyValueFrom.GetLowerBound(1) To v_vKeyValueFrom.GetUpperBound(1)

						If sKeyName.Trim() = CStr(v_vKeyValueFrom(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)) Then
							If Information.VarType(v_vKeyValueFrom(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = VariantType.Object Then
								vKeyValue = v_vKeyValueFrom(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
							Else

                                vKeyValue = v_vKeyValueFrom(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
							End If
							Exit For
						End If
					Next lRow
					
				End If
				
				' Did we find the Key Value

                If IsDBNull(vKeyValue) OrElse IsNothing(vKeyValue) Then

                    ' No, so use the Initial Value if there is one.
                    If Not (Convert.IsDBNull(oKey.Value) Or IsNothing(oKey.Value)) Then
                        vKeyValue = oKey.Value
                    End If

                End If
				
				' If we did not find the Key Value, log an error

                'If String.IsNullOrEmpty(vKeyValue) Then
                If IsDBNull(vKeyValue) OrElse IsNothing(vKeyValue) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Find a Key Value For Key - " & sKeyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateKeyValuesFromArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the Key Value in the Collection
                m_lReturn = CType(v_colKeysToBeUpdated.UpdateValue(v_sKeyName:=sKeyName, v_vValue:=vKeyValue), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                vKeyValue = Nothing

            Next oKey2
			
			' Release the local reference
			oKey = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error.
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateKeyValuesFromArray", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateKeyValuesFromArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UpdateKeyValuesFromCollection
	'
	' Description:
	'
	' v_colKeysToUpdate is the list of Keys which need to be updated.
	' v_colKeysToBeUpdated is the collection of Keys which will have
	' their values updated. Note: If the Key does not already exist in the
	' collection it will be added.
	' v_colKeyValuesFrom is the Key Values which the Collection will be
	' updated from.
	' ***************************************************************** '
	Public Function UpdateKeyValuesFromCollection(ByVal v_colKeysToUpdate As iPMNavigator.Keys, ByVal v_colKeysToBeUpdated As iPMNavigator.Keys, ByVal v_colKeyValuesFrom As iPMNavigator.Keys) As Integer
		
		Dim result As Integer = 0
		Dim oKey, oKeyValue As iPMNavigator.Key
        Dim sKeyName As String = ""
		Dim vKeyValue As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Key Value Changes
			'    ' If there are Keys to Update, but NO Key Value Collection
			'    ' then Log an Error and exit.
			'    If (v_colKeysToUpdate.Count > 0) _
			''    And (v_colKeyValuesFrom.Count < 1) Then
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Key Value Collection is Empty", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="UpdateKeyValuesFromCollection"
			'        UpdateKeyValuesFromCollection = PMFalse
			'        Exit Function
			'    End If
			
			' For each Key which needs to be updated
			For	Each oKey2 As iPMNavigator.Key In v_colKeysToUpdate
				oKey = oKey2
				
				' Get the Key Name
				sKeyName = oKey.KeyName
				
				' Init the Key Value to empty
				vKeyValue = String.Empty
				
				' If we Have a Key Value Collection
				If Not (v_colKeyValuesFrom Is Nothing) Then
					
					' Find this Key in the Value Collection
					oKeyValue = v_colKeyValuesFrom.Item(sKeyName)
					
					' If we found the Key Value, use it.
					If Not (oKeyValue Is Nothing) Then
						If Information.VarType(oKeyValue.Value) = VariantType.Object Then
							vKeyValue = oKeyValue.Value
						Else
							vKeyValue = oKeyValue.Value
						End If
						oKeyValue = Nothing
					End If
					
				End If
				
				' Did we find the Key Value

				If String.IsNullOrEmpty(vKeyValue) Then
					
					' No, so use the Initial Value if there is one.

					If Not (Convert.IsDBNull(oKey.Value) Or IsNothing(oKey.Value)) Then
						vKeyValue = oKey.Value
					End If
					
				End If
				
				' Did we find the Key Value

				If String.IsNullOrEmpty(vKeyValue) Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Find a Key Value For Key - " & sKeyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateKeyValuesFromCollection")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Update the Key Value in the Collection
				m_lReturn = CType(v_colKeysToBeUpdated.UpdateValue(v_sKeyName:=sKeyName, v_vValue:=vKeyValue), gPMConstants.PMEReturnCode)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			Next oKey2
			
			' Release the local reference
			oKey = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error.
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateKeyValuesFromCollection", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateKeyValuesFromCollection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ReturnKeyValuesAsArray
	'
	' Description:
	'
	'
	' ***************************************************************** '
    Public Function ReturnKeyValuesAsArray(ByVal v_colKeysToReturn As iPMNavigator.Keys, ByVal v_colKeyValueCollection As iPMNavigator.Keys, ByRef r_vKeyValueArray As Object) As Integer

        Dim result As Integer = 0
        Dim oKey, oKeyValue As iPMNavigator.Key
        Dim lRow As Integer
        Dim sKeyName As String = ""
        Dim vKeyValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vKeyValueArray = Nothing

            ' If there are NO keys to Return,
            ' just return an empty string.
            If v_colKeysToReturn.Count() < 1 Then

                'developer guide no. 17

                r_vKeyValueArray = Nothing
                Return result
            End If

            lRow = 0

            ' Dimension the Array
            ReDim r_vKeyValueArray(1, v_colKeysToReturn.Count() - 1)

            ' For each Key to Return
            For Each oKey2 As iPMNavigator.Key In v_colKeysToReturn
                oKey = oKey2

                ' Get the Key Name
                sKeyName = oKey.KeyName

                ' Init the Key Value to empty


                ' If we Have a Key Value Collection
                If Not (v_colKeyValueCollection Is Nothing) Then

                    ' Find this Key in the Value Collection
                    oKeyValue = v_colKeyValueCollection.Item(sKeyName)

                    ' If we found the Key Value, use it.
                    If Not (oKeyValue Is Nothing) Then
                        If Information.VarType(oKeyValue.Value) = VariantType.Object Then
                            vKeyValue = oKeyValue.Value
                        Else
                            vKeyValue = oKeyValue.Value
                        End If
                        oKeyValue = Nothing
                    End If

                End If

                ' Did we find the Key Value

               If IsDBNull(vKeyValue) OrElse IsNothing(vKeyValue) Then
                    ' No, so use the Initial Value if there is one.

                    If Not (Convert.IsDBNull(oKey.Value) Or IsNothing(oKey.Value)) Then
                        vKeyValue = oKey.Value
                    End If

                End If

                ' Did we find the Key Value

                If IsDBNull(vKeyValue) OrElse IsNothing(vKeyValue) Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Find a Key Value For Key - " & sKeyName, vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnKeyValuesAsArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Key Value to the Array

                r_vKeyValueArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow) = sKeyName
                If Information.VarType(vKeyValue) = VariantType.Object Then
                    r_vKeyValueArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = vKeyValue
                Else

                    r_vKeyValueArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = vKeyValue
                End If

                lRow += 1

            Next oKey2

            ' Release the local reference
            oKey = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ReturnKeyValuesAsArray Key to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnKeyValuesAsArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: UpdateFromSetKeyInitValues
	'
	' Description:
	'
	' v_colSetKeys is the list of Set Keys which need to be updated if
	' they have an initial Key Value.
	' v_colKeysToBeUpdated is the collection of Keys which will have
	' their values updated. Note: If the Key does not already exist in the
	' collection it will be added.
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (UpdateFromSetKeyInitValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function UpdateFromSetKeyInitValues(ByVal v_colSetKeys As iPMNavigator.Keys, ByVal v_colKeysToBeUpdated As iPMNavigator.Keys) As Integer
		'
		'Dim result As Integer = 0
		'Dim oKey, oKeyValue As iPMNavigator.Key
		'Dim lRow As Integer
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' For each Key which needs to be updated
			'For	Each oKey2 As iPMNavigator.Key In v_colSetKeys
				'oKey = oKey2
				'
				'With oKey
					' If the Set Key has an initial Value
					' i.e. It is NOT NULL

					'If Not (Convert.IsDBNull(.Value) Or IsNothing(.Value)) Then
						' Update the Key Value in the Collection
						'm_lReturn = CType(v_colKeysToBeUpdated.UpdateValue(v_sKeyName:=.KeyName, v_vValue:=.Value), gPMConstants.PMEReturnCode)
						'
						'oKeyValue = Nothing
						'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							'Return gPMConstants.PMEReturnCode.PMFalse
						'End If
					'End If
				'End With
				'
			'Next oKey2
			'
			' Release the local reference
			'oKey = Nothing
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error.
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateFromSetKeyInitValues", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFromSetKeyInitValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
    'End Function
End Module
