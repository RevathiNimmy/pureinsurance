Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Module MainModule

#Region "Public Constants"

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iSIRMIDMaintenance"

    ' Task Group User Group Data Array Positions
    Public Const ACTaskGroupUserGroup_TaskGroupId As Integer = 0
    Public Const ACTaskGroupUserGroup_UserGroupId As Integer = 2
    Public Const ACTaskGroupUserGroup_UserGroupDescription As Integer = 3

    Public Const ACCancelDetailsTitle As Integer = 700
    Public Const ACCancelDetails As Integer = 701
    Public Const ACBusinessFailTitle As Integer = 702
    Public Const ACBusinessFail As Integer = 703
    Public Const ACNoSelectionTitle As Integer = 710
    Public Const ACNoSelectionDetails As Integer = 711
    Public Const ACConfirmDeleteTitle As Integer = 712
    Public Const ACConfirmDeleteDetails As Integer = 713
    Public Const ACRuleUsedTitle As Integer = 714
    Public Const ACRuleUsedDetails As Integer = 715

    ' lookup tables
    Public Const ACLookupTablePMWrkTaskGroup As String = "PMWrk_Task_Group"

    ' lookup detail constants
    Public Const ACDetailKey As Integer = 0
    Public Const ACDetailDesc As Integer = 1
    Public Const ACDetailCode As Integer = 2

#End Region

#Region "Public Variables"

    <ThreadStatic()> _
    Public ofrmDetails As frmMIDRuleConfiguration

    ' Public source and language ID's from the Object Manager.
    Public g_nSourceID As Integer
    Public g_nLanguageID As Integer
    Public g_sUserName As String = ""

    ' Extra variables for component services
    Public g_sPassword As String = ""
    Public g_nUserID As Integer
    Public g_nCurrencyID As Integer
    Public g_nLogLevel As Integer

    ' Public instance of the object manager.
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

#End Region

#Region "Public Constants"

    ' Constant for the functions to identify 
    ' which class this is.
    Private Const ACClass As String = "MainModule"

#End Region

#Region "Private Variables"

    Private m_oSystemOption As bSIROptions.Business

#End Region

#Region "Public Functions"

    ''' <summary>
    ''' Get an option.
    ''' </summary>
    ''' <param name="v_nOptionNumber"></param>
    ''' <param name="r_sOptionValue"></param>
    ''' <param name="r_oDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOption(ByVal v_nOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef r_oDatabase As Object = Nothing) As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            If m_oSystemOption Is Nothing Then
                ' Get an instance of the business object via the public object manager.
                nResult = g_oObjectManager.GetInstance(m_oSystemOption, "bSIROptions.Business", vInstanceManager:="ClientManager")
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            nResult = m_oSystemOption.GetOption(iOptionNumber:=v_nOptionNumber, sValue:=r_sOptionValue)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sOptionValue = "0"
            End If

            m_oSystemOption.Dispose()
            m_oSystemOption = Nothing
        Catch Excep As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Displays a message
    ''' </summary>
    ''' <param name="r_nTitleId"></param>
    ''' <param name="r_nMessageId"></param>
    ''' <param name="r_nOptions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DisplayMessage(ByRef r_nTitleId As Integer, ByRef r_nMessageId As Integer, ByRef r_nOptions As Integer) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim sTitle, sMessage As String
        Try
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=r_nTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=r_nMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            nResult = Interaction.MsgBox(sMessage, r_nOptions, sTitle)

        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End Try

        Return nResult
    End Function

#End Region

End Module
