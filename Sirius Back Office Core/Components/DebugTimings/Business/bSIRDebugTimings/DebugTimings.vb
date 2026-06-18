Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
'developer guide no 129. 
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed

    ' ************************************************
    ' Added to replace global variables 19/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Interface"

    Private Const ACProcedureName As Integer = 0
    Private Const ACTotalTime As Integer = 1
    Private Const ACNoOfCalls As Integer = 2

    Private Const ACStartTime As Integer = 1
    Private Const ACEndTime As Integer = 2

    Private m_vProcedureArray As Object

    Private m_cCurrentProcedure As New CollectionWrapper
    Private m_cTotalProcedure As New CollectionWrapper
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' CallingAppName

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    ' ***************************************************************** '
    '
    ' Name: StartTiming
    '
    ' Description:
    '
    ' History: 24/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Sub StartTiming(ByVal v_sProcedureName As String)

        Dim lTime As Integer
        Dim vProcedureArray(1) As Object
        Dim bExists As Boolean

        Try

            lTime = Environment.TickCount


            vProcedureArray(ACProcedureName) = v_sProcedureName.Trim()

            vProcedureArray(ACStartTime) = lTime

            m_lReturn = CType(m_cCurrentProcedure.Add(vProcedureArray, v_sProcedureName.Trim(), bExists), gPMConstants.PMEReturnCode)
            If bExists Then
                m_lReturn = CType(m_cCurrentProcedure.Remove(v_sProcedureName.Trim()), gPMConstants.PMEReturnCode)
                m_lReturn = CType(m_cCurrentProcedure.Add(vProcedureArray, v_sProcedureName.Trim(), bExists), gPMConstants.PMEReturnCode)
            End If

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartTiming Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartTiming", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: EndTiming
    '
    ' Description:
    '
    ' History: 24/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Sub EndTiming(ByVal v_sProcedureName As String)

        Dim lEndTime As Integer
        Dim vCurrentProcedureArray() As Object = Nothing
        Dim vTotalProcedureArray() As Object = Nothing
        Dim bExists As Boolean
        Dim sProcedureName As String = ""
        Dim lStartTime, lTotalTime, lGrandTotalTime, lGrandTotalNoOfCalls As Integer

        Try

            'Get the details for the current running procedure (should always exist)

            m_lReturn = CType(m_cCurrentProcedure.Item(v_sProcedureName.Trim(), vCurrentProcedureArray, bExists), gPMConstants.PMEReturnCode)
            If bExists Then
                'Get the stored start time

                lStartTime = CInt(vCurrentProcedureArray(ACStartTime))
                lEndTime = Environment.TickCount
                'Calculate total time in current running procedure
                lTotalTime = lEndTime - lStartTime

                'Have we called this procedure before

                m_lReturn = CType(m_cTotalProcedure.Item(v_sProcedureName.Trim(), vTotalProcedureArray, bExists), gPMConstants.PMEReturnCode)
                If bExists Then
                    'If we have then get the current running totals

                    lGrandTotalTime = CInt(vTotalProcedureArray(ACTotalTime))

                    lGrandTotalNoOfCalls = CInt(vTotalProcedureArray(ACNoOfCalls))
                    'Remove the current item so we can re-add it
                    m_lReturn = CType(m_cTotalProcedure.Remove(v_sProcedureName.Trim()), gPMConstants.PMEReturnCode)
                Else
                    ReDim vTotalProcedureArray(2)
                End If

                'Increment the totals
                lGrandTotalTime += lTotalTime
                lGrandTotalNoOfCalls += 1

                'Re-add the item to the collection

                vTotalProcedureArray(ACProcedureName) = v_sProcedureName.Trim()

                vTotalProcedureArray(ACTotalTime) = lGrandTotalTime

                vTotalProcedureArray(ACNoOfCalls) = lGrandTotalNoOfCalls

                m_lReturn = CType(m_cTotalProcedure.Add(vTotalProcedureArray, v_sProcedureName.Trim()), gPMConstants.PMEReturnCode)

                m_lReturn = CType(m_cCurrentProcedure.Remove(v_sProcedureName), gPMConstants.PMEReturnCode)

            End If

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EndTiming Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EndTiming", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: Report
    '
    ' Description:
    '
    ' History: 24/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Sub Report()

        Try

            Dim vTotalProcedureArray() As Object = Nothing
            Dim sProcedureName As String = ""
            Dim lGrandTotalTime, lGrandTotalNoOfCalls As Integer
            Dim sMessage As New StringBuilder
            Dim dAvTime, dTotalTime As Decimal
            Dim lCount As Integer

            If m_sCallingAppName = "" Then
                m_sCallingAppName = ACApp
            End If

            m_lReturn = CType(m_cTotalProcedure.Count(lCount), gPMConstants.PMEReturnCode)

            sMessage = New StringBuilder(Strings.ChrW(13) & Strings.ChrW(10))

            For i As Integer = 1 To lCount


                m_lReturn = CType(m_cTotalProcedure.Item(CStr(i), vTotalProcedureArray), gPMConstants.PMEReturnCode)

                lGrandTotalTime = CInt(vTotalProcedureArray(ACTotalTime))

                lGrandTotalNoOfCalls = CInt(vTotalProcedureArray(ACNoOfCalls))

                sProcedureName = CStr(vTotalProcedureArray(ACProcedureName))

                dTotalTime = lGrandTotalTime / 1000
                dAvTime = dTotalTime / lGrandTotalNoOfCalls

                sMessage.Append(
                                sProcedureName & "|" & CStr(dTotalTime) & "|" & CStr(lGrandTotalNoOfCalls) &
                                "|" & CStr(dAvTime) & Strings.ChrW(13) & Strings.ChrW(10))

            Next i

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage.ToString(), vApp:=m_sCallingAppName, vClass:=ACClass, vMethod:="Report")

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Report Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Report", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub


    Protected Overrides Sub Finalize()
        m_cCurrentProcedure = Nothing
        m_cTotalProcedure = Nothing
    End Sub
End Class
