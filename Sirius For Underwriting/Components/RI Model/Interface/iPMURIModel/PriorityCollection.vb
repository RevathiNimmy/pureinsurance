Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("PriorityCollection_NET.PriorityCollection")> _
Public NotInheritable Class PriorityCollection 
	Implements IEnumerable
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "PriorityCollection"
	
	
	' Collection object
	Private m_cPriorities As New Collection
	
	
	Public ReadOnly Property Count() As Integer
		Get
			Return m_cPriorities.Count
		End Get
	End Property
	
	
	' ***************************************************************** '
	' Returns either a valid priority object or, where it does not
	' exist yet, a new empty priority object.
	' ***************************************************************** '
	Public ReadOnly Property Item(ByVal lPriority As Integer) As Priority
		Get
            Dim oPriority As Priority
			
			' Try and get an item

            'Try 
            If (m_cPriorities.Contains("P" & lPriority)) Then
                oPriority = m_cPriorities.Item("P" & lPriority)
            Else

                ' If we haven't got one yet, create it
                If oPriority Is Nothing Then
                    oPriority = New Priority()
                    oPriority.Priority = lPriority
                    oPriority.Lines = 1
                    m_cPriorities.Add(oPriority, "P" & lPriority)
                End If
            End If

            Return oPriority

            'Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
            'End Try
        End Get
	End Property
	
	' ***************************************************************** '
	' Name: NewEnum (Posh Method :-)
	'
	' Description: Allow this collection to be enumerated with
	'   For Each...Next
	'
	' Notes:
	'   The return property from this call must be IUnknown!!
	'   The _NewEnum property of the collection is hidden
	'   For this to function the Procedure ID must be set to -4
	' ***************************************************************** '
	
	Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		' Pass through to collection class
		Return m_cPriorities.GetEnumerator
	End Function
	
	
	' ***************************************************************** '
	' Gets the next priority with less than 100% share
	' ***************************************************************** '
	Public ReadOnly Property NextAvailable() As Priority
		Get
			
			Dim result As Priority = Nothing
			Dim oAvailable, oHighest As Priority
			
			Dim lReturn As Integer
			Const kMethodName As String = "Refresh"
			
			
            Try

                ' Check for existing priorities
                If m_cPriorities.Count = 0 Then
                    ' If we have nothing yet return an empty priority 1
                    result = Item(1)
                Else
                    ' Set highest to start
                    oHighest = m_cPriorities(1)

                    ' Walk the list

                    For Each oNext As Priority In m_cPriorities



                        ' Check availability
                        If (oNext.Share < 100 Or oNext.Share > 100) And Not oNext.bIsObligatory Then
                            ' If we have no available priority yet store this one
                            If oAvailable Is Nothing Then
                                oAvailable = oNext
                            Else
                                ' If we have an available priority check to see if this is lower
                                If oNext.Priority < oAvailable.Priority Then
                                    oAvailable = oNext
                                End If
                            End If
                        End If

                        ' Check for highest
                        If oNext.Priority > oHighest.Priority Then
                            oHighest = oNext
                        End If
                    Next oNext

                    ' Check to see what we found
                    If Not (oAvailable Is Nothing) Then
                        ' We have an available priority, return it
                        result = oAvailable
                    Else
                        ' We don't so get the next hightest empty priority
                        result = Item(oHighest.Priority + 1)
                    End If
                End If

            Catch ex As Exception
                ' DO Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)


            Finally
            End Try

            Return result
        End Get
	End Property
	
	
	' ***************************************************************** '
	' Takes an array of ri model lines and extracts priority summary
	' information for use when adding or editing lines
	' ***************************************************************** '
	Public Function Refresh(ByVal vLines( ,  ) As Object, Optional ByVal lCurrentItem As Integer = -1, Optional ByVal bRefreshRI As Boolean = False, Optional ByVal bIsRI2007 As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim oPriority As Priority
		
		Dim lReturn As Integer
		Const kMethodName As String = "Refresh"
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Clear existing items
		m_cPriorities = New Collection()
		
		' Check for array
		If Information.IsArray(vLines) Then
				' Process each line

				' But not the current one
				'PN 25691
				'todosriram

				If bRefreshRI Then

					For lCount As Integer = vLines.GetLowerBound(1) To vLines.GetUpperBound(1)
						If gPMFunctions.ToSafeInteger(vLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount), 0) > 0 Then
							' Get a priority object (use the internal method as it's already protected!!)

							oPriority = Item(CInt(vLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)))

							oPriority.Lines = CDec(gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)))
							oPriority.LineLimit = gPMFunctions.ToSafeCurrency(vLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))
							oPriority.Share += gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount))

							oPriority.LowerLimit = gPMFunctions.ToSafeCurrency(vLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount))
							oPriority.Ceding += gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLCedingrate, lCount))
							'Start (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
							oPriority.lReinsuranceType = gPMFunctions.ToSafeLong(vLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
							'End (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
							'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
							oPriority.bIsObligatory = gPMFunctions.ToSafeBoolean(vLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount))
							'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
						End If

					Next lCount

				Else

					For lCount As Integer = vLines.GetLowerBound(1) To vLines.GetUpperBound(1)

						'PN-69480 -Sushil Kumar
						If gPMFunctions.ToSafeInteger(vLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount), 0) > 0 Then
							If lCount = lCurrentItem Then
								' Get a priority object (use the internal method as it's already protected!!)

								oPriority = Item(CInt(vLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)))

								oPriority.Lines = CDec(gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)))
								oPriority.LineLimit = gPMFunctions.ToSafeCurrency(vLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))
								oPriority.Share += gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount))

								oPriority.LowerLimit = gPMFunctions.ToSafeCurrency(vLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount))
								oPriority.Ceding += gPMFunctions.ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLCedingrate, lCount))
								'Start (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
								oPriority.lReinsuranceType = gPMFunctions.ToSafeLong(vLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
								'End (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
								'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
								oPriority.bIsObligatory = gPMFunctions.ToSafeBoolean(vLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount))
								'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
							End If
						End If
					Next lCount
				End If
			
			
			
		End If
		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	
	
	Protected Overrides Sub Finalize()
		m_cPriorities = Nothing
	End Sub
End Class
