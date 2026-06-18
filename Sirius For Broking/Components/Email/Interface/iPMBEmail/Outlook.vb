Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.Office.Interop.Outlook
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Outlook_NET.Outlook")> _
Public NotInheritable Class Outlook

    Implements IDisposable
    Private Const ACClass As String = "Outlook"

    Private m_bIsOutlookInstalled As Boolean

    Public ReadOnly Property IsOutlookInstalled() As Boolean
        Get
            Return m_bIsOutlookInstalled
        End Get
    End Property

    Public ReadOnly Property olObject() As Object
        Get

        End Get
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        'Dim oOutlook As Object
        Dim sVersion As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue


        Try

            'Developer Guide 207
            Dim oOutlook As Microsoft.Office.Interop.Outlook._Application
            oOutlook = New Microsoft.Office.Interop.Outlook.Application

            If Information.Err().Number <> 0 Then
                m_bIsOutlookInstalled = False
                Return gPMConstants.PMEReturnCode.PMFalse
            Else


            End If

            If oOutlook Is Nothing Then
                m_bIsOutlookInstalled = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sVersion = oOutlook.Version
            If Information.Err().Number <> 0 Then
                m_bIsOutlookInstalled = False
                Return gPMConstants.PMEReturnCode.PMFalse
            Else

            End If

            oOutlook = Nothing

            Dim alRequiredVersion(3) As Integer
            alRequiredVersion(0) = 11
            alRequiredVersion(1) = 0
            alRequiredVersion(2) = 0
            alRequiredVersion(3) = 8002

            Dim alThisVersion(3) As Integer
            Dim asVersion() As String

            asVersion = sVersion.Split("."c)

            Dim lCount As Integer

            For Each asVersion_item As String In asVersion
                If lCount <= 3 Then
                    alThisVersion(lCount) = CInt(asVersion_item)
                    lCount += 1
                End If
            Next asVersion_item

            If alThisVersion(0) = alRequiredVersion(0) Then
                If alThisVersion(1) = alRequiredVersion(1) Then
                    If alThisVersion(2) = alRequiredVersion(2) Then
                        result = IIf(alThisVersion(3) >= alRequiredVersion(3), gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                    Else
                        result = IIf(alThisVersion(2) > alRequiredVersion(2), gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                    End If
                Else
                    result = IIf(alThisVersion(1) > alRequiredVersion(1), gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                End If
            Else
                result = IIf(alThisVersion(0) > alRequiredVersion(0), gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
            End If

            m_bIsOutlookInstalled = (result = gPMConstants.PMEReturnCode.PMTrue)

            Return result

Err_Initialise:

            m_bIsOutlookInstalled = False
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        Catch exc As System.Exception

        End Try
    End Function

    Public Function NewEmail(ByVal v_vToList As Object, ByVal v_sSubject As String, Optional ByVal v_vCCList As Object = Nothing, Optional ByVal v_vAttachmentFileList() As Object = Nothing, Optional ByVal v_vUserPropertyArray(,) As Object = Nothing, Optional ByVal v_sBody As String = "", Optional ByVal v_bSendImmediately As Boolean = False) As Integer

        Dim result As Integer = 0
        Const olByValue As Integer = 1
        Const olText As Integer = 1
        Const olMailItem As Integer = 0

        'Dim oOutlook, olNs, olMail As Object
        Dim olNs, olMail As Object
        Dim sToList, sCCList As String
        Dim oCommandBars, oCommandBar As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide 207
            Dim oOutlook As Microsoft.Office.Interop.Outlook._Application
            oOutlook = New Microsoft.Office.Interop.Outlook.Application
            olNs = oOutlook.GetNamespace("MAPI")


            olNs.Logon()


            olMail = oOutlook.CreateItem(olMailItem)

            If Information.IsArray(v_vToList) Then

                For lLoop As Integer = v_vToList.GetLowerBound(0) To v_vToList.GetUpperBound(0)

                    sToList = CDbl(v_vToList(lLoop)) + ";"
                Next lLoop
                sToList = CStr(1).Substring(CInt(sToList) - 1, Math.Min(CStr(1).Length, sToList.Length - 1))
            Else

                sToList = CStr(v_vToList)
            End If


            If Not Information.IsNothing(v_vCCList) Then
                If Information.IsArray(v_vCCList) Then
                    For Each v_vCCList_item As Object In v_vCCList

                        sCCList = CDbl(v_vCCList_item) + ";"
                    Next v_vCCList_item
                    sCCList = CStr(1).Substring(CInt(sCCList) - 1, Math.Min(CStr(1).Length, sCCList.Length - 1))
                Else
                    sCCList = v_vCCList
                End If
            End If

            With olMail


                .To = sToList

                .CC = sCCList

                .Subject = v_sSubject

                .Body = v_sBody


                If Not Information.IsNothing(v_vAttachmentFileList) Then
                    If Information.IsArray(v_vAttachmentFileList) Then
                        For Each v_vAttachmentFileList_item As Object In v_vAttachmentFileList

                            If gPMFunctions.FileExists(CStr(v_vAttachmentFileList_item)) Then

                                .Attachments.Add(v_vAttachmentFileList_item, olByValue, 1, v_vAttachmentFileList_item)
                            End If
                        Next v_vAttachmentFileList_item
                    End If
                End If


                .UserProperties.Add(USER_PROPERTY_ARCHIVE, olText, True)

                .UserProperties.Item(USER_PROPERTY_ARCHIVE).Value = "true"

                If Information.IsArray(v_vUserPropertyArray) Then
                    For lRow As Integer = v_vUserPropertyArray.GetLowerBound(1) To v_vUserPropertyArray.GetUpperBound(1)


                        Select Case CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                            Case USER_PROPERTY_DATABASE

                                .UserProperties.Add(USER_PROPERTY_DATABASE, olText, True)


                                .UserProperties.Item(USER_PROPERTY_DATABASE).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_BRANCH_CODE

                                .UserProperties.Add(USER_PROPERTY_BRANCH_CODE, olText, True)


                                .UserProperties.Item(USER_PROPERTY_BRANCH_CODE).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_CLIENT_KEY

                                .UserProperties.Add(USER_PROPERTY_CLIENT_KEY, olText, True)


                                .UserProperties.Item(USER_PROPERTY_CLIENT_KEY).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_INSURANCEFILE_KEY

                                .UserProperties.Add(USER_PROPERTY_INSURANCEFILE_KEY, olText, True)


                                .UserProperties.Item(USER_PROPERTY_INSURANCEFILE_KEY).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_INSURANCEFOLDER_KEY

                                .UserProperties.Add(USER_PROPERTY_INSURANCEFOLDER_KEY, olText, True)


                                .UserProperties.Item(USER_PROPERTY_INSURANCEFOLDER_KEY).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_CLAIM_KEY

                                .UserProperties.Add(USER_PROPERTY_CLAIM_KEY, olText, True)


                                .UserProperties.Item(USER_PROPERTY_CLAIM_KEY).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Case USER_PROPERTY_ORIGIN

                                .UserProperties.Add(USER_PROPERTY_ORIGIN, olText, True)


                                .UserProperties.Item(USER_PROPERTY_ORIGIN).Value = CStr(v_vUserPropertyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End Select

                    Next lRow
                End If

                If v_bSendImmediately Then

                    .Send()
                Else

                    .Display()

                    'Remove the Sirius commandbar if present

                    oCommandBars = olMail.GetInspector.CommandBars

                    If Not (oCommandBars Is Nothing) Then

                        For lLoop As Integer = 1 To oCommandBars.Count

                            oCommandBar = oCommandBars.Item(lLoop)

                            If oCommandBar.Name = "Sirius Commandbar" Then

                                oCommandBar.Delete()
                            End If
                        Next lLoop
                    End If

                End If

            End With


            olNs.Logoff()

            olNs = Nothing
            olMail = Nothing
            oOutlook = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            olNs.Logoff()


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to send the New Mail", vApp:=ACApp, vClass:=ACClass, vMethod:="NewEMail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class