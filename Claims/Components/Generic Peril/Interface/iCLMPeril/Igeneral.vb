Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
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
    ' Date: 22/08/2000
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' CJB 170605 PN21855 Fix failure in CreateAuthorisationTask as party_cnt was 0
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"

    ' Private instance of the interface form.
    Private m_frmInterface As Object

    ' Private instance of the business object.
    Private m_oBusiness As Object
    Private m_oWrkTaskInstanceTemp As Object

    Private m_lmandatory As gPMConstants.PMEMandatoryStatus

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' Primary Keys to work with

    ' list item object declared
    Private m_lstitem As ListViewItem

    Private lDriverTag As Integer
    Private lThirdPartyTag As Integer
    Private lRepairerTag As Integer
    Private lWitnessTag As Integer
    Private sGeneralText As String = ""
    Public Property GeneralText() As String
        Get
            Return sGeneralText
        End Get
        Set(ByVal Value As String)
            sGeneralText = Value
        End Set
    End Property

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property
    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property
    Public Property PerilTypeID() As Integer
        Get
            Return m_lPerilTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeID = Value
        End Set
    End Property
    Public Property Partycnt() As Integer
        Get
            Return m_lPartycnt
        End Get
        Set(ByVal Value As Integer)
            'UNCOMMENT FOR INTEGRATION*******************************************
            'm_lpartycnt = lpartycnt
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '


    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As bCLMPeril.Business) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            g_oBusiness = oBusiness

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
                g_oBusiness = Nothing
                DestroyWrkTaskInstances()
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
        Dim NewLargeChange As Integer
        Dim r_vControlsArray(,) As Object
        Dim lcontrolcount, llabelcount As Integer
        Dim r_vLookupArray(,) As Object
        Dim lTop As Integer
        Dim lngLastTabID As Integer
        Dim intCurrentTab As Integer
        Dim lngCurrentTabID As Integer
        Dim intTabCount As Integer
        Dim bFirstDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' setting the tag property vales to 0
            lDriverTag = 0
            lThirdPartyTag = 0
            lRepairerTag = 0
            lWitnessTag = 0

            ' Check the task.

            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Move from setdefaults
            ' Display all language specific captions.
            m_lReturn = objfrmInterface.DisplayCaptions()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = g_oBusiness.GetControls(r_vControlsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Analyze the values in the Control Array

            If Not Information.IsArray(r_vControlsArray) Or Object.Equals(r_vControlsArray, Nothing) Then
                'ED 03102002 - Hide tabs 1-4, Tab for tab specific user defined data,
                '              only used in Broking.
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, 1, False)
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, 2, False)
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, 3, False)
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, 4, False)
                'ED - End

                ' Change the captions for other tabs.
                SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, ACReserve, " - Reserve")
                SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, ACPayment, " - Payment")
                If SSTabHelper.GetTabVisible(objfrmInterface.SSTab1, ACComments) Then
                    SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, ACComments, " - Comments")
                End If

                'AR - PN34860

                GetTabCaption()

                If m_lClaimID <> 0 Then

                    m_lReturn = g_oBusiness.GetClaimCurrency(v_lClaimID:=m_lClaimID, r_lCurrencyID:=m_lCurrencyID, r_sCurrencyDesc:=m_sCurrencyDesc)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get the claim currency")
                    End If
                End If

                ' GetInterfaceDetails = PMFalse
                Return result
            Else

                lngLastTabID = -999
                intCurrentTab = -1

                intTabCount = CInt(r_vControlsArray(g_cICTabCount, 0))

                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACGeneral, True)
                'DC030703 -ISS4415 -set default tabe title
                SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, ACGeneral, " - General Details")

                lTop = 120
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACGeneral, True)

                For lRow As Integer = r_vControlsArray.GetLowerBound(1) To r_vControlsArray.GetUpperBound(1)
                    'lTop = frmInterface.GetPos				

                    lngCurrentTabID = CInt(r_vControlsArray(g_cICTabID, lRow))
                    If lngCurrentTabID <> lngLastTabID Then
                        intCurrentTab += 1
                        SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, intCurrentTab, True)
                        SSTabHelper.SetSelectedIndex(objfrmInterface.SSTab1, intCurrentTab)

                        SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, intCurrentTab, " - " & CStr(r_vControlsArray(g_cICTabCaption, lRow)))


                        objfrmInterface.fraGeneral(intCurrentTab).Text = CStr(r_vControlsArray(g_cICTabCaption, lRow))
                        lTop = 120
                        'DC110703 -ISS4596 -set this otherwise first two sit on top of one another
                        bFirstDone = True
                    Else

                        If (CDbl(r_vControlsArray(g_cICAtype, lRow)) >= 1) And (CDbl(r_vControlsArray(g_cICAtype, lRow)) <= 5) Then
                            If bFirstDone Then
                                lTop = lTop + 315 + 120
                            Else
                                bFirstDone = True
                            End If
                        End If
                    End If
                    If lTop > VB6.PixelsToTwipsY(objfrmInterface.Picture1(intCurrentTab).Height) Then
                        objfrmInterface.Picture1(intCurrentTab).Height = VB6.TwipsToPixelsY(1.5 * VB6.PixelsToTwipsY(objfrmInterface.Picture1(intCurrentTab).Height))
                        objfrmInterface.VScroll_Renamed(intCurrentTab).Visible = True
                        objfrmInterface.VScroll_Renamed(intCurrentTab).Minimum = 0
                        objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum = (100 + objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange - 1) 'frmInterface.Picture2.Height
                        'frmInterface.Picture2.Height =
                        NewLargeChange = (objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum - (objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange + 1)) / 3
                        objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum = objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum + NewLargeChange - objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange
                        objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange = NewLargeChange
                        objfrmInterface.VScroll_Renamed(intCurrentTab).SmallChange = 1
                    End If

                    Select Case (r_vControlsArray(g_cICAtype, lRow))
                        Case 1 'Text

                            lcontrolcount = objfrmInterface.txtBox.Length ' + 1
                            llabelcount = objfrmInterface.lbl.Length  ' + 1

                            ReDim Preserve intControlTypes(lcontrolcount)
                            intControlTypes(lcontrolcount) = 1

                            ' load the controls
                            'developer guide no. Todo list
                            ' ContainerHelper.LoadControl(Me, "Item", lcontrolcount)
                            ContainerHelper.LoadControl(objfrmInterface, "txtBox", lcontrolcount, True)

                            'sg
                            objfrmInterface.txtBox(lcontrolcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            'developer guide no. Todo list
                            'ContainerHelper.LoadControl(Me, "Item", llabelcount)
                            ContainerHelper.LoadControl(objfrmInterface, "lbl", llabelcount, True)

                            'sg
                            objfrmInterface.lbl(llabelcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.txtBox(lcontrolcount).Visible = True
                            objfrmInterface.lbl(llabelcount).Visible = True
                            ' arrange the positions
                            objfrmInterface.txtBox(lcontrolcount).Top = VB6.TwipsToPixelsY(lTop)
                            objfrmInterface.lbl(llabelcount).Top = VB6.TwipsToPixelsY(lTop)

                            ' load the values for the text box
                            'sg
                            objfrmInterface.txtBox(lcontrolcount).Text = CStr(r_vControlsArray(g_cICAvalue, lRow))
                            'sg
                            objfrmInterface.txtBox(lcontrolcount).Tag = CStr(r_vControlsArray(g_ciCAuserdefperildataid, lRow))
                            GeneralText = objfrmInterface.txtBox(lcontrolcount - 1).Text
                            If Information.IsArray(vDataTypeArray) Then

                                ReDim Preserve vDataTypeArray(1, vDataTypeArray.GetUpperBound(1) + 1)
                            Else
                                ReDim vDataTypeArray(1, 1)
                            End If
                            vDataTypeArray(0, vDataTypeArray.GetUpperBound(1)) = "Text"
                            vDataTypeArray(1, vDataTypeArray.GetUpperBound(1)) = r_vControlsArray(g_ciCAuserdefperildataid, lRow)

                            ' change the caption name
                            'sg
                            objfrmInterface.lbl(llabelcount).Text = CStr(r_vControlsArray(g_ciCAcaption, lRow)) & " :"

                            ' check whether read_only
                            'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                            'DC280302 -start -added check for Underwriting/Broking


                            If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                'sg
                                objfrmInterface.txtBox(lcontrolcount).Enabled = False
                            Else
                                'sg
                                objfrmInterface.txtBox(lcontrolcount).Enabled = True
                            End If


                            'DC280302 -end

                            'DC280302 -start - check for underwriting/broking



                            If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMMandatory

                                'sg
                                objfrmInterface.lbl(llabelcount).Font = VB6.FontChangeBold(objfrmInterface.lbl(llabelcount).Font, True)
                                objfrmInterface.lbl(llabelcount).Tag = "txtBox" & lcontrolcount
                            Else
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory

                                'sg
                                objfrmInterface.lbl(llabelcount).Tag = CStr(0)
                            End If


                            'DC280302 -end

                        Case 2 'Integer
                            lcontrolcount = objfrmInterface.txtBox.Length  ' + 1
                            llabelcount = objfrmInterface.lbl.Length  '+ 1
                            ReDim Preserve intControlTypes(lcontrolcount)
                            intControlTypes(lcontrolcount) = 2

                            ' load the controls
                            'developer guide no. Todo list
                            'ContainerHelper.LoadControl(Me, "Item", lcontrolcount)
                            ContainerHelper.LoadControl(objfrmInterface, "txtBox", lcontrolcount, True)
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", llabelcount)
                            ContainerHelper.LoadControl(objfrmInterface, "lbl", llabelcount, True)
                            objfrmInterface.txtBox(lcontrolcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.lbl(llabelcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.txtBox(lcontrolcount).Visible = True
                            objfrmInterface.lbl(llabelcount).Visible = True

                            ' arrange the positions
                            objfrmInterface.txtBox(lcontrolcount).Top = VB6.TwipsToPixelsY(lTop)
                            objfrmInterface.lbl(llabelcount).Top = VB6.TwipsToPixelsY(lTop)

                            ' load the values for the text box

                            objfrmInterface.txtBox(lcontrolcount).Text = CStr(r_vControlsArray(g_cICAvalue, lRow))

                            objfrmInterface.txtBox(lcontrolcount).Tag = CStr(r_vControlsArray(g_ciCAuserdefperildataid, lRow))
                            GeneralText = objfrmInterface.txtBox(lcontrolcount).Text
                            If Information.IsArray(vDataTypeArray) Then

                                ReDim Preserve vDataTypeArray(1, vDataTypeArray.GetUpperBound(1) + 1)
                            Else
                                ReDim vDataTypeArray(1, 1)
                            End If

                            vDataTypeArray(0, vDataTypeArray.GetUpperBound(1)) = "Integer"

                            vDataTypeArray(1, vDataTypeArray.GetUpperBound(1)) = r_vControlsArray(g_ciCAuserdefperildataid, lRow)

                            ' change the caption name

                            objfrmInterface.lbl(llabelcount).Text = CStr(r_vControlsArray(g_ciCAcaption, lRow)) & " :"

                            ' check whether read_only
                            'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                            'DC280302 -start -check for underwriting/broking

                            If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                objfrmInterface.txtBox(lcontrolcount).Enabled = False
                            Else
                                objfrmInterface.txtBox(lcontrolcount).Enabled = True
                            End If
                            If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
                                objfrmInterface.lbl(llabelcount).Font = VB6.FontChangeBold(objfrmInterface.lbl(llabelcount).Font, True)
                                objfrmInterface.lbl(llabelcount).Tag = "txtBox" & lcontrolcount
                            Else
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
                                objfrmInterface.lbl(llabelcount).Tag = CStr(0)
                            End If

                            'DC280302 -end

                        Case 3 'Date
                            lcontrolcount = objfrmInterface.txtBox.Length  ' + 1
                            llabelcount = objfrmInterface.lbl.Length ' + 1
                            ReDim Preserve intControlTypes(lcontrolcount)
                            intControlTypes(lcontrolcount) = 3

                            ' load the controls
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", lcontrolcount)
                            ContainerHelper.LoadControl(objfrmInterface, "txtBox", lcontrolcount, True)
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", llabelcount)
                            ContainerHelper.LoadControl(objfrmInterface, "lbl", llabelcount, True)
                            objfrmInterface.txtBox(lcontrolcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.lbl(llabelcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.txtBox(lcontrolcount).Visible = True
                            objfrmInterface.lbl(llabelcount).Visible = True

                            ' arrange the positions
                            objfrmInterface.txtBox(lcontrolcount).Top = VB6.TwipsToPixelsY(lTop)
                            objfrmInterface.lbl(llabelcount).Top = VB6.TwipsToPixelsY(lTop)

                            ' load the values for the text box

                            If CStr(r_vControlsArray(g_cICAvalue, lRow)) <> "" Then

                                objfrmInterface.txtBox(lcontrolcount).Text = StringsHelper.Format(r_vControlsArray(g_cICAvalue, lRow), ACDateDisplay)
                            Else
                                objfrmInterface.txtBox(lcontrolcount).Text = StringsHelper.Format(DateTime.Today, ACDateDisplay)
                            End If
                            GeneralText = objfrmInterface.txtBox(lcontrolcount).Text


                            objfrmInterface.txtBox(lcontrolcount).Tag = CStr(r_vControlsArray(g_ciCAuserdefperildataid, lRow))
                            If Information.IsArray(vDataTypeArray) Then

                                ReDim Preserve vDataTypeArray(1, vDataTypeArray.GetUpperBound(1) + 1)
                            Else
                                ReDim vDataTypeArray(1, 1)
                            End If
                            vDataTypeArray(0, vDataTypeArray.GetUpperBound(1)) = "Date"
                            vDataTypeArray(1, vDataTypeArray.GetUpperBound(1)) = r_vControlsArray(g_ciCAuserdefperildataid, lRow)

                            ' change the caption name

                            objfrmInterface.lbl(llabelcount).Text = CStr(r_vControlsArray(g_ciCAcaption, lRow)) & " :"

                            ' check whether read_only
                            'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                            'DC280302 -start -check for underwriting/broking


                            If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                objfrmInterface.txtBox(lcontrolcount).Enabled = False
                            Else
                                objfrmInterface.txtBox(lcontrolcount).Enabled = True
                            End If
                            If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
                                objfrmInterface.lbl(llabelcount).Font = VB6.FontChangeBold(objfrmInterface.lbl(llabelcount).Font, True)
                                objfrmInterface.lbl(llabelcount).Tag = "txtBox(Date)" & lcontrolcount
                            Else
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
                                objfrmInterface.lbl(llabelcount).Tag = CStr(0)
                            End If

                            'DC280302 -end

                        Case 4 'Yes/ No
                            lcontrolcount = objfrmInterface.chkBox.Length  ' + 1
                            llabelcount = objfrmInterface.lbl.Length  '+ 1
                            ReDim Preserve intControlTypes(lcontrolcount)
                            intControlTypes(lcontrolcount) = 4

                            ' load the controls
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", lcontrolcount)
                            ContainerHelper.LoadControl(objfrmInterface, "chkBox", lcontrolcount, True)
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", llabelcount)
                            ContainerHelper.LoadControl(objfrmInterface, "lbl", llabelcount, True)
                            objfrmInterface.chkBox(lcontrolcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.lbl(llabelcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.chkBox(lcontrolcount).Visible = True
                            objfrmInterface.lbl(llabelcount).Visible = True

                            ' arrange the positions
                            objfrmInterface.chkBox(lcontrolcount).Top = VB6.TwipsToPixelsY(lTop)
                            objfrmInterface.lbl(llabelcount).Top = VB6.TwipsToPixelsY(lTop)

                            ' load the values for the text box

                            If CStr(r_vControlsArray(g_cICAvalue, lRow)) = "" Then

                                r_vControlsArray(g_cICAvalue, lRow) = 0
                            End If


                            objfrmInterface.chkBox(lcontrolcount).CheckState = r_vControlsArray(g_cICAvalue, lRow)


                            objfrmInterface.chkBox(lcontrolcount).Tag = CStr(r_vControlsArray(g_ciCAuserdefperildataid, lRow))
                            GeneralText = CStr(objfrmInterface.chkBox(lcontrolcount).CheckState)
                            ' change the caption name

                            objfrmInterface.lbl(llabelcount).Text = CStr(r_vControlsArray(g_ciCAcaption, lRow)) & " :"

                            ' check whether read_only
                            'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                            'DC280302 -start -check for underwriting/broking

                            If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                objfrmInterface.chkBox(lcontrolcount).Enabled = False
                            Else
                                objfrmInterface.chkBox(lcontrolcount).Enabled = True
                            End If

                            If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
                                objfrmInterface.lbl(llabelcount).Font = VB6.FontChangeBold(objfrmInterface.lbl(llabelcount).Font, True)
                                objfrmInterface.lbl(llabelcount).Tag = "chkBox" & lcontrolcount
                            Else
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
                                objfrmInterface.lbl(llabelcount).Tag = CStr(0)
                            End If
                            'DC280302 -end

                        Case 5 'Lookup
                            lcontrolcount = objfrmInterface.cmbBox.Length ' + 1
                            llabelcount = objfrmInterface.lbl.Length '+ 1
                            ReDim Preserve intControlTypes(lcontrolcount)
                            intControlTypes(lcontrolcount) = 5

                            ' load the controls
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", lcontrolcount)
                            ContainerHelper.LoadControl(objfrmInterface, "cmbBox", lcontrolcount)
                            'developer guide no. Todo List
                            'ContainerHelper.LoadControl(Me, "Item", llabelcount)
                            ContainerHelper.LoadControl(objfrmInterface, "lbl", llabelcount)
                            objfrmInterface.cmbBox(lcontrolcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.lbl(llabelcount).Parent = objfrmInterface.Picture1(intCurrentTab)
                            objfrmInterface.cmbBox(lcontrolcount).Visible = True
                            objfrmInterface.lbl(llabelcount).Visible = True

                            ' arrange the positions
                            objfrmInterface.cmbBox(lcontrolcount).Top = VB6.TwipsToPixelsY(lTop)
                            objfrmInterface.lbl(llabelcount).Top = VB6.TwipsToPixelsY(lTop)

                            ' load the values for the text box


                            r_vLookupArray = Nothing

                            m_lReturn = g_oBusiness.GetClaimLookup(r_vControlsArray(g_cICAclaimlookupid, lRow), r_vLookupArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            If Not Information.IsArray(r_vLookupArray) Or Object.Equals(r_vLookupArray, Nothing) Then
                                ' do nothing
                            Else

                                For iCount As Integer = r_vLookupArray.GetLowerBound(1) To r_vLookupArray.GetUpperBound(1)

                                    'developer guide no.153
                                    'objfrmInterface.cmbBox(lcontrolcount).Items.Add(CStr(r_vLookupArray(g_cICLAdescription, iCount)))



                                    'VB6.SetItemData(objfrmInterface.cmbBox(lcontrolcount), "NewIndex", CInt(r_vLookupArray(g_cICLAlookupid, iCount)))
                                    Dim newIndex As Integer = objfrmInterface.cmbBox(lcontrolcount).Items.Add(New VB6.ListBoxItem(CStr(r_vLookupArray(g_cICLAdescription, iCount)), CInt(r_vLookupArray(g_cICLAlookupid, iCount))))
                                Next iCount
                            End If

                            For iCount As Integer = 0 To objfrmInterface.cmbBox(lcontrolcount).Items.Count - 1

                                If VB6.GetItemString(objfrmInterface.cmbBox(lcontrolcount), iCount) = CStr(r_vControlsArray(g_cICAvalue, lRow)) Then
                                    objfrmInterface.cmbBox(lcontrolcount).SelectedIndex = iCount
                                    Exit For
                                Else
                                    objfrmInterface.cmbBox(lcontrolcount).SelectedIndex = -1
                                End If
                            Next iCount



                            objfrmInterface.cmbBox(lcontrolcount).Tag = CStr(r_vControlsArray(g_ciCAuserdefperildataid, lRow))

                            ' change the caption name

                            objfrmInterface.lbl(llabelcount).Text = CStr(r_vControlsArray(g_ciCAcaption, lRow)) & " :"

                            ' check whether read_only
                            'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                            'DC280302 -start -check for underwriting/broking

                            If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                objfrmInterface.cmbBox(lcontrolcount).Enabled = False
                            Else
                                objfrmInterface.cmbBox(lcontrolcount).Enabled = True
                            End If

                            If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
                                objfrmInterface.lbl(llabelcount).Font = VB6.FontChangeBold(objfrmInterface.lbl(llabelcount).Font, True)
                                objfrmInterface.lbl(llabelcount).Tag = "cmbBox" & lcontrolcount
                            Else
                                m_lmandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
                                objfrmInterface.lbl(llabelcount).Tag = CStr(0)
                            End If


                            'DC280302 -end
                            '
                        Case 6 'Party



                            Select Case r_vControlsArray(g_ciCAcaption, lRow)
                                Case "Driver"
                                    SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACDriver, True)
                                    ' Read only
                                    'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then


                                    If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                        objfrmInterface.uctDriver.Task = gPMConstants.PMEComponentAction.PMView
                                    Else
                                        objfrmInterface.uctDriver.Task = gPMConstants.PMEComponentAction.PMEdit
                                    End If

                                    'Mandatory


                                    If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                        objfrmInterface.uctDriver.Tag = CStr(1)
                                    Else
                                        objfrmInterface.uctDriver.Tag = CStr(0)
                                    End If


                                    'DC280302 -end

                                    objfrmInterface.uctDriver.ClaimId = m_lClaimID
                                    objfrmInterface.uctDriver.PartyTypeCode = "OTDRIVER"

                                    m_lReturn = objfrmInterface.uctDriver.Initialise()

                                    m_lReturn = objfrmInterface.uctDriver.LoadControl()

                                    m_lReturn = objfrmInterface.uctDriver.GetParties()

                                Case "Third Party"

                                    SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACThirdParty, True)

                                    ' read only
                                    'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                                    'DC280302 -start -check for underwriting/broking

                                    If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                        objfrmInterface.uctThirdParty.Task = gPMConstants.PMEComponentAction.PMView
                                    Else
                                        objfrmInterface.uctThirdParty.Task = gPMConstants.PMEComponentAction.PMEdit
                                    End If

                                    ' mandatory


                                    If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                        objfrmInterface.uctThirdParty.Tag = CStr(1)
                                    Else
                                        objfrmInterface.uctThirdParty.Tag = CStr(0)
                                    End If

                                    objfrmInterface.uctThirdParty.ClaimId = m_lClaimID
                                    objfrmInterface.uctThirdParty.PartyTypeCode = "OTTHIRD"

                                    m_lReturn = objfrmInterface.uctThirdParty.Initialise()

                                    m_lReturn = objfrmInterface.uctThirdParty.LoadControl()

                                    m_lReturn = objfrmInterface.uctThirdParty.GetParties()

                                Case "Witness"

                                    SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACWitness, True)
                                    ' read only
                                    'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                                    'DC280302 -start -check for underwriting/agency

                                    If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                        objfrmInterface.uctWitness.Task = gPMConstants.PMEComponentAction.PMView
                                    Else
                                        objfrmInterface.uctWitness.Task = gPMConstants.PMEComponentAction.PMEdit
                                    End If

                                    ' mandatory


                                    If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                        objfrmInterface.uctWitness.Tag = CStr(1)
                                    Else
                                        objfrmInterface.uctWitness.Tag = CStr(0)
                                    End If

                                    'DC280302 -end

                                    objfrmInterface.uctWitness.ClaimId = m_lClaimID
                                    objfrmInterface.uctWitness.PartyTypeCode = "OTWITNESS"

                                    m_lReturn = objfrmInterface.uctWitness.Initialise()

                                    m_lReturn = objfrmInterface.uctWitness.LoadControl()

                                    m_lReturn = objfrmInterface.uctWitness.GetParties()

                                Case "Repairer"
                                    SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACRepairer, True)
                                    ' read only
                                    'TN20010409     If r_vControlsArray(g_cICAreadonly, lRow) = "True" Or m_iTask = PMView Or m_iTask = 3 Then

                                    'DC280302 -start -check for underwriting/broking

                                    If (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType <> "C_CO") Or m_iTask = gPMConstants.PMEComponentAction.PMView Or objfrmInterface.TransactionType = "C_CP" Or objfrmInterface.DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then

                                        objfrmInterface.uctRepairer.Task = gPMConstants.PMEComponentAction.PMView
                                    Else
                                        objfrmInterface.uctRepairer.Task = gPMConstants.PMEComponentAction.PMEdit
                                    End If

                                    ' mandatory


                                    If CStr(r_vControlsArray(g_cICAmandatory, lRow)) = "True" Or (CStr(r_vControlsArray(g_cICAreadonly, lRow)) = "True" And objfrmInterface.TransactionType = "C_CO") Then

                                        objfrmInterface.uctRepairer.Tag = CStr(1)
                                    Else
                                        objfrmInterface.uctRepairer.Tag = CStr(0)
                                    End If


                                    'DC280302 -end

                                    objfrmInterface.uctRepairer.ClaimId = m_lClaimID
                                    objfrmInterface.uctRepairer.PartyTypeCode = "OTREPAIRER"

                                    m_lReturn = objfrmInterface.uctRepairer.Initialise()

                                    m_lReturn = objfrmInterface.uctRepairer.LoadControl()

                                    m_lReturn = objfrmInterface.uctRepairer.GetParties()

                                Case Else
                                    ' do nothing
                            End Select

                            'TN20010409


                            'DC140302
                        Case 7 'General Tab Name
                            'We use the tab caption from the tabs table now and put this thing in the form caption instead
                            'DC030703 -ISS4415 -set title to that defined in user defined details

                            SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, ACGeneral, " - " & CStr(r_vControlsArray(g_cICDescription, lRow)))
                        Case Else
                            ' do nothing
                    End Select

                    lngLastTabID = lngCurrentTabID

                    'Set the correct picture height for how many controls in the current tab
                    objfrmInterface.Picture1(intCurrentTab).Height = VB6.TwipsToPixelsY(lTop + 315)

                    objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum = (100 + objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange - 1)
                    objfrmInterface.VScroll_Renamed(intCurrentTab).Minimum = 0
                    objfrmInterface.VScroll_Renamed(intCurrentTab).SmallChange = 1
                    NewLargeChange = (objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum - (objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange + 1))
                    objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum = objfrmInterface.VScroll_Renamed(intCurrentTab).Maximum + NewLargeChange - objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange
                    objfrmInterface.VScroll_Renamed(intCurrentTab).LargeChange = NewLargeChange

                    objfrmInterface.VScroll_Renamed(intCurrentTab).Visible = Not (VB6.PixelsToTwipsY(objfrmInterface.Picture1(intCurrentTab).Height) < VB6.PixelsToTwipsY(objfrmInterface.Picture2(intCurrentTab).Height))

                Next lRow


            End If

            'Get rid of the unused tabs
            For i As Integer = intCurrentTab + 1 To 4
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, i, False)
            Next


            ' visible/ invisible the Party Tab
            lcontrolcount = 0

            For Each ctrl As Control In ContainerHelper.Controls(objfrmInterface)
                If (ctrl.Name = "chkBox") Or (TypeOf ctrl Is TextBox) Or (TypeOf ctrl Is ComboBox) Then
                    If ctrl.Name <> "txtComments" Then
                        lcontrolcount += 1
                    End If
                End If
            Next ctrl
            If lcontrolcount = 3 Then
                SSTabHelper.SetTabVisible(objfrmInterface.SSTab1, ACGeneral, False)
            End If
            GetTabCaption()


            ' RDC 03062004
            If m_lClaimID <> 0 Then

                m_lReturn = g_oBusiness.GetClaimCurrency(v_lClaimID:=m_lClaimID, r_lCurrencyID:=m_lCurrencyID, r_sCurrencyDesc:=m_sCurrencyDesc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the claim currency", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            Return result

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' Change made: Changes have been made in this functions so that payment
    '              payment can be made even when the screen is called in the
    '              payment mode of Claim
    '
    ' BugID  : 23
    '
    ' Author :     Ranjit R
    ' ********************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        'Tracy Richards - 29/06/2003 - VAT on Claims
        Dim sTaxTypeCode As String = ""
        Dim sOption As String = ""
        Dim sInsuranceRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If objfrmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                'not interested
                Return result
            End If

            If objfrmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then

                'developer guide no.243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'developer guide no.243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    'nope, we are not cancelling
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If
            'TN20010409 End

            If SSTabHelper.GetTabVisible(objfrmInterface.SSTab1, ACGeneral) Then

                ' Update the details using the business object.
                m_lReturn = objfrmInterface.UpdateGeneral()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                End If
            End If

            ' save reserve and payment details
            m_lReturn = objfrmInterface.SaveReserveAndPaymentDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SaveReserveAndPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                End If


                ' Insert the Comments in the Peril Table
                m_lReturn = objfrmInterface.AddComments()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                End If
            Else
                result = m_lReturn
            End If
            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    'Private Function CreateAuthorisationTask() As Integer
    'Dim result As Integer = 0
    'Dim vKeyArray As Object
    'Dim lPMWrkTaskInstanceCnt As Integer
    'Dim sTaskGroupCode As String = ""
    '
    'Const c_sTaskCode As String = "AUTHCLMCHQ"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Create instances of the objects we need to use.
    'm_lReturn = CreateWrkTaskInstances()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateWrkTaskInstance failed.")
    'End If
    '

    'm_lReturn = g_oBusiness.GetTaskGroupCode(v_sTaskCode:=c_sTaskCode, r_sTaskGroupCode:=sTaskGroupCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetTaskGroupCode failed.")
    'End If
    '

    'm_lReturn = g_oBusiness.GetPartyName(v_lPartyCnt:=m_lPartycnt, v_sFieldName:="shortname", r_sResult:=m_sPartyShortName)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetPartyName failed.")
    'End If
    '

    'm_lReturn = g_oBusiness.GetClaimNumberFromClaim(v_lClaimID:=m_lClaimID, r_sClaimRef:=m_sClaimRef)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetClaimNumberFromClaim failed.")
    'End If
    '
    'Resize the key array.
    ''ReDim vKeyArray(1, 13)
    '
    'Populate the key array with all of the keys required to create a work task.

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sTaskGroupCode

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskCode

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = c_sTaskCode

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskDescription

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "CLAIM: " & m_sClaimRef.Trim() & " - Payment authorisation requested"

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskCustomer

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sPartyShortName

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameUseExtraKeys

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = True
    '
    'Populate the key array with all of the extra keys required to run the authorise navigator.

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameCurrentNashStep

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = 1

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClaimID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lClaimID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameClaimPerilID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lPerilID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameClaimPaymentSequence

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_lSequenceNo

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNamePartyCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_lPartycnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameRiskTypeID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lRiskID

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_lInsurance_file_cnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = "insurancefile_cnt"

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_lInsurance_file_cnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameClaimReference

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_sClaimRef
    '
    'Pass the keys into the object.

    'm_lReturn = m_oWrkTaskInstanceTemp.SetKeys(vKeyArray:=vKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oWrkTaskInstance.SetKeys failed.")
    'End If
    '

    'm_lReturn = m_oWrkTaskInstanceTemp.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
    '

    'm_oWrkTaskInstanceTemp.CallingAppName = "Authorise Claim Payment Task"
    '
    'Start the object.

    'm_lReturn = m_oWrkTaskInstanceTemp.Start
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oWrkTaskInstance.Start failed.")
    'End If
    '
    'Destroy the instance of the object.
    'm_lReturn = DestroyWrkTaskInstances()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateWrkTaskInstance failed.")
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAuthorisationTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAuthorisationTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    Private Function CreateWrkTaskInstances() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oWrkTaskInstanceTemp Is Nothing Then

            Dim temp_m_oWrkTaskInstanceTemp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oWrkTaskInstanceTemp, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oWrkTaskInstanceTemp = temp_m_oWrkTaskInstanceTemp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of iPMWrkTaskInstanceTemp.Interface")
            End If

        End If

        Return result

    End Function

    Private Function DestroyWrkTaskInstances() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oWrkTaskInstanceTemp Is Nothing) Then

            m_oWrkTaskInstanceTemp.Dispose()
            m_oWrkTaskInstanceTemp = Nothing
        End If

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.

        For Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
            ' Check the type of the control.
            If TypeOf ctlFormControl Is TextBox Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            End If
        Next ctlFormControl

        Return result

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
        ' Error Section.
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


    Private Sub GetTabCaption()
        Dim icaption As Integer


        icaption = 1
        For lCount As Integer = 0 To SSTabHelper.GetTabCount(objfrmInterface.SSTab1) - 1
            If SSTabHelper.GetTabVisible(objfrmInterface.SSTab1, lCount) Then
                SSTabHelper.SetTabCaption(objfrmInterface.SSTab1, lCount, icaption & SSTabHelper.GetTabCaption(objfrmInterface.SSTab1, lCount))
                icaption += 1
            End If
        Next lCount

    End Sub

    '*************************************************************************************
    ' Name : PostPaymentToOrion
    '
    ' Desc : post payment transactions to orion
    '
    ' Hist : 15/03/2001 Created - Tinny
    '        05/07/01   RWH - Revised production of stats and removed stuff geared to
    '                   production of transactions as these will now be done in stored
    '                   procedures at the end of the roadmap.
    '*************************************************************************************

    'Private Function PostPaymentToOrion(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimID As Integer, ByVal v_lPerilID As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_cTaxAmount As Decimal = 0, Optional ByVal v_sTaxTypeCode As String = "") As Integer
    '
    'Dim result As Integer = 0
    'Dim lDebitAccountID, lCreditAccountID As Integer
    'Dim sDebitAccountCode As String = ""
    'Dim lLedgerId As Integer
    'Dim sLedgerShortName, sCreditTransLedgerCode, sCreditAccountLedgerCode As String
    'Dim lStatsFolderCnt As Integer
    'Dim sCreditAccountCode As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'A payment will debit the reserve account.
    'sDebitAccountCode = "CLMRES" & v_sCOBCode.Trim()
    '
    'get credit account id - use party count if we have it
    'If v_lPartyCnt <> 0 Then

    'result = g_oClaimTrans.GetAccountID(r_lAccountID:=lCreditAccountID, v_lPartyCnt:=v_lPartyCnt)
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    'If result <> gPMConstants.PMEReturnCode.PMNotFound Then
    'Return result
    'End If
    'End If
    'End If
    '
    'data which goes in stats folder/detail and transaction detail

    'g_oClaimTrans.DebitAccountID = lDebitAccountID

    'g_oClaimTrans.CreditAccountID = lCreditAccountID

    'g_oClaimTrans.TransactionTypeID = 27

    'g_oClaimTrans.TransactionTypeCode = "C_CP" 'claim payment

    'g_oClaimTrans.DocumentTypeID = 28 'Claim Payment

    'g_oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

    'g_oClaimTrans.ClaimID = v_lClaimID

    'g_oClaimTrans.PerilID = v_lPerilID

    'g_oClaimTrans.DebitCredit = "C"

    'g_oClaimTrans.DocumentComment = "Payment for claim number " & v_lClaimID

    'g_oClaimTrans.TransactionAmount = v_cPayAmount
    '
    'RWH(02/07/01) Need to create stats separately now for each record to
    'account for reins and coins.

    'm_lReturn = g_oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:="C_CP")
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'RWH(02/07/01) Create stats_detail for main payment.

    'm_lReturn = g_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Tracy Richards - 29/06/2003 - VAT on Claims
    ' Alix - 21/05/2003 - Insert stats details records for VAT (one NET  and one GROSS)
    ' If we have tax on this payment post it, but only post the GROSS portion, the NET, TREATY
    ' or FAC portions cannot be posted until reinsurance has been evaluated.
    'If (v_cTaxAmount <> 0) And g_bIsPostTaxes Then
    '
    ' Pass tax amount

    'g_oClaimTrans.TransactionAmount = v_cTaxAmount
    '
    ' set tan / tag account code
    'sCreditAccountCode = "NOTA" & v_sTaxTypeCode.Trim() & "IN"
    '
    ' Create stats for TAG amount

    'm_lReturn = g_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse

    'm_lReturn = g_oClaimTrans.Terminate()
    'g_oClaimTrans = Nothing
    'Return result
    'End If
    '
    ' None-gross tax postings need to take into account reinsurance so do it later
    '        ' Pass negated tax amount
    '        g_oClaimTrans.TransactionAmount = -v_cTaxAmount
    ''
    '        ' Create stats for NET amount
    '        m_lReturn = g_oClaimTrans.CreateStatsDetails( _
    ''            v_lStatsFolderCnt:=lStatsFolderCnt, _
    ''            v_sStatsDetailType:="TAN", v_lClassOfBusId:=v_lCOBId, _
    ''            v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, _
    ''            v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, _
    ''            v_sglRISharePercent:=0)
    '        If (m_lReturn <> PMTrue) Then
    '            PostPaymentToOrion = PMFalse
    '            m_lReturn = g_oClaimTrans.Terminate()
    '            Set g_oClaimTrans = Nothing
    '            Exit Function
    '        End If
    'End If
    ' /Alix
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostPaymentToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post payment transactions to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostPaymentToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    '*************************************************************************************
    ' Name : PostReserveToOrion
    '
    ' Desc : post reserve transactions to orion
    '
    ' Hist : 15/03/2001 Created - Tinny
    '        05/07/01   RWH - Revised production of stats and removed stuff geared to
    '                   production of transactions as these will now be done in stored
    '                   procedures at the end of the roadmap.
    '*************************************************************************************

    'Private Function PostReserveToOrion(ByVal v_vReserveArray( ,  ) As Object, ByVal v_oListView As ListView, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimID As Integer, ByVal v_lPerilID As Integer, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim cTotalReserveTrans, cCurrentValue, cNewValue As Decimal
    'Dim lTransactionTypeID As Integer
    'Dim sTransactionTypeCode As String = ""
    'Dim lDebitAccountID, lCreditAccountID As Integer
    'Dim sDebitAccountCode, sCreditAccountCode As String
    'Dim lStatsFolderCnt As Integer
    '
    'Const ACInitialReserve As Integer = 1
    'Const ACThisRevision As Integer = 3
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'view mode, who cares
    'If m_iTask = gPMConstants.PMEComponentAction.PMView Then
    'Return result
    'End If
    '
    'initialising debit/credit account
    'sDebitAccountCode = "CLMEXP" & v_sCOBCode.Trim()
    'sCreditAccountCode = "CLMRES" & v_sCOBCode.Trim()
    '
    'cTotalReserveTrans = 0
    'cNewValue = 0
    'cCurrentValue = 0
    '
    'check to see if there are any changes in the reserves
    'If v_oListView.Items.Count <> 0 Then
    '
    'Select Case frmInterface.TransactionType
    'Case "C_CO" 'from Open claim (initial reserve can be changed)
    'lTransactionTypeID = 26
    'sTransactionTypeCode = "C_CO" 'claim open
    '
    'total up new initial reserve
    'For 'lCount As Integer = 3 To v_oListView.Items.Count - 2
    'cNewValue += CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lCount - 1), ACInitialReserve).Text.Trim())
    'Next 
    '
    'total up old inital reserve
    'If Information.IsArray(v_vReserveArray) Then
    'For 'lCount As Integer = 0 To v_vReserveArray.GetUpperBound(1)

    'cCurrentValue += CDbl(v_vReserveArray(g_cIRDAinitialreserve, lCount))
    'Next 
    'End If
    '
    'cTotalReserveTrans = cNewValue - cCurrentValue
    '
    'Case "C_CR", "C_CP"
    'C_CP - reserves might have been adjusted because payment is greater than current reserve
    'C_CR - reserves might have been revised
    'lTransactionTypeID = 28
    'sTransactionTypeCode = "C_CR"
    '
    'loop thro and total this revision column
    'For 'lCount As Integer = 3 To v_oListView.Items.Count - 2
    'cTotalReserveTrans += CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lCount - 1), ACThisRevision).Text.Trim())
    'Next 
    '
    'End Select
    '
    'End If
    '
    'MSS011001 - Added switch for merge. Seemed 2 completely different ways of doing it
    '
    'post to Orion only when reserves has been added/changed
    'If cTotalReserveTrans <> 0 Then
    '
    'data which goes in stats folder/detail and transaction detail

    'g_oClaimTrans.DebitAccountID = lDebitAccountID 'claim expense

    'g_oClaimTrans.CreditAccountID = lCreditAccountID 'claim reserve

    'g_oClaimTrans.TransactionTypeID = lTransactionTypeID

    'g_oClaimTrans.TransactionTypeCode = sTransactionTypeCode

    'g_oClaimTrans.DocumentTypeID = 35 'Transferred Debit

    'g_oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

    'g_oClaimTrans.ClaimID = v_lClaimID

    'g_oClaimTrans.PerilID = v_lPerilID

    'g_oClaimTrans.DebitCredit = "D"

    'g_oClaimTrans.DocumentComment = "Reserve for claim number " & v_lClaimID

    'g_oClaimTrans.TransactionAmount = cTotalReserveTrans
    '
    'RWH(02/07/01) Need to create stats separately now for each record to
    'account for reins and coins.

    'm_lReturn = g_oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=sTransactionTypeCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'RWH(02/07/01) Create stats_detail for main payment.

    'm_lReturn = g_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=lCreditAccountID, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostReserveToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'End If
    '
    'End If
    '
    '
    'MSS011001 - Merge end
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post reserve transactions to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostReserveToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetMappingCodes
    '
    ' Description:
    '
    ' History: 28/06/2001 RWH - Created.
    '
    ' ***************************************************************** '

    'Private Function GetMappingCodes(ByRef v_sLedgerShortName As String, ByRef r_sTransLedgerCode As String, ByRef r_sLedgerMappingCode As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Select Case v_sLedgerShortName.Trim()
    'Case "NO"
    'r_sTransLedgerCode = "NO"
    'r_sLedgerMappingCode = "CLAIMPAY"
    '
    'Case "SA"
    'r_sTransLedgerCode = "SL"
    'r_sLedgerMappingCode = "SALESLEDGR"
    '
    'Case "PU", "RF", "FE", "DI", "CO"
    '
    'Case "IN"
    'r_sTransLedgerCode = "IN"
    'r_sLedgerMappingCode 'REINSACC or COINSACC
    '
    'Case "AG"
    'r_sTransLedgerCode = "AG"
    'r_sLedgerMappingCode = "AGENTLEDGR"
    '
    'Case "UB"
    '
    '
    'End Select
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMappingCodes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMappingCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
End Class

