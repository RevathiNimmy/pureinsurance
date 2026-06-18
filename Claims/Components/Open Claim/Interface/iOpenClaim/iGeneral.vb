'GetResData is replaced in the whole project by GetResData, and a function is also made
Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 17/02/1997
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    
    Public objfrmInterface As frmInterface
    Private Const ACClass As String = "General"

    ' Private instance of the interface form.
    Private m_frmInterface As Form

    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oBusiness = Nothing
                m_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.

            'NIIT - Replaced with the Migrated code 1144 
            'm_lReturn = m_frmInterface.GetBusiness()
            m_lReturn = ReflectionHelper.Invoke(m_frmInterface, "GetBusiness", New Object() {})

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Dim lOriginalClaimID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.

            'NIIT - Replaced with the Migrated code 1144 
            'If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
            If ReflectionHelper.GetMember(m_frmInterface, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else

                    'NIIT - Replaced with the Migrated code 1144 
                    '					If frmInterface.TransactionType <> "C_CO" Then
                    If ReflectionHelper.GetMember(m_frmInterface, "TransactionType") <> "C_CO" Then

                        m_lReturn = g_oBusiness.GetOriginalClaimNo(v_lClaimId:=g_lClaimID, r_lOriginalClaimID:=lOriginalClaimID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' TB 16/12/02: No lock in view mode
                        'PSL 15/07/2003 have over-ridden VIEW mode with DUMMYDELETE at this point in an effort to
                        'make it work like it used to before the VIEW roadmap
                        'If frmInterface.Task <> PMView Then

                        
                        m_lReturn = CType(ReflectionHelper.Invoke(m_frmInterface, "UnlockClaim", New Object() {lOriginalClaimID}), gPMConstants.PMEReturnCode)

                        '                End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        g_oBusiness.Claimid = g_lClaimID

                        ' only delete if this is not view mode
                        'NIIT - Replaced with the Migrated code 1144 
                        'If frmInterface.Task <> gPMConstants.PMEComponentAction.PMView Then
                        If ReflectionHelper.GetMember(m_frmInterface, "Task") <> gPMConstants.PMEComponentAction.PMView Then
                            '*****************
                            ' MEvans : 07-01-2004 : CQ3414
                            ' delete the work gis policy link that was created for the view
                            m_lReturn = CType(DeleteGisDetails(v_lWorkClaimId:=g_lClaimID), gPMConstants.PMEReturnCode)
                            '*****************


                            m_lReturn = g_oBusiness.DeleteClaim

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return result
                            End If
                        End If

                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '##ModelId=3957D10100F9
    ' ***************************************************************** '
    ' Name: DeleteGisDetails
    '
    ' Parameters: n/a
    '
    ' Description: Deletes work gis dataset and gis policy link details
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Private Function DeleteGisDetails(ByVal v_lWorkClaimId As Integer) As Integer
        Dim result As Integer = 0

        Const sFunctionName As String = "DeleteGisDetails"


        Dim oRiskDetails As bCLMRiskDetails.Business


        result = gPMConstants.PMEReturnCode.PMTrue

        ' copy claim to work table
        Dim temp_oRiskDetails As Object
        If g_oObjectManager.GetInstance(temp_oRiskDetails, "bCLMRiskDetails.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) = gPMConstants.PMEReturnCode.PMTrue Then
            oRiskDetails = temp_oRiskDetails


            If oRiskDetails.DeleteGISDetails(v_lClaimId:=v_lWorkClaimId) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error
                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to delete work gis records for work claim id:" & CStr(v_lWorkClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

        Else
            oRiskDetails = temp_oRiskDetails

            ' Log Error
            result = gPMConstants.PMEReturnCode.PMFalse

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get instance of bCLMRiskDetails.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

        End If



        ' destroy object reference
        oRiskDetails = Nothing

        Return result

    End Function
End Class

