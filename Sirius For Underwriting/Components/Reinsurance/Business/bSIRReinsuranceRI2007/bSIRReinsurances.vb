Option Strict Off
Option Explicit On
'developer guide no. 129
Imports System.Collections.ObjectModel
Imports SSP.Shared

Friend NotInheritable Class Reinsurances
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Reinsurances
    '
    ' Date: 05/05/1997
    '
    ' Description: Maintains the Reinsurances Collection.
    '
    ' Edit History:
    '   23/06/2003 Peter Finney - Made class enumerable
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Reinsurances"


    ' Define the Reinsurance Collection
    Private m_cReinsurances As New ReinsurancesKeyedCollection


    Public Function Add(ByRef oNewReinsurance As bSIRReinsuranceRI2007.Reinsurance) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Add"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied Reinsurance into the Reinsurances Collection
            m_cReinsurances.Add(oNewReinsurance)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Sub Clear()
        ' Set Reinsurance Collection to Nothing
        m_cReinsurances = Nothing

        ' Added by Scalability Update Program - 01/08/2002
        m_cReinsurances = New ReinsurancesKeyedCollection
    End Sub

    Public Function Count() As Integer
        Return m_cReinsurances.Count
    End Function

    Public Function Item(ByVal Index As String) As bSIRReinsuranceRI2007.Reinsurance
        ' Return item

        Try
            Return m_cReinsurances(Index)

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return Nothing
    End Function

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
        Return m_cReinsurances.GetEnumerator
    End Function

    Public Sub Remove(ByVal Index As String)
        m_cReinsurances.Remove(Index)
    End Sub

    Public Sub Round()


        ' No error handling, allow errors to bubble up

        ' Simply loop round each band and perform any required rounding
        For Each oReinsurance As bSIRReinsuranceRI2007.Reinsurance In m_cReinsurances
            If oReinsurance IsNot Nothing Then
                oReinsurance.Round()
            End If
        Next oReinsurance
    End Sub

End Class
