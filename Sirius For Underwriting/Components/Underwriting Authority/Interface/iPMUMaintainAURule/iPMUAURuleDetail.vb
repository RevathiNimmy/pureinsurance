Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDetail
    Inherits System.Windows.Forms.Form

    'Developer Guide No. 69
    Private frmRuleSetList As frmRuleSetList
    Private Const ACClass As String = "frmDetail"

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_vProductsLookup(,) As Object
    Private m_vAuthorityLevelsLookup(,) As Object

    Private m_lTag As Integer
    Private m_lDetailType As Integer

    Private m_sLookupTableName As String = ""

    ' Object parameter members.
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iSourceId As Integer
    Private m_sCallingAppName As String = ""

    ' AuthorityLevelId
    Private m_lAuthorityLevelId As Integer
    ' ProductId
    Private m_lProductId As Integer
    ' IsUnderwriter
    Private m_bIsUnderwriter As Boolean
    ' RuleSetId
    Private m_lRuleSetId As Integer
    ' TransactionTypeId
    Private m_lTransactionTypeId As Integer
    ' RuleSetDescription
    Private m_sRuleSetDescription As String = ""
    ' AuthorityLevelDescription
    Private m_sAuthorityLevelDescription As String = ""
    ' ProductDescription
    Private m_sProductDescription As String = ""

    Public Property ProductDescription() As String
        Get
            Return m_sProductDescription
        End Get
        Set(ByVal Value As String)
            m_sProductDescription = Value
        End Set
    End Property


    Public Property AuthorityLevelDescription() As String
        Get
            Return m_sAuthorityLevelDescription
        End Get
        Set(ByVal Value As String)
            m_sAuthorityLevelDescription = Value
        End Set
    End Property


    Public Property RuleSetDescription() As String
        Get
            Return m_sRuleSetDescription
        End Get
        Set(ByVal Value As String)
            m_sRuleSetDescription = Value
        End Set
    End Property


    Public Property TransactionTypeId() As Integer
        Get
            Return m_lTransactionTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeId = Value
        End Set
    End Property


    Public Property RuleSetID() As Integer
        Get
            Return m_lRuleSetId
        End Get
        Set(ByVal Value As Integer)
            m_lRuleSetId = Value
        End Set
    End Property


    Public Property IsUnderwriter() As Boolean
        Get
            Return m_bIsUnderwriter
        End Get
        Set(ByVal Value As Boolean)
            m_bIsUnderwriter = Value
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property


    Public Property AuthorityLevelId() As Integer
        Get
            Return m_lAuthorityLevelId
        End Get
        Set(ByVal Value As Integer)
            m_lAuthorityLevelId = Value
        End Set
    End Property


    Public Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
        Set(ByVal Value As Integer)
            m_lErrorNumber = Value
        End Set
    End Property


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property


    Public Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim sName As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                ' Assign the details from the business object
                ' to the data storage.
                m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************



            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            '
            '    If (g_oBusiness Is Nothing) Then
            '        ' Get an instance of the business object via
            '        ' the public object manager.
            '        m_lReturn = g_oObjectManager.GetInstance( _
            ''            oObject:=g_oBusiness, _
            ''            sClassName:="bSIRMaintainAURule.Business", _
            ''            vInstanceManager:=PMGetViaClientManager)
            '
            '        ' Check for errors.
            '        If (m_lReturn <> PMTrue) Then
            '            ' Failed to get an instance of the business object.
            '            m_lErrorNumber = PMFalse
            '
            '            ' Display error stating the problem.
            '
            '            ' Get description from the resource file.
            '            sTitle$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lID:=ACBusinessFailTitle, _
            ''                iDataType:=PMResString)
            '
            '            sMessage$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lID:=ACBusinessFail, _
            ''                iDataType:=PMResString)
            '
            '            ' Display message.
            '            MsgBox sMessage$, vbCritical, sTitle$
            '
            '            GetBusiness = PMFalse
            '
            '            Exit Function
            '        End If
            '    End If
            '
            '    m_lReturn& = g_oBusiness.SetProcessModes( _
            ''        vTask:=CVar(m_iTask%), _
            ''        vNavigate:=CVar(m_lNavigate&), _
            ''        vProcessMode:=CVar(m_lProcessMode&), _
            ''        vTransactionType:=CVar(m_sTransactionType$), _
            ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to process the interface.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to set the process modes for the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '
            '        Exit Function
            '    End If
            '
            '    ' Set the business keys.
            '    ' {* USER DEFINED CODE (Begin) *}
            '    ' {* USER DEFINED CODE (End) *}
            '
            '    g_oBusiness.GISDataModelId = m_lGISDataModelId
            '
            '    g_oBusiness.GISObjectId = m_lGISObjectId
            '
            ''    m_lReturn& = g_oBusiness.GetDetails(r_vDataDictionary:=m_vDataDictionary)
            ''
            ''    ' Check for errors
            ''    If (m_lReturn& <> PMTrue) Then
            ''        ' Failed to get details.
            ''        If (m_lReturn <> PMNotFound) Then
            ''            GetBusiness = PMFalse
            ''
            ''            ' Log Error.
            ''            LogMessage _
            '''                iType:=PMLogError, _
            '''                sMsg:="Failed to get details from the business object", _
            '''                vApp:=ACApp, _
            '''                vClass:=ACClass, _
            '''                vMethod:="GetBusiness"
            ''
            ''            'Don't exit, we need to terminate
            '''            Exit Function
            ''        End If
            ''    End If
            '
            '    If (m_iTask = PMEdit) Then
            '        m_lReturn& = g_oBusiness.GetInclusions(m_vRules(ACUARuleId, m_iRulesIndex), r_vInclusions:=m_vInclusions)
            '
            '    End If

            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '    End If
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer
        Dim result As Integer = 0
        Dim iListIndex As Integer

        Dim vTransactionTypeArray(,) As Object
        Const ACId As Integer = 0
        Const ACDescription As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iListIndex = -1

            'Products
            If Information.IsArray(m_vProductsLookup) Then
                For iCount As Integer = 0 To m_vProductsLookup.GetUpperBound(1)
                    Dim cboProduct_NewIndex As Integer = -1
                    cboProduct_NewIndex = cboProduct.Items.Add(CStr(m_vProductsLookup(1, iCount)))
                    VB6.SetItemData(cboProduct, cboProduct_NewIndex, CInt(m_vProductsLookup(0, iCount)))
                    If CDbl(m_vProductsLookup(0, iCount)) = m_lProductId Then
                        iListIndex = iCount
                    End If
                Next iCount
                'cboProduct.ListIndex = iListIndex
                If iListIndex > -1 Then
                    cboProduct.Text = CStr(m_vProductsLookup(1, iListIndex))
                Else
                    cboProduct.SelectedIndex = iListIndex
                End If
            End If

            iListIndex = -1

            'Authority Levels
            If Information.IsArray(m_vAuthorityLevelsLookup) Then
                For iCount As Integer = 0 To m_vAuthorityLevelsLookup.GetUpperBound(1)
                    Dim cboAuthLevel_NewIndex As Integer = -1
                    cboAuthLevel_NewIndex = cboAuthLevel.Items.Add(CStr(m_vAuthorityLevelsLookup(1, iCount)))
                    VB6.SetItemData(cboAuthLevel, cboAuthLevel_NewIndex, CInt(m_vAuthorityLevelsLookup(0, iCount)))
                    If CDbl(m_vAuthorityLevelsLookup(0, iCount)) = m_lAuthorityLevelId Then
                        iListIndex = iCount
                    End If
                Next iCount
                cboAuthLevel.SelectedIndex = iListIndex
            End If

            'sj 01/04/2003 - start
            'Transaction Types
            iListIndex = -1


            m_lReturn = g_oBusiness.GetTransactionTypeList(r_vResultArray:=vTransactionTypeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="g_oBusiness.GetTransactionTypeList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
                Return result
            End If

            If Information.IsArray(vTransactionTypeArray) Then

                For i As Integer = 0 To vTransactionTypeArray.GetUpperBound(1)
                    With cboTransType
                        Dim cboTransType_NewIndex As Integer = -1

                        cboTransType_NewIndex = .Items.Add(CStr(vTransactionTypeArray(ACDescription, i)))

                        VB6.SetItemData(cboTransType, cboTransType_NewIndex, CInt(vTransactionTypeArray(ACId, i)))
                    End With
                Next i
            End If

            '    'Transaction Types
            '    cboTransType.AddItem SIRTransCodeDescNewBusiness
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdNewBusiness
            '    cboTransType.AddItem SIRTransCodeDescAdditionalPremium
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdAdditionalPremium
            '    cboTransType.AddItem SIRTransCodeDescReturnPremium
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdReturnPremium
            '    cboTransType.AddItem SIRTransCodeDescRenewal
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdRenewal
            '    cboTransType.AddItem SIRTransCodeDescClaimOpen
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdClaimOpen
            '    cboTransType.AddItem SIRTransCodeDescClaimRevision
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdClaimRevision
            '    cboTransType.AddItem SIRTransCodeDescClaimPaid
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdClaimPaid
            '    'sj 17/12/2002 - start
            '    'PS104
            '    cboTransType.AddItem SIRTransCodeDescBackdatedCancellation
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdBackdatedCancellation
            '    cboTransType.AddItem SIRTransCodeDescBackdatedMTA
            '    cboTransType.ItemData(cboTransType.NewIndex) = SIRTransCodeIdBackdatedMTA
            '    'sj 17/12/2002 - end
            'sj 01/04/2003 - end

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = g_oBusiness.SetProcessModes(vTask:=m_iTask)

            '    ' Update the business object.
            '    m_lReturn& = g_oBusiness.SetProperties(lRuleId:=m_lRuleId, _
            ''                                            lfailureconsequence:=m_lFailureConsequenceId, _
            ''                                            lGISObjectId:=m_lGISObjectId, lGISPropertyId:=m_lGISPropertyId, _
            ''                                            lLowerBound:=m_lLowerBound, lUpperBound:=m_lUpperBound, _
            ''                                            sDescription:=m_sDescription, sCode:=m_sCode, _
            ''                                            dtEffectiveDate:=m_dtEffectiveDate, vInclusions:=m_vInclusionsUpdate)




            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    '' ***************************************************************** '
    ''
    '' Name: UpdateBusiness
    ''
    '' Description:
    ''
    '' History: 13/07/2000 Tomo - Created.
    ''
    '' ***************************************************************** '
    'Public Function UpdateBusiness() As Long
    '
    'Dim sTitle As String
    'Dim sMessage As String
    'Dim bOK As Boolean
    '
    '    On Error GoTo Err_UpdateBusiness
    '
    '    UpdateBusiness = PMTrue
    '
    '    ' Get an instance of the business object via
    '    ' the public object manager.
    '    m_lReturn = g_oObjectManager.GetInstance( _
    ''        oObject:=g_oBusiness, _
    ''        sClassName:="bSIRMaintainAURule.Business", _
    ''        vInstanceManager:=PMGetViaClientManager)
    '
    '    ' Check for errors.
    '    If (m_lReturn <> PMTrue) Then
    '        ' Failed to get an instance of the business object.
    '        m_lErrorNumber = PMFalse
    '
    '        ' Display error stating the problem.
    '
    '        ' Get description from the resource file.
    '        sTitle$ = iPMFunc.GetResData( _
    ''            iLangID:=g_iLanguageID%, _
    ''            lID:=ACBusinessFailTitle, _
    ''            iDataType:=PMResString)
    '
    '        sMessage$ = iPMFunc.GetResData( _
    ''            iLangID:=g_iLanguageID%, _
    ''            lID:=ACBusinessFail, _
    ''            iDataType:=PMResString)
    '
    '        ' Display message.
    '        MsgBox sMessage$, vbCritical, sTitle$
    '
    '        UpdateBusiness = PMFalse
    '
    '        Exit Function
    '    End If
    '
    '    m_lReturn& = g_oBusiness.SetProcessModes( _
    ''        vTask:=CVar(m_iTask%), _
    ''        vNavigate:=CVar(m_lNavigate&), _
    ''        vProcessMode:=CVar(m_lProcessMode&), _
    ''        vTransactionType:=CVar(m_sTransactionType$), _
    ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        ' Failed to process the interface.
    '        m_lErrorNumber& = PMFalse
    '
    '        ' Log Error Message
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to set the process modes for the business object", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="UpdateBusiness"
    '
    '        Exit Function
    '    End If
    '
    '    ' Set the business keys.
    '    ' {* USER DEFINED CODE (Begin) *}
    '    ' {* USER DEFINED CODE (End) *}
    '
    ''    m_lReturn = CheckCode(v_lScreenId:=m_lScreenId, _
    '''                          v_sCode:=m_vScreenHeader(ACHCode, 0), _
    '''                          r_bOK:=bOK)
    ''
    ''    If (m_lReturn = PMTrue) Then
    ''        If bOK Then
    '
    '            g_oBusiness.SourceId = m_iSourceId
    '
    '            g_oBusiness.GISDataModelId = m_lGISDataModelId
    '
    '            m_lReturn& = g_oBusiness.Update()
    '
    '            ' Check for errors.
    '            If (m_lReturn& <> PMTrue) Then
    '                UpdateBusiness = PMFalse
    '
    '                ' Log Error.
    '                LogMessage _
    ''                    iType:=PMLogError, _
    ''                    sMsg:="Failed to update the business object", _
    ''                    vApp:=ACApp, _
    ''                    vClass:=ACClass, _
    ''                    vMethod:="UpdateBusiness"
    '            End If
    '
    ''        End If
    ''    End If
    '
    '    ' Terminate the business object
    '    m_lReturn& = g_oBusiness.Terminate()
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        m_lErrorNumber& = PMFalse
    '
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to terminate the business object", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="UpdateBusiness"
    '    End If
    '
    '    ' Destroy the instance of the business object
    '    ' from memory.
    '    Set g_oBusiness = Nothing
    '
    '    Exit Function
    '
    'Err_UpdateBusiness:
    '
    '    UpdateBusiness = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="UpdateBusiness Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="UpdateBusiness", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetNext(r_vDataDictionary:=m_vDataDictionary, _
            'r_vScreenHeader:=m_vScreenHeader, _
            'r_vScreenDetails:=m_vScreenDetails, _
            'r_vChildScreenDetails:=m_vChildScreenDetails)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SpacifyName
    '
    ' Description: Replaces underscores with spaces.
    '
    ' History: 24/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SpacifyName) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SpacifyName(ByRef sOriginal As String, ByRef sNew As String) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sNew = ""
    '
    'For 'iTemp As Integer = 1 To sOriginal.Length
    'If sOriginal.Substring(iTemp - 1, 1) = "_" Then
    'sNew = sNew & " "
    'Else
    'sNew = sNew & sOriginal.Substring(iTemp - 1, 1)
    'End If
    '
    'Next iTemp
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpacifyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpacifyName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Get all of the lookup values.

            m_lReturn = g_oBusiness.GetLookupValues(v_iLanguageID:=g_iLanguageID, v_dtEffectiveDate:=DateTime.Now, v_sTableName:="Product", r_vLookupArray:=m_vProductsLookup)


            m_lReturn = g_oBusiness.GetLookupValues(v_iLanguageID:=g_iLanguageID, v_dtEffectiveDate:=DateTime.Now, v_sTableName:="Authority_Level_Type", r_vLookupArray:=m_vAuthorityLevelsLookup)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateForm
    '
    ' Description: Is the data on the interface ok
    '
    '
    ' ***************************************************************** '
    Public Function ValidateForm() As Integer
        Dim result As Integer = 0
        Dim sTitle, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If cboAuthLevel.SelectedIndex = -1 Then

                sMsg = sMsg & Strings.Chr(13) & Strings.Chr(10) & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAuthorityLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If cboProduct.SelectedIndex = -1 Then

                sMsg = sMsg & Strings.Chr(13) & Strings.Chr(10) & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If cboTransType.SelectedIndex = -1 Then

                sMsg = sMsg & Strings.Chr(13) & Strings.Chr(10) & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblTransactionType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            'Developer Guide No. 26
            If lblRuleSet.Text.Trim() = "" Then

                sMsg = sMsg & Strings.Chr(13) & Strings.Chr(10) & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblRuleSet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            'If sMsg.Trim() <> "" Then
            If Not String.IsNullOrEmpty(sMsg) Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryFields, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & Strings.Chr(13) & Strings.Chr(10) & sMsg

                MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            m_lAuthorityLevelId = VB6.GetItemData(cboAuthLevel, cboAuthLevel.SelectedIndex)
            m_sAuthorityLevelDescription = cboAuthLevel.Text.Trim()
            m_lProductId = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)
            m_sProductDescription = cboProduct.Text.Trim()
            m_lTransactionTypeId = VB6.GetItemData(cboTransType, cboTransType.SelectedIndex)
            m_sTransactionType = cboTransType.Text.Trim()
            m_bIsUnderwriter = (chkIsUnderwriter.CheckState = CheckState.Checked)

            'Rule Set details will already have been set.

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If ValidateForm() <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Everything OK, so we can hide the interface.
            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRuleSet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRuleSet.Click

        Try
            'Developer Guide No. 69
            frmRuleSetList = New frmRuleSetList
            With frmRuleSetList

                .ShowDialog()

                If .Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_lRuleSetId = .SelectedRuleSetId
                    m_sRuleSetDescription = .SelectedRuleSetDesc


                    'Developer Guide No. 51
                    lblRuleSet.Text = " " & m_sRuleSetDescription
                End If

            End With
            frmRuleSetList.Close()
            frmRuleSetList = Nothing
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Rule Set command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRuleSet_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()
        'Dim sMessage As String
        'Dim sTitle As String
        '
        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue


            '    ' Create an instance of the general interface object.
            '    Set m_oGeneral = New iPMUMaintainAURule.General
            '
            '    ' Create an instance of the form control object.
            '    Set m_oFormFields = New iPMFormControl.FormFields
            '
            '    ' Set language
            '    m_oFormFields.LanguageID = g_iLanguageID%

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub


    Private Sub frmDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' m_lReturn = GetBusiness()
            m_lReturn = DisplayLookupDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface values passed in.
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
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
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}#

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            Dim m_ctlTabFirstLast(1, 0) As Object

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    Set m_ctlTabFirstLast(ACControlStart, 0) = tvwDataDictionary
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = tvwDataDictionary

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLinkTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabRuleDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            lblAuthLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAuthorityLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkIsUnderwriter.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblIsUnderwriter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTransType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblTransactionType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRuleSet.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblRuleSet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        '    If Not (g_oBusiness Is Nothing) Then
        '        ' Terminate the business object
        '        m_lReturn& = g_oBusiness.Terminate()
        '
        '        ' Check for errors.
        '        If (m_lReturn& <> PMTrue) Then
        '            m_lErrorNumber& = PMFalse
        '
        '            ' Log Error.
        '            LogMessage _
        ''                iType:=PMLogError, _
        ''                sMsg:="Failed to terminate the business object", _
        ''                vApp:=ACApp, _
        ''                vClass:=ACClass, _
        ''                vMethod:="Form_QueryUnload"
        '        End If
        '
        '        ' Destroy the instance of the business object
        '        ' from memory.
        '        Set g_oBusiness = Nothing
        '    End If

        eventArgs.Cancel = Cancel <> 0
    End Sub


    Private Sub Form_Terminate_Renamed()

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


        ' Reset the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    End Sub
    '' ***************************************************************** '
    ''
    '' Name: AddInclusion
    ''
    '' Description:
    ''
    '' History: 06/11/2000 RWH - Created.
    ''
    '' ***************************************************************** '
    'Private Function AddInclusion() As Long
    'Dim iSelectedIndex As Integer
    'Dim oListitem As ListItem
    'Dim iInclusionCount As Integer
    'Dim iTag As Integer
    'Dim oInclusion As Inclusion
    '
    '    On Error GoTo Err_AddInclusion
    '
    '    AddInclusion = PMTrue
    '
    '
    '    'populate lookup array and pass into Inclusion form.
    '    If (m_sLookupTableName <> "") Then
    '        m_lReturn = g_oBusiness.GetLookupValues(g_iLanguageID, _
    ''                                                Now, _
    ''                                                m_sLookupTableName, _
    ''                                                m_vLookupArray)
    '    End If
    '
    '    If (m_lUserDefHeaderId <> 0) Then
    '        m_lReturn = g_oBusiness.GetGISUserDefDetail(m_lUserDefHeaderId, _
    ''                                                m_vLookupArray)
    '    End If
    '
    '    frmInclusion.DetailType = m_lDetailType
    '
    '    frmInclusion.PropertyDescription = pnlProperty.Caption
    '
    '    frmInclusion.LookupArray = m_vLookupArray
    '
    '    frmInclusion.Show vbModal
    '
    '    If (frmInclusion.Status = PMOK) Then
    '        iSelectedIndex = frmInclusion.SelectedIndex
    '        For iInclusionCount = 1 To lvwInclusions.ListItems.Count
    '            iTag = lvwInclusions.ListItems(iInclusionCount).Tag
    '            If (iSelectedIndex = iTag) Then
    '                MsgBox "Already got this one !"
    '                Exit Function
    '            End If
    '        Next iInclusionCount
    '
    '        'Add new record to Update array. This will be added to database when
    '        'OK this form.
    '
    ''        Set oInclusion = New Inclusion
    ''        oInclusion.Include = frmInclusion.DetailType
    ''        oInclusion.Value = m_vLookupArray(ACGISUserDefDetDescription, iSelectedIndex)
    ''        oInclusion.Action = PMAdd
    ''        m_colInclusions.Add Item:=oInclusion, Key:=iselecteditem
    '
    '
    '        If (IsArray(m_vInclusionsUpdate)) Then
    '            ReDim Preserve m_vInclusionsUpdate(ACInclusionFieldMaxIndex, UBound(m_vInclusionsUpdate, 2) + 1)
    '        Else
    '            ReDim m_vInclusionsUpdate(ACInclusionFieldMaxIndex, 0)
    '        End If
    '        m_lDetailType = frmInclusion.DetailType
    '        m_vInclusionsUpdate(ACUARuleDetailInclude, UBound(m_vInclusionsUpdate, 2)) = frmInclusion.DetailType
    '        m_vInclusionsUpdate(ACUARuleDetailValue, UBound(m_vInclusionsUpdate, 2)) = m_vLookupArray(ACGISUserDefDetDescription, iSelectedIndex)
    '        m_vInclusionsUpdate(ACUARuleDetailAction, UBound(m_vInclusionsUpdate, 2)) = PMAdd
    '
    '        If (lvwInclusions.ListItems.Count = 0) Then
    '            If (m_lDetailType = ACInclude) Then
    '                fraIncEx.Caption = "Inclusions"
    '            Else
    '               fraIncEx.Caption = "Exclusions"
    '            End If
    '        End If
    '
    '        'Add new record to ListView.
    '        Set oListitem = lvwInclusions.ListItems.Add(Text:=m_vLookupArray(ACGISUserDefDetDescription, iSelectedIndex))
    '        oListitem.Tag = iSelectedIndex
    '
    '    End If
    '
    '    Unload frmInclusion
    '
    '    Exit Function
    '
    'Err_AddInclusion:
    '
    '    AddInclusion = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="AddInclusion Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="AddInclusion", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ProcessCommand) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ProcessCommand() As Integer
    '
    'Dim result As Integer = 0
    'Dim iMsgResult As DialogResult
    'Dim sMessage, sTitle As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
    'If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
    ' Update the business from the interface.
    'm_lReturn = CType(InterfaceToBusiness(), gPMConstants.PMEReturnCode)
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to update business.
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    'End Select
    '
    'If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
    ' Check the details havn't changed.
    '        m_lReturn& = m_oBusiness.Cancel()
    '
    '        If (m_lReturn& = PMDataChanged) Then
    ' Get string messages
    '

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
    '
    ' Check message result.
    'If iMsgResult = System.Windows.Forms.DialogResult.No Then
    ' Set return to false, meaning
    ' don't cancel.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '        End If
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set required ListIndex.
            If m_lTransactionTypeId <> 0 Then
                For iCount As Integer = 0 To cboTransType.Items.Count - 1
                    If VB6.GetItemData(cboTransType, iCount) = m_lTransactionTypeId Then
                        cboTransType.SelectedIndex = iCount
                        Exit For
                    End If
                Next iCount
            End If

            If m_bIsUnderwriter Then
                chkIsUnderwriter.CheckState = CheckState.Checked
            Else
                chkIsUnderwriter.CheckState = CheckState.Unchecked
            End If


            'Developer Guide No. 26
            lblRuleSet.Text = " " & m_sRuleSetDescription

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmDetail_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
