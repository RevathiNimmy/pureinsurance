Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

Imports SharedFiles
Module Configuration
	
	Private Const ACClass As String = "Configuration"
	
	Private m_lReturn As Integer
	
	' "Roadmap" name
    'Public m_sRoadmap As String = ""
    <ThreadStatic()> _
    Public m_sRoadmap As String
    <ThreadStatic()> _
    Public m_sRoadmapTemp As String = ""
	
    ' End the map or not?
    <ThreadStatic()> _
 Public m_bEndMap As Boolean
	
    ' Auto close the map or not?
    <ThreadStatic()> _
    Public m_bAutoClose As Boolean
    <ThreadStatic()> _
    Public m_bAutoCloseTemp As Boolean
	
    ' Navigator or user driven?
    <ThreadStatic()> _
 Public m_bNavigatorDriven As Boolean
	
    ' CTAF 230502 - Used for SetProcessModes
    <ThreadStatic()> _
    Public g_vTransactionType As String = ""
    <ThreadStatic()> _
    Public g_vTransactionTypeTemp As String = ""
    <ThreadStatic()> _
    Public g_vProcessMode As String = ""
    <ThreadStatic()> _
    Public g_vProcessModeTemp As String = ""
	
    ' Carole Nash web site
    <ThreadStatic()> _
    Public m_sImageURL As String = ""
    <ThreadStatic()> _
    Public m_sImageURLTemp As String = ""
	
    ' Title of the form
    <ThreadStatic()> _
    Public g_sTitle As String = ""
    <ThreadStatic()> _
    Public g_sTitleTemp As String = ""
	
    ' Number of steps in the map
    <ThreadStatic()> _
    Public g_lNumberOfSteps As Integer
    <ThreadStatic()> _
    Public g_sWMTaskCode As String = ""
    <ThreadStatic()> _
    Public g_sWMTaskCodeTemp As String = ""
    <ThreadStatic()> _
    Public g_sWMTaskDescription As String = ""
    <ThreadStatic()> _
    Public g_sWMTaskDescriptionTemp As String = ""
	
	' Reset module-level Key Array when Restarting?
	' The default value is True - PN17474
    Private m_bResetKeysOnRestart As Boolean
    Public ReadOnly Property ResetKeysOnRestart() As Boolean
        Get
            Return m_bResetKeysOnRestart
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: ValidateMap
    '
    ' Description: Validates the map for crazy steps.
    '
    ' History: 16/10/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateMap(ByVal v_vMapProperties As Object, ByRef r_vSteps() As MainModule.Step_Renamed) As Integer

        Dim result As Integer = 0
        Dim lEndStep As Integer

        

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bNavigatorDriven Then

                ' Does the first step try and go backwards?
                If (r_vSteps(0).CancelAction = gPMConstants.PMNavActionBackOne) Or (r_vSteps(0).CancelAction = gPMConstants.PMNavActionBackX) Or (r_vSteps(0).OKAction = gPMConstants.PMNavActionBackOne) Or (r_vSteps(0).OKAction = gPMConstants.PMNavActionBackX) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The first step tries to go backwards.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMap", excep:=New Exception("Invalid steps"))

                    Return result

                End If

                ' Get the end step
                lEndStep = r_vSteps.GetUpperBound(0)

                If (r_vSteps(lEndStep).OKAction = gPMConstants.PMNavActionForwardOne) Or (r_vSteps(lEndStep).OKAction = gPMConstants.PMNavActionForwardX) Or (r_vSteps(lEndStep).CancelAction = gPMConstants.PMNavActionForwardOne) Or (r_vSteps(lEndStep).CancelAction = gPMConstants.PMNavActionForwardX) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The last step tries to go off the end of the map.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMap", excep:=New Exception("Invalid steps"))

                    Return result

                End If

            End If

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: LoadMap
    '
    ' Description: Loads the map from the XML file
    '
    ' History: 21/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LoadMap(ByVal v_sXMLFileName As String, ByRef r_sRoadmapPath As String) As Integer

        Dim result As Integer = 0
        Dim vMap As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' HKEYLM\Software\PM\SiriusArchitecture\Common = NavigatorXMPath

            ' This is stored in the registry somewhere
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=ACNavigatorXMPath, r_sSettingValue:=r_sRoadmapPath)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get registry value of NavigatorXMPath", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadMap", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Check we have a trailing \
            If Not r_sRoadmapPath.EndsWith("\") Then
                r_sRoadmapPath = r_sRoadmapPath & "\"
            End If

            ' Load the map from the XML file
            m_lReturn = LoadRoadmapFromXML(v_sRoadmapPath:=r_sRoadmapPath, v_sXMLFile:=v_sXMLFileName, r_vMapProperties:=vMap, r_vSteps:=m_vSteps)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bNavigatorDriven = CBool(vMap(XMLFunc.pmeXMLMapTypes.NavigatorDriven))

            ' Validate the map
            m_lReturn = ValidateMap(v_vMapProperties:=vMap, r_vSteps:=m_vSteps)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the map properties

            m_sRoadmap = CStr(vMap(XMLFunc.pmeXMLMapTypes.RoadmapName))

            m_bAutoClose = CBool(vMap(XMLFunc.pmeXMLMapTypes.AutoClose))

            g_vTransactionType = CStr(vMap(XMLFunc.pmeXMLMapTypes.TransactionType))

            g_vProcessMode = CStr(vMap(XMLFunc.pmeXMLMapTypes.ProcessMode))

            m_sImageURL = CStr(vMap(XMLFunc.pmeXMLMapTypes.ImageURL))

            g_sWMTaskCode = CStr(vMap(XMLFunc.pmeXMLMapTypes.WMTaskCode))

            g_sWMTaskDescription = CStr(vMap(XMLFunc.pmeXMLMapTypes.WMTaskDescription))

            g_sTitle = CStr(vMap(XMLFunc.pmeXMLMapTypes.Title))

            m_bResetKeysOnRestart = CBool(vMap(XMLFunc.pmeXMLMapTypes.ResetKeysOnRestart)) 'PN17474

            ' Store the number of steps we have
            g_lNumberOfSteps = m_vSteps.GetUpperBound(0)
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadMap", excep:=excep)

            Return result

        End Try
    End Function
End Module

