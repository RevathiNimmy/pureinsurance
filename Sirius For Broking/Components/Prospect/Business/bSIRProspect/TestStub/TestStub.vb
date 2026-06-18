Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Module TestStub
    Private m_lReturn As Integer

    Public Sub main()

        'TestGet

        'TestAddProspect
        'TestAddPolicy
        'TestAddCampaign
        'TestDelCampaign
        'TestDelPolicy
        TestDelProspect()

    End Sub

    ' ***********************************************************************
    '
    ' Function: VBsprintf(...)
    '
    ' Desc.   : VB equivalent of the C sprintf function
    '
    ' Note    : Target is likely to be bigger than Source when finished.
    '
    ' ***********************************************************************
    Public Function VBsprintf(ByRef sTarget As String, ByRef sSource As String, ByVal ParamArray vParams() As Object) As Integer

        Dim sByte1 As New FixedLengthString(1)
        Dim sByte2 As New FixedLengthString(1)

        sTarget = ""

        Dim iCurrParam As Integer = vParams.GetLowerBound(0)

        Dim iLen As Integer = sSource.Length

        For iLoop1 As Integer = 1 To iLen
            sByte1.Value = sSource.Substring(iLoop1 - 1, 1)


            Select Case sByte1.Value
                Case "%"
                    If iLoop1 < iLen Then
                        iLoop1 += 1
                        sByte2.Value = sSource.Substring(iLoop1 - 1, 1)

                        Select Case sByte2.Value
                            Case "s" ' String

                                sTarget = sTarget & CStr(vParams(iCurrParam))
                                iCurrParam += 1
                            Case "c" ' Char

                                sTarget = sTarget & CStr(vParams(iCurrParam))
                                iCurrParam += 1
                            Case "d" ' Decimal number

                                sTarget = sTarget & CStr(vParams(iCurrParam))
                                iCurrParam += 1
                        End Select

                    End If

                Case "\"
                    If iLoop1 < iLen Then
                        iLoop1 += 1
                        sByte2.Value = sSource.Substring(iLoop1 - 1, 1)

                        Select Case sByte2.Value
                            Case "n" ' New line
                                sTarget = sTarget & Environment.NewLine
                            Case "r" ' Return feed
                                sTarget = sTarget & Strings.Chr(10).ToString()
                            Case "t" ' Tab
                                sTarget = sTarget & Strings.Chr(9).ToString()
                            Case "\" ' Forward Slash
                                sTarget = sTarget & "\"
                        End Select
                    End If

                Case Else
                    sTarget = sTarget & sByte1.Value

            End Select

        Next iLoop1

    End Function

    Sub TestGet()
        Dim bSIRProspect As Object
        Dim vProspectStatusID As String = ""


        Dim oProspect As bSIRProspect.Business

        Dim vWageRoll As String = ""
        Dim vTurnover As String = ""
        Dim vAgentReference As String = ""
        Dim vCurrentIntermediary As String = ""
        Dim vTargetPremium As String = ""
        Dim vCampaigns, vPolicies As Object

        Dim sTarget As String = ""

        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oProspect As Object
        m_lReturn = oObjectManager.GetInstance(temp_oProspect, "bSIRProspect.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oProspect = temp_oProspect
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
        End If


        m_lReturn = oProspect.GetProspectData(v_vPartyCnt:=10, r_vWageRoll:=vWageRoll, r_vTurnover:=vTurnover, r_vAgentReference:=vAgentReference, r_vCurrentIntermediary:=vCurrentIntermediary, r_vProspectStatusID:=vProspectStatusID, r_vTargetPremium:=vTargetPremium, r_vCampaigns:=vCampaigns, r_vPolicies:=vPolicies)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("get data failed", Application.ProductName)
        End If

        Dim sSource As String = "PartyCnt : %s\n" & _
                                "WageRoll : %s\n" & _
                                "Turnover : %s\n" & _
                                "AgentReference %s\n" & _
                                "CurrentIntermediary : %s\n" & _
                                "ProspectStatusID : %s\n" & _
                                "TargetPremium : %s\n"

        m_lReturn = VBsprintf(sTarget, sSource, 10, vWageRoll, vTurnover, vAgentReference, vCurrentIntermediary, vProspectStatusID, vTargetPremium)

        MessageBox.Show(sTarget, Application.ProductName)


        oProspect.Dispose()
        oProspect = Nothing

        oObjectManager.Dispose()

    End Sub

    Sub TestAddProspect()
        Dim bSIRProspect As Object


        Dim oProspect As bSIRProspect.Prospect


        Dim vCampaigns, vPolicies As Object

        Dim sTarget, sSource As String

        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oProspect As Object
        m_lReturn = oObjectManager.GetInstance(temp_oProspect, "bSIRProspect.Prospect", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oProspect = temp_oProspect
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 9
        Dim vWageRoll As Integer = 12345
        Dim vTurnover As Integer = 6789
        Dim vAgentReference As String = DateTimeHelper.ToString(DateTimeHelper.Time)
        Dim vCurrentIntermediary As Byte = 0
        Dim vProspectStatusID As Byte = 2
        Dim vTargetPremium As Integer = 3000


        m_lReturn = oProspect.DirectAdd(vPartyCnt:=vPartyCnt, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vTargetPremium:=vTargetPremium)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("error directadd", Application.ProductName)
        End If


        oProspect.Dispose()
        oProspect = Nothing

        oObjectManager.Dispose()

    End Sub

    Sub TestAddPolicy()
        Dim bSIRProspect As Object


        Dim oPolicy As bSIRProspect.Policy

        Dim vPolicyID As String = ""
        Dim vRenewlDate As Object
        Dim sMsg As String = ""

        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oPolicy As Object
        m_lReturn = oObjectManager.GetInstance(temp_oPolicy, "bSIRProspect.Policy", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPolicy = temp_oPolicy
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 10

        Dim vPolicyTypeID As Byte = 1
        Dim vRenewalDate As Date = DateTime.Now
        Dim vNoOfTimesQuoted As Byte = 1


        m_lReturn = oPolicy.DirectAdd(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("error directadd", Application.ProductName)
            Exit Sub
        End If

        m_lReturn = VBsprintf(sMsg, "Added new policy\nPolicy ID: %d", vPolicyID)

        MessageBox.Show(sMsg, Application.ProductName)


        oPolicy.Dispose()
        oPolicy = Nothing

        oObjectManager.Dispose()

    End Sub

    Sub TestAddCampaign()
        Dim bSIRProspect As Object


        Dim oCampaign As bSIRProspect.Campaign

        Dim vRecordNo As String = ""
        Dim sMsg As String = ""

        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oCampaign As Object
        m_lReturn = oObjectManager.GetInstance(temp_oCampaign, "bSIRProspect.Campaign", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oCampaign = temp_oCampaign
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 10

        Dim vCampaignID As Byte = 1

        m_lReturn = oCampaign.DirectAdd(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("error directadd", Application.ProductName)
            Exit Sub
        End If

        m_lReturn = VBsprintf(sMsg, "Prospect Campaign added\nRecord No : %d", vRecordNo)

        MessageBox.Show(sMsg, Application.ProductName)


        oCampaign.Dispose()
        oCampaign = Nothing

        oObjectManager.Dispose()

    End Sub


    Sub TestDelCampaign()
        Dim bSIRProspect As Object


        Dim oCampaign As bSIRProspect.Campaign


        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oCampaign As Object
        m_lReturn = oObjectManager.GetInstance(temp_oCampaign, "bSIRProspect.Campaign", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oCampaign = temp_oCampaign
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 10
        Dim vRecordNo As Byte = 3


        m_lReturn = oCampaign.DirectDelete(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error delete", Application.ProductName)
        End If


        oCampaign.Dispose()
        oCampaign = Nothing

        oObjectManager.Dispose()

    End Sub

    Sub TestDelPolicy()
        Dim bSIRProspect As Object
        Dim oCampaign As Object


        Dim oPolicy As bSIRProspect.Policy


        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oPolicy As Object
        m_lReturn = oObjectManager.GetInstance(temp_oPolicy, "bSIRProspect.Policy", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPolicy = temp_oPolicy
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 10
        Dim vPolicyID As Byte = 5


        m_lReturn = oPolicy.DirectDelete(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error delete", Application.ProductName)
        End If


        oPolicy.Dispose()
        oCampaign = Nothing

        oObjectManager.Dispose()

    End Sub

    Sub TestDelProspect()
        Dim bSIRProspect As Object
        Dim oCampaign As Object


        Dim oProspect As bSIRProspect.Prospect


        Dim oObjectManager As New bObjectManager.ObjectManager

        m_lReturn = oObjectManager.Initialise(sCallingAppName:="TestStub")

        Dim temp_oProspect As Object
        m_lReturn = oObjectManager.GetInstance(temp_oProspect, "bSIRProspect.Prospect", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oProspect = temp_oProspect
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("failed", Application.ProductName)
            Exit Sub
        End If

        Dim vPartyCnt As Byte = 10


        m_lReturn = oProspect.DirectDelete(vPartyCnt:=vPartyCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error delete", Application.ProductName)
        End If


        oProspect.Dispose()
        oCampaign = Nothing

        oObjectManager.Dispose()

    End Sub
End Module