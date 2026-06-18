Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 02/07/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBPartyOT"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACInterfaceCaption As Integer = 100
    Public Const ACMainTabTitle0 As Integer = 101
    Public Const ACMainTabTitle1 As Integer = 102
    Public Const ACConPostCodeCaption As Integer = 103
    Public Const ACConReferenceCaption As Integer = 104
    Public Const AClbAdReferenceCaption As Integer = 105
    Public Const ACAdPostcodeCaption As Integer = 106

    ' Form Constants for Address ListView
    Public Const ACAddressListUsage As Integer = 108
    Public Const ACAddressListLine1 As Integer = 109
    Public Const ACAddressListLine2 As Integer = 110
    Public Const ACAddressListLine3 As Integer = 111
    Public Const ACAddressListLine4 As Integer = 112
    Public Const ACAddressListPostCode As Integer = 113
    Public Const ACInsurerTabCaption As Integer = 114

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACEditConCaption As Integer = 204
    Public Const ACDeleteConCaption As Integer = 205
    Public Const ACAddConCaption As Integer = 206
    Public Const ACPartyCodeCaption As Integer = 207
    Public Const ACPartyNameCaption As Integer = 208
    Public Const ACDateOfBirthCaption As Integer = 209
    Public Const ACGenderCaption As Integer = 210

    ' Messages

    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACMandatoryFieldsTitle As Integer = 304

    'SD 17/09/2002 Add Supplier party type changes
    Public Const ACInformationLossWarning As Integer = 305
    Public Const ACInformationLossTitle As Integer = 306
    Public Const ACActive As Integer = 307
    Public Const ACAfterHours As Integer = 308
    Public Const ACPriority As Integer = 309
    ' Menus



    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const ACIADDRESS As String = "ADDRESS"

    Public Const ConvictionImage As String = "CONVICTIONIMAGE"
    Public Const AccidentImage As String = "ACCIDENTIMAGE"


    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 4061

    'RWH(09/06/2000)
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object

    'SD 17/09/2002 START
    Public Const ACConstantBlank As String = ""
    Public Const ACConstantYes As String = "Yes"
    Public Const ACConstantNo As String = "No"
    Public Const ACComboPriorityHigh As String = "1"
    Public Const ACComboPriorityMed As String = "2"
    Public Const ACComboPriorityLow As String = "3"

    Public Const ACPartyTypeSupplier As String = "OTSUPPLIER"
    'S4B Claims Enhancements R&D 2005
    Public Const ACPartyTypeThirdParty As String = "OTTHIRD"

    Public Const ACActiveIndicator As Integer = 0
    Public Const ACAfterHoursIndicator As Integer = 1
    Public Const ACPriorityIndicator As Integer = 2
    Public Const ACTPASettleDirectly As Integer = 3
    'SD 17/09/2002 END

    '****************************************
    ' Party Detail Array Position Constants
    Public Const kPartyDetailTaxNumber As Integer = 0
    Public Const kPartyDetailDomiciledForTax As Integer = 1
    Public Const kPartyDetailTaxExempt As Integer = 2
    Public Const kPartyDetailTaxPercentage As Integer = 3
    '****************************************



    Sub Main_Renamed()

        Dim s As String = ""
        'Developer Guide No.108
        Dim o As New Interface_Renamed

        Dim l As DialogResult = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        o.CallingAppName = "TEST"
        'o.Reference = "Salvo"
        'o.PostCode = "BR3 3P"

        l = MessageBox.Show("Yes to Add, No to Edit", Application.ProductName, MessageBoxButtons.YesNo)

        If l = System.Windows.Forms.DialogResult.Yes Then
            ' o.AddressCnt = 22
            l = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            l = o.Start()

        Else
            s = Interaction.InputBox("enter address_cnt")
            o.AddressCnt = CInt(s)
            l = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            l = o.Start()
        End If




		o.Dispose()


    End Sub

End Module