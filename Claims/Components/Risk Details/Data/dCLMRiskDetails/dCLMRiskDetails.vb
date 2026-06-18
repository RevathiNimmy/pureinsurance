Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  {TodaysDate}
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "dCLMRiskDetails"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"




    ' index value
    Public Const g_iClaim As Integer = 1
    Public Const g_iRiskType As Integer = 2
    Public Const g_iRiskDataDefn As Integer = 3

    'Resizing Constants*******************************************************
    ' ACTopOfTabInform is for setting the top of the SSTab control on the form.
    Const ACTopOfTabInform As Integer = 120

    ' ACLeftOfTabInform is for setting the left of the SSTab control on the form.
    Const ACLeftOfTabInform As Integer = 120

    ' ACTopOfFirstFrameInTab is for setting the top of the first frame control on the form.
    Const ACTopOfFirstFrameInTab As Integer = 480

    ' ACLeftOfFrameInTab is for setting the left of the frame control contained the SSTab.
    Const ACLeftOfFrameInTab As Integer = 240

    ' ACLabelHeight is for setting the height of a label
    Const ACLabelHeight As Integer = 255
    Const ACLabelWidthMedium As Integer = 1575

    ' Text box constants.
    Const ACTextBoxHeight As Integer = 285
    Const ACTopOfFirstTextBoxInTab As Integer = 540
    Const ACNormalGapBetweenTopsOfTwoTextBoxes As Integer = 360
    Const ACButtonHeight As Integer = 330
    Const ACButtonWidth As Integer = 1095

    Const ACLeftOFLabelInFirstColumn As Integer = 480
    Const ACMinimumFormHeight As Integer = 4965
    Const ACMinimumFormWidth As Integer = 8085
    Const ACTwice As Integer = 2
    Const ACFormBorder As Integer = 120
    Const ACThrice As Integer = 3
    Const ACDiffBetweenTopsOfLabelAndText As Integer = 45
End Module