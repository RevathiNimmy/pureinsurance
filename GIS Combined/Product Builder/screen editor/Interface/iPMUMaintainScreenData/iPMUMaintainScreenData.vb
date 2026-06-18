Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 10/05/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUMaintainScreenData"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACLblRiskGroup As Integer = 102
    Public Const ACFraStucture As Integer = 103
    Public Const ACFraCommon As Integer = 104
    Public Const ACLblDataType As Integer = 105
    Public Const ACLblDescription As Integer = 106
    Public Const ACLblCaption As Integer = 107
    Public Const ACFraRange As Integer = 108
    Public Const ACLblFrom As Integer = 109
    Public Const ACLblTo As Integer = 110
    Public Const ACFraLookup As Integer = 111
    Public Const ACLblAllowedValues As Integer = 112
    Public Const ACFraDateRange As Integer = 113
    Public Const ACLblDateFrom As Integer = 114
    Public Const ACLblDateTo As Integer = 115

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    'Node Title
    Public Const ACRiskDataDictionary As Integer = 304
    Public Const ACCaseDataDictionary As Integer = 305

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    'Array constants for the Data Dictionary
    Public Const ACOGISObjectId As Integer = 0
    Public Const ACOGISDataModelId As Integer = 1
    Public Const ACOObjectName As Integer = 2
    Public Const ACOTableName As Integer = 3
    Public Const ACOMaxInstances As Integer = 4
    Public Const ACOIsQuoteObject As Integer = 5
    Public Const ACOParentObjectId As Integer = 6
    Public Const ACOPolarisObjectId As Integer = 7
    Public Const ACOIsSelectable As Integer = 8
    Public Const ACOIsNonGIS As Integer = 9
    Public Const ACPGISPropertyId As Integer = 10
    Public Const ACPGISObjectId As Integer = 11
    Public Const ACPPropertyName As Integer = 12
    Public Const ACPColumnName As Integer = 13
    Public Const ACPDataType As Integer = 14
    Public Const ACPIsInputProperty As Integer = 15
    Public Const ACPIsIdentifyingProperty As Integer = 16
    Public Const ACPIsPrimaryKey As Integer = 17
    'GSD deleted
    'Public Const ACPGISListId = 18
    Public Const ACPPolarisPropertyId As Integer = 18
    Public Const ACPIsDeleted As Integer = 19
    Public Const ACPIsSearchProperty As Integer = 20
    'GSD
    'Public Const ACPLookupTableName = 22
    'Public Const ACPPartyTypeId = 23
    'Public Const ACPSumInsuredType = 24
    'Public Const ACPStdWordingType = 25
    'Public Const ACPGISUserDefHeaderId = 26
    'Public Const ACPProductId = 27
    Public Const ACPEditFlags As Integer = 21
    Public Const ACPSpecialsType As Integer = 22
    Public Const ACPSpecialsTypeReference As Integer = 23
    'Tomo151002
    'Public Const ACPIsComment = 26


    Public Const ACOpenFolder As String = "Open"
    Public Const ACClosedFolder As String = "Closed"

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    ' Counter for g_oObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iObjectManagerCount As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bGetEnableClaimVersions As Boolean


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Sub Main_Renamed()

    End Sub
    ' ***************************************************************** '
    '
    ' Name: PopulateListView
    '
    ' Description:
    '
    ' History: 10/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    ' Developer Guide No. 
    Public Function PopulateListView(ByRef lListViewIndex As Integer, ByRef lChildId As Integer, ByRef bClear As Boolean, ByRef lvwListView() As Object, ByRef m_vChildScreenDetails(,) As Object, ByRef r_bSequenceControls As Boolean, Optional ByVal cToolTip As ToolTip = Nothing) As Integer

        Dim result As Integer = 0
        Dim vColumnOrder(,) As Object
        Dim lTemp2 As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'Developer Guide No. 
            lvwListView(lListViewIndex).Columns.Clear()

            If Not Information.IsArray(m_vChildScreenDetails) Then
                Return result
            End If

            lTemp2 = -1 'column order array index
            r_bSequenceControls = False 'signal no sequence controls
            For lTemp As Integer = m_vChildScreenDetails.GetLowerBound(1) To m_vChildScreenDetails.GetUpperBound(1)

                If CDbl(m_vChildScreenDetails(PBDatabaseConsts.ACDGISScreenId, lTemp)) = lChildId Then

                    If Not (Convert.IsDBNull(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)) Or IsNothing(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp))) Then

                        If CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)).ToLower() = GISSharedPropertyConstants.cProperty_SequenceId Then
                            r_bSequenceControls = True
                        End If
                    End If

                    If Not (Convert.IsDBNull(m_vChildScreenDetails(PBDatabaseConsts.ACDColumnWidth, lTemp)) Or IsNothing(m_vChildScreenDetails(PBDatabaseConsts.ACDColumnWidth, lTemp))) Then
                        lTemp2 += 1
                        If lTemp2 = 0 Then
                            ReDim vColumnOrder(2, lTemp2)
                        Else
                            ReDim Preserve vColumnOrder(2, lTemp2)
                        End If

                        If Not (Convert.IsDBNull(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)) Or IsNothing(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp))) Then



                            ' Developer Guide No. 
                            If Information.IsArray(m_vChildScreenDetails) Then
                                If Not (Convert.IsDBNull(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)) Or IsNothing(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp))) Then
                                    cToolTip.SetToolTip(lvwListView(lListViewIndex), CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)))
                                End If
                            End If
                        End If
                        'add this column and its required position into the array
                        'DC160503 -ISS4099 -use proper column position for child screen array
                        ' RAW 08/07/2003 : CQ1335 : reversed earlier change since column positions have been realigned in sp


                        'vColumnOrder(0, lTemp2) = m_vChildScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp) 'sort order

                        'vColumnOrder(1, lTemp2) = lTemp 'column idetifier


                        'vColumnOrder(2, lTemp2) = PBRiskScreenCommon.StripColonFromCaption(CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDCaption, lTemp))) 'just to make array easier to debug

                        vColumnOrder(0, lTemp2) = m_vChildScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp) 'sort order
                        vColumnOrder(1, lTemp2) = CStr(lTemp) 'column idetifier
                        vColumnOrder(2, lTemp2) = PBRiskScreenCommon.StripColonFromCaption(m_vChildScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)) 'just to make array easier to debug





                        'if blank caption then blank it!

                        If CStr(vColumnOrder(2, lTemp2)) = PBRiskScreenCommon2.ACBlankCaption Then

                            vColumnOrder(2, lTemp2) = ""
                        Else
                            'convert any double ampersands to single


                            vColumnOrder(2, lTemp2) = CStr(vColumnOrder(2, lTemp2)).Replace("&&", "&")
                        End If
                        'set tooltip (can't see any other way of doing this)
                    End If
                End If
            Next lTemp

            If Information.IsArray(vColumnOrder) Then 'only display if any fields in list

                PBRiskScreenCommon.SortThreeElementArray(vColumnOrder)


                For lTemp As Integer = vColumnOrder.GetLowerBound(1) To vColumnOrder.GetUpperBound(1)

                    lvwListView(lListViewIndex).Columns.Add("D" & lTemp, vColumnOrder(2, lTemp))



                    lvwListView(lListViewIndex).Columns("D" & lTemp).Width = m_vChildScreenDetails(PBDatabaseConsts.ACDWidth, CInt(vColumnOrder(1, lTemp)))
                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: InputBoxEx
    '
    ' Description: Replace vba.InputBox
    '
    ' History: 17/3/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '

    Public Function InputBoxEx(ByRef Prompt As String, Optional ByRef Title As String = "Screen Designer", Optional ByRef Default_Renamed As String = "") As String


        Dim result As String = String.Empty
        Dim frmInput As New frmInputBox


        'Akash: Commented because load event is automatically called when ShowDialog is used
        'Load(frmInput)

        With frmInput
            .Prompt = Prompt
            .Text = Title
            .Value = IIf(Default_Renamed = PBRiskScreenCommon2.ACBlankCaption, "", Default_Renamed)

            .ShowDialog()

            'If OK not clicked then returns [CANCELED]
            result = .Value


        End With
        frmInput.Close()

        Return result
    End Function

    ' Developer Guide No. 

    Public Function FindControlName(ByVal root As Control, ByVal target As String) As Control

        If root.Name.ToUpper.Contains(target.ToUpper) Then
            Return root
        End If
        For iCtlCtr As VariantType = 0 To root.Controls.Count - 1
            If root.Controls(iCtlCtr).Name.ToUpper.Contains(target.ToUpper) Then
                Return root.Controls(iCtlCtr)
            End If
        Next
        For iCtlCtr As VariantType = 0 To root.Controls.Count - 1
            Dim result As Control
            For iCtlChildCtr As VariantType = 0 To root.Controls(iCtlCtr).Controls.Count - 1
                result = FindControlName(root.Controls(iCtlCtr).Controls(iCtlChildCtr), target)
                If result IsNot Nothing Then
                    Return result
                End If
            Next
        Next
        Return Nothing
    End Function
    'To be converted in Pixels

    'Public Sub ScreenDetailsToPixel(ByRef vArrDetail(,) As Object)
    Public Sub TwipsToPixcelFormat(ByRef arrScreenValue(,) As Object, ByVal Type As String)
        If Type.ToUpper() = "DETAILS" Then
            For lTemp As Integer = arrScreenValue.GetLowerBound(1) To arrScreenValue.GetUpperBound(1)
                arrScreenValue(PBDatabaseConsts.ACDTop, lTemp) = VB6.TwipsToPixelsX(arrScreenValue(PBDatabaseConsts.ACDTop, lTemp))
                arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp) = VB6.TwipsToPixelsX(arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp))
                arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp) = VB6.TwipsToPixelsY(arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp))
                arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp) = VB6.TwipsToPixelsY(arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp))

            Next
        ElseIf Type.ToUpper() = "HEADER" Then
            For lTemp As Integer = arrScreenValue.GetLowerBound(1) To arrScreenValue.GetUpperBound(1)
                arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp) = VB6.TwipsToPixelsY(arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp))
                arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp) = VB6.TwipsToPixelsX(arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp))

            Next
        End If

    End Sub
    Public Sub PixcelToTwipsFormat(ByRef arrScreenValue(,) As Object, ByVal Type As String)
        Try
            If Type.ToUpper() = "DETAILS" Then
                For lTemp As Integer = arrScreenValue.GetLowerBound(1) To arrScreenValue.GetUpperBound(1)
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACDTop, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACDTop, lTemp) = 0
                    End If
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp) = 0
                    End If
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp) = 0
                    End If
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp) = 0
                    End If
                    arrScreenValue(PBDatabaseConsts.ACDTop, lTemp) = VB6.PixelsToTwipsX(arrScreenValue(PBDatabaseConsts.ACDTop, lTemp))
                    arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp) = VB6.PixelsToTwipsX(arrScreenValue(PBDatabaseConsts.ACDWidth, lTemp))
                    arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp) = VB6.PixelsToTwipsY(arrScreenValue(PBDatabaseConsts.ACDHeight, lTemp))
                    arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp) = VB6.PixelsToTwipsY(arrScreenValue(PBDatabaseConsts.ACDLeft, lTemp))

                Next
            ElseIf Type.ToUpper() = "HEADER" Then

                For lTemp As Integer = arrScreenValue.GetLowerBound(1) To arrScreenValue.GetUpperBound(1)
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp) = 0
                    End If
                    If Convert.ToString(arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp)) = "" Then
                        arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp) = 0
                    End If
                    arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp) = VB6.PixelsToTwipsY(arrScreenValue(PBDatabaseConsts.ACHScreenHeight, lTemp))
                    arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp) = VB6.PixelsToTwipsX(arrScreenValue(PBDatabaseConsts.ACHScreenWidth, lTemp))

                Next
            End If
        Catch excep As System.Exception



            '  result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Return result

        End Try
    End Sub

End Module
