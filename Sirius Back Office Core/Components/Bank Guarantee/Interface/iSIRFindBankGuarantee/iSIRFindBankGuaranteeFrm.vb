Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History: Gaurav
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    Private m_bIsInitialised As Boolean
    'Constants for Defining Width of Columns in List View

    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRFindBankGuarantee.General

    Private Const ColWidthBroking As Integer = 1700
    Private Const ColWidthUnderWriting As Integer = 1300
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As Integer

    ' Details of the Selected Bank
    Private m_vBankNameId As String = ""
    Private m_sShortCode As String = ""
    Private m_sAccountName As String = ""

    Private m_sPartyType As String = ""

    'developer guide no. 33
    Private m_vPartyCnt As Object
    Private m_vPartyCode As String = ""
    Private m_vPartyResolvedName As Object

    'developer guide no. 33
    Private m_vAgentCnt As Object
    Private m_vAgentCode As String = ""
    Private m_vAgentResolvedName As Object


    Private m_vInsuranceFileRef As String = ""
    Private m_lInsuranceFileCnt As Integer

    Private m_vBGStatusCode As Object
    Private m_vbankGuaranteeRef As String = ""

    Private m_vBGStatusId As Integer


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oParty As Object

    Private m_vResultArray(,) As Object
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    Private m_vSourceArray As Object ' MKW 190503 PN2032 START
    Private m_IIsComplaint As Integer
    Private m_lListSelectedItem As Integer

    Public ReadOnly Property ResultArray() As Object
        Get
            Return VB6.CopyArray(m_vResultArray)
        End Get
    End Property

    Public WriteOnly Property IsComplaint() As Integer
        Set(ByVal Value As Integer)

            m_IIsComplaint = Value

        End Set
    End Property

    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user


            m_vSourceArray = Value

        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property


    'DC180202
    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property


    Public Property InsuranceFileRef() As String
        Get

            'Return Insurance File Id
            Return m_vInsuranceFileRef

        End Get
        Set(ByVal Value As String)

            'Set Insurance File id

            m_vInsuranceFileRef = CStr(Value)

        End Set
    End Property


    Public Property PartyCode() As String
        Get

            'Return Client Name
            Return m_vPartyCode

        End Get
        Set(ByVal Value As String)

            'Set Client Name

            m_vPartyCode = CStr(Value)

        End Set
    End Property

    'Public ReadOnly Property PartyCnt() As Byte
    Public ReadOnly Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
    End Property

    'Public ReadOnly Property AgentCnt() As Byte
    Public ReadOnly Property AgentCnt() As Object
        Get
            Return m_vAgentCnt
        End Get
    End Property

    Public ReadOnly Property BankNameId() As String
        Get
            Return m_vBankNameId
        End Get
    End Property

    Public ReadOnly Property BankGuarenteeRef() As String
        Get
            Return m_vbankGuaranteeRef
        End Get
    End Property


    Public Property ListSelectedItem() As Integer
        Get
            Return m_lListSelectedItem
        End Get
        Set(ByVal Value As Integer)
            m_lListSelectedItem = Value
        End Set
    End Property

    '***************************************************************** '
    ' Name: SetupBankDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : Date :
    '***************************************************************** '
    Private Function SetupBankDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupBankDetailsListView"

        Dim lColWidth As Integer
        Dim sCaption As String = ""


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lColWidth = CInt((VB6.PixelsToTwipsX(lvwsearchdetails.Width) - 100) / 6)

            lvwsearchdetails.Columns.Clear()


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBankName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexBankName, "", sCaption, CInt(VB6.TwipsToPixelsX(2129)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBGNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexBGNo, "", sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBGLimit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexBGLimit, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAvailableBal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexAvailableBal, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwExpDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexExpdate, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexClientCode, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwClientName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexClientName, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)



            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexproduct, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwsearchdetails.Columns.Insert(kBankGuaranteeColHIndexBranch, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)



            lvwsearchdetails.LabelEdit = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function SearchResults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SearchResults"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = g_oBusiness.GetBankGuaranteeDetails(vPartyCode:=m_vPartyCode, vAgentCode:=m_vAgentCode, vBankGuaranteeRef:=m_vbankGuaranteeRef, vInsuranceFileRef:=m_vInsuranceFileRef, vBankName:=m_vBankNameId, vBGStatusId:=m_vBGStatusId, r_vResultArray:=m_vResultArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "g_oBusiness.GetBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                cmdEdit.Enabled = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' Date:11/07/00
    '
    ' Edit History:Gaurav

    '
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_lReturn = GetLookUps()



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateBankDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    Private Function PopulateBankDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBankDetails"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oListItem As ListViewItem



            'Set max rows to number of addresses - though must be at least 5
            lvwsearchdetails.Items.Clear()

            If gPMFunctions.IsArrayEmpty(m_vResultArray) Then
                Return result
            End If

            For i As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)
                If m_vResultArray(MainModule.ENBankGuarantee.RowStatus, i) <> gPMConstants.PMEReturnCode.PMNotFound Then


                    'developer guide no.49
                    oListItem = lvwsearchdetails.Items.Add(CStr(m_vResultArray(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Description)).Trim(), "list")
                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGNo).Text = CStr(m_vResultArray(MainModule.ENBankGuarantee.BGRef, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGLimit).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vResultArray(MainModule.ENBankGuarantee.BGLimit, i)).Trim())))

                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexAvailableBal).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vResultArray(MainModule.ENBankGuarantee.AvailableBal, i)).Trim())))

                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexExpdate).Text = CStr(m_vResultArray(MainModule.ENBankGuarantee.ExpiryDate, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexClientCode).Text = CStr(m_vResultArray(MainModule.ENBankGuarantee.ShortName, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexClientName).Text = CStr(m_vResultArray(MainModule.ENBankGuarantee.ResolvedName, i)).Trim()

                    'Start - Sankar - Bank Guarantee Bug Fixing

                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexproduct).Text = ToSafeString(m_vResultArray(MainModule.ENBankGuarantee.Products, 0))

                    ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBranch).Text = ToSafeString(m_vResultArray(MainModule.ENBankGuarantee.Branches, 0)).Trim()

                    'End - Sankar - Bank Guarantee Bug Fixing

                    oListItem.Tag = CStr(m_vResultArray(MainModule.ENBankGuarantee.RowIndex, i))
                End If

            Next i




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Date:11/07/00
    '
    ' Edit History:Gaurav
    '
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = PopulateBankDetailsList()
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    ' Date:15/07/00
    '
    ' Edit History:Gaurav
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToProperties"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vbankGuaranteeRef = txtBGRef.Text
            m_vPartyCode = txtBGHolder.Text
            m_vAgentCode = txtAgent.Text
            m_vInsuranceFileRef = txtPolicy.Text
            m_vBankNameId = txtBank.Text
            m_vBGStatusId = cboBGStatus.ItemId
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyInfo
    '
    ' Description:  Instance FindInsurance to retrieve Policy reference
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Public Function GetPolicyInfo() As Integer
        Dim result As Integer = 0
        Dim iPMBFindInsurance As Object
        Const kMethodName As String = "GetPolicyInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed


            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iSIRFindInsurance.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp


            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iSIRFindInsurance.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindPolicy.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Retrieve InsuranceRef and set as PolicyRef

                m_vInsuranceFileRef = oFindPolicy.InsReference

                txtBGHolder.Text = oFindPolicy.ShortName

                m_vPartyCnt = oFindPolicy.InsHolderCnt
            ElseIf oFindPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
            'm_vInsuranceFilecnt = oFindPolicy.InsFileCnt
            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing

            ' Display Policy Reference on form
            txtPolicy.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_vInsuranceFileRef.Trim())


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetBGHolderInfo(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer
        Dim result As Integer = 0
        Dim oFindParty As Object
        Const kMethodName As String = "ClearInterface"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdEdit.Enabled = False
            cmdNew.Enabled = False

            lvwsearchdetails.Items.Clear()
            txtBGHolder.Text = ""
            txtAgent.Text = ""
            txtPolicy.Text = ""
            txtBGRef.Text = ""
            m_vAgentCnt = 0
            m_vPartyCnt = 0
            m_vbankGuaranteeRef = ""
            m_vBankNameId = ""
            m_vInsuranceFileRef = ""
            m_vPartyCode = ""
            m_vAgentCode = ""
            m_vBGStatusId = 0
            'm_vInsuranceFilecnt = 0
            m_vResultArray = Nothing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBGHolderInfo
    '
    ' Description: Instance FindParty to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Public Function GetBGHolderInfo() As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Const kMethodName As String = "GetBGHolderInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IsComplaint = m_IIsComplaint

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = True

            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details

                m_vPartyCode = oFindParty.ShortName


                m_vPartyResolvedName = oFindParty.LongName

                m_vPartyCnt = oFindParty.PartyCnt

                ' Destroy Find Party object

                oFindParty.Dispose()
                oFindParty = Nothing

                ' Display Agent on form
                txtBGHolder.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_vPartyCode.Trim())

                txtAgent.Text = ""
            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBGHolderInfo
    '
    ' Description: Instance FindParty to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Public Function GetAgentInfo() As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Const kMethodName As String = "GetAgentInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oFindParty.SpecialParty = "AG"
            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IsComplaint = m_IIsComplaint

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = True


            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details

                m_vAgentCode = oFindParty.ShortName

                m_vAgentCnt = oFindParty.PartyCnt


                m_vAgentResolvedName = oFindParty.LongName

                ' Destroy Find Party object

                oFindParty.Dispose()
                oFindParty = Nothing

                ' Display Agent on form
                txtAgent.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_vAgentCode.Trim())

                txtBGHolder.Text = ""
            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PropertiesToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PropertiesToInterface() As Integer
    '
    'End Function

    '***************************************************************** '
    ' Name: SetupListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : Date :
    '***************************************************************** '
    Private Function SetupListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupListView"

        Dim lColWidth As Integer
        Dim sCaption As String = ""


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(SetupBankDetailsListView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' Date :15/07/2000
    '
    ' Edit History : Gaurav
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = SetupListView()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisableInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
    'Dim result As Integer = 0
    'Const kMethodName As String = "DisableInterface"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    '
    '
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory() As Integer
    'Dim result As Integer = 0
    'Const kMethodName As String = "CheckMandatory"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Gaurav
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ResizeInterface"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function



    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgent.Click
        m_lReturn = ClearInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdAgent_Click", "ClearInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetAgentInfo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            gPMFunctions.RaiseError("cmdAgent_Click", "GetAgentInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            m_lReturn = SearchResults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdNew.Enabled = True
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: GetBGHolderInfo
    '
    ' Description: Instance FindParty to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Gaurav
    ' ***************************************************************** '
    Public Function GetBankInfo() As Integer
        Dim result As Integer = 0
        Dim iACTFindBank As Object


        Dim oFindBank As iACTFindBank.Interface_Renamed
        Const kMethodName As String = "GetAgentInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindBank As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindBank, sClassName:="iACTFindBank.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindBank = temp_oFindBank

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oFindBank.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_vBankNameId = oFindBank.ShortCode

            m_sShortCode = oFindBank.ShortCode

            m_sAccountName = oFindBank.AccountName

            txtBank.Text = m_sShortCode
            ' Destroy Find Party object

            oFindBank.Dispose()
            oFindBank = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function


    Private Sub cmdBank_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBank.Click
        m_lReturn = GetBankInfo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub CmdClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdClient.Click
        m_lReturn = ClearInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CmdClient_Click", "ClearInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetBGHolderInfo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            m_lReturn = SearchResults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdNew.Enabled = True
        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        If lvwsearchdetails.Items.Count > 0 Then
            If CStr(m_vResultArray(MainModule.ENBankGuarantee.BGStatusDesc, m_lListSelectedItem - 1)) = "Active" Then
                m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf lvwsearchdetails.Items.Count > 0 Then
                m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If
        End If

        'm_lReturn = ProcessBG(Mode:=PMEdit)
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        m_lReturn = DataToProperties()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdFindNow_Click", "DataToProperties Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CType(SearchResults(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = DataToInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdFindNow_Click", "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub


    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
        Dim bMsg As String = ""

        m_lReturn = CType(ValidatePartyCode(), gPMConstants.PMEReturnCode)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            MessageBox.Show("Please select a Party to proceed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            MessageBox.Show("Party Details Not Found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If m_lListSelectedItem = 0 And m_vPartyCnt = 0 And m_vAgentCnt = 0 Then
                MessageBox.Show("No Party is Selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf m_vAgentCnt <> 0 Then
                bMsg = CStr(MessageBox.Show("Do you want to add Bank Guarantee Details for " & m_vAgentCode & " ?", "Question", MessageBoxButtons.YesNo))
                If bMsg = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            ElseIf m_vPartyCnt <> 0 Then
                bMsg = CStr(MessageBox.Show("Do you want to add Bank Guarantee Details for " & m_vPartyCode & " ?", "Add Bank Guarantee", MessageBoxButtons.YesNo))
                If bMsg = "6" Then
                    m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click
        m_lReturn = ClearInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdPolicy_Click", "ClearInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub

    Private Sub cmdPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicy.Click
        m_lReturn = ClearInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdPolicy_Click", "ClearInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = GetPolicyInfo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            gPMFunctions.RaiseError("cmdPolicy_Click", "GetPolicyInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            m_lReturn = SearchResults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdNew.Enabled = True
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: FormIntialise
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:Gaurav
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Initialise"

        Try

            iPMFunc.ShowFormInTaskBar_Attach()




            ' Check if already initialised
            If m_bIsInitialised Then
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRFindBankGuarantee.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' hold Initialised status
            m_bIsInitialised = True



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: FormLoad
    '
    ' Description: Loads all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:Gaurav
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try
            'developer guide no. 220
            Me.cboBGStatus.FirstItem = ""
            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Display all language specific captions.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Form_Query Unload
    '
    ' Description: Store all Property Details before unloading form
    '
    ' Date:11/07/00
    '
    ' Edit History:Gaurav
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub
    ' ***************************************************************** '
    ' Name:Form_Resize
    '
    ' Description: Resize the the controls on form
    '
    ' Date:11/07/00
    '
    ' Edit History:Gaurav
    ' ***************************************************************** '
    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch



            Exit Sub
        End Try


    End Sub



    Private Sub Form_Terminate_Renamed()
        If Not (m_oParty Is Nothing) Then

            m_oParty.Dispose()
            m_oParty = Nothing
        End If
    End Sub


    Public Function ProcessBG(ByRef Mode As gPMConstants.PMEComponentAction) As Integer
        Dim result As Integer = 0
        Dim iSIRBankGuarantee As Object
        ' Instantiate the Bank Guarantee Component

        Dim oBankGuarantee As iSIRBankGuarantee.Interface_Renamed
        Const kMethodName As String = "GetBGHolderInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oBankGuarantee As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankGuarantee, sClassName:="iSIRBankGuarantee.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oBankGuarantee = temp_oBankGuarantee

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iSIRBankGuarantee.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Mode = gPMConstants.PMEComponentAction.PMEdit Then
                ' Set component properties and start interface

                oBankGuarantee.PartyCnt = m_vResultArray(MainModule.ENBankGuarantee.PartyCnt, m_lListSelectedItem - 1) 'lvwsearchdetails.ListItems(m_lListSelectedItem).ListSubItems(ENBankGuarantee.PartyCnt).Text

                oBankGuarantee.BGId = m_vResultArray(MainModule.ENBankGuarantee.BGId, m_lListSelectedItem - 1) '   lvwsearchdetails.ListItems(m_lListSelectedItem).ListSubItems(ENBankGuarantee.BGId).Text

                oBankGuarantee.LoadChildEdit = True
            ElseIf Mode = gPMConstants.PMEComponentAction.PMAdd Then
                If m_vPartyCnt <> 0 Then

                    oBankGuarantee.PartyCnt = m_vPartyCnt

                    oBankGuarantee.PartyCode = m_vPartyCode


                    oBankGuarantee.PartyName = m_vPartyResolvedName
                ElseIf m_vAgentCnt <> 0 Then

                    oBankGuarantee.PartyCnt = m_vAgentCnt

                    oBankGuarantee.PartyCode = m_vAgentCode


                    oBankGuarantee.PartyName = m_vAgentResolvedName
                End If


                oBankGuarantee.LoadChildAdd = True
            ElseIf Mode = gPMConstants.PMEComponentAction.PMView Then
                ' Set component properties and start interface
                'oBankGuarantee.ResolvedName = m_vResultArray(ENBankGuarantee.PartyCnt, m_lListSelectedItem - 1)

                oBankGuarantee.PartyCnt = m_vResultArray(MainModule.ENBankGuarantee.PartyCnt, m_lListSelectedItem - 1)

                oBankGuarantee.BGId = m_vResultArray(MainModule.ENBankGuarantee.BGId, m_lListSelectedItem - 1)

                oBankGuarantee.LoadChildView = True
            End If


            m_lReturn = oBankGuarantee.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' Destroy Bank Guarantee object

            oBankGuarantee.Dispose()
            oBankGuarantee = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetBGHolderInfo(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function


    'Private Sub lvwsearchdetails_ItemClick(ByVal Item As ListViewItem)
    '    m_lListSelectedItem = lvwsearchdetails.FocusedItem.Index + 1
    '    cmdEdit.Enabled = True
    '    If CStr(m_vResultArray(MainModule.ENBankGuarantee.BGStatusDesc, m_lListSelectedItem - 1)) = "Active" Then
    '        cmdEdit.Text = "&Edit"
    '    Else
    '        cmdEdit.Text = "&View"
    '    End If
    'End Sub

    'Start - Sankar - Bank Guarantee Bug Fixing
    Private Function ValidatePartyCode() As Integer
        Dim result As Integer = 0
        Dim sPartyShortname As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sAgent As String = gPMFunctions.ToSafeString(txtAgent.Text, "").Trim()
        Dim sClient As String = gPMFunctions.ToSafeString(txtBGHolder.Text, "").Trim()

        If sAgent.Trim() = "" And sClient.Trim() = "" Then
            cmdNew.Enabled = False
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If sAgent <> "" Then

                m_lReturn = g_oBusiness.GetPartyShortname(m_vAgentCnt, sPartyShortname)
            ElseIf sClient <> "" Then

                m_lReturn = g_oBusiness.GetPartyShortname(m_vPartyCnt, sPartyShortname)
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                cmdNew.Enabled = False
                Return gPMConstants.PMEReturnCode.PMNotFound
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cmdNew.Enabled = False
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                If sPartyShortname.Trim() = sAgent Or sPartyShortname.Trim() = sClient Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                Else
                    cmdNew.Enabled = False
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If
            '        m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=g_oBusiness, _
            ''        sClassName:="bSIRFindBankGuarantee.Business", _
            ''        vInstanceManager:="ClientManager")
            '
            '        ' Check for errors.
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Failed to get an instance of the business object.
            '            ValidateWhileADD = False
            '        End If

        End If
        Return result
    End Function
    'End - Sankar - Bank Guarantee Bug Fixing

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub

    Private Sub lvwsearchdetails_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwsearchdetails.DoubleClick
        If lvwsearchdetails.Items.Count > 0 Then
            If CStr(m_vResultArray(MainModule.ENBankGuarantee.BGStatusDesc, lvwsearchdetails.FocusedItem.Index + 1 - 1)) = "Active" Then
                m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf lvwsearchdetails.Items.Count > 0 Then
                m_lReturn = CType(ProcessBG(Mode:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessBG", "ProcessBG Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If
    End Sub

    Private Sub lvwsearchdetails_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwsearchdetails.SelectedIndexChanged
        If Not IsNothing(lvwsearchdetails.FocusedItem) Then
            m_lListSelectedItem = lvwsearchdetails.FocusedItem.Index + 1
            cmdEdit.Enabled = True
            If CStr(m_vResultArray(MainModule.ENBankGuarantee.BGStatusDesc, m_lListSelectedItem - 1)) = "Active" Then
                cmdEdit.Text = "&Edit"
            Else
                cmdEdit.Text = "&View"
            End If
        End If
    End Sub


End Class
