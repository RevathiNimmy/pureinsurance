Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Word = Microsoft.Office.Interop.Word
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' DJM 27/05/2002 : Allowed merge button to appear when adding a text file.
    ' DJM 01/05/2002 : Added merge button
    ' KN (CMG) 171002: View Merge facility (F00055299) plus enable zipping up of multiple files to include HTML dependencies
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}

    Private m_lMode As Integer

    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer
    Private m_lEventCnt As Integer

    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer

    Private m_lDocumentTypeId As Integer
    Private m_lDocumentTemplateId As Integer
    Private m_sDocumentTemplateDescription As String = ""
    Private m_sDocumentTemplateDesc As String = ""

    Private m_lSlotNumber As Integer
    Private m_lFileNumber As Integer
    Private m_lOrigFileNumber As Integer
    Private m_lOldFileNumber As Integer

    Private m_sClient As String = ""
    Private m_sServer As String = ""

    Private m_lSourceId As Integer

    Private m_sClientDocument As String = ""
    Private m_oWord As Word.Application
    Private m_oDocument As Word.Document

    'Holds what class of OLE we're using
    Private m_sClass As String = ""
    Private m_sWordVersion As String = ""
    'Allowed values are:
    'Word.Document.8 for Word 97
    'Word.Document.9 for Word 2000

    Private m_bSetUp As Boolean

    Private m_sZIP_DIRECTORY As String = ""

    Private m_vDocClauseLinkArray() As Object 'RWH(21/08/2000) RSAIB Process 12

    Private m_sFieldStartMarker As String = ""
    Private m_sFieldEndMarker As String = ""
    Private m_iFieldMarkerLength As Integer
    Private m_sDocFileExtension As String = ""

    Private m_bSpoolMessage As Boolean

    Private m_lSpoolNumber As Integer
    'KN (CMG) Start 171002
    Private m_bTempDir As Boolean
    'KN (CMG) End 171002
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBTextFile.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRTextFile.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields


    Private m_oSIRDOCAPI As bSIRDOCAPI.Form
    'Private m_oSIRDOCAPI As bSIRDOCAPI.Form


    Private m_oDocSpooler As bSIRDocSpooler.Business
    'Private m_oDocSpooler As bSIRDocSpooler.Business

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_bUniqueClientDirNeedsDeleting As Boolean 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup

    Private m_lWordHwnd As Integer

    Private m_bButtonNavigate As Boolean
    Private m_bButtonEdit As Boolean
    Private m_bButtonClose As Boolean
    Private m_bButtonMerge As Boolean
    Private m_bButtonOK As Boolean
    Private m_bButtonCancel As Boolean
    Private m_lButtonHelp As Boolean
    Private m_bIsDocChanged As Boolean
    Private m_sFileCopyMsg As String = ""

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public Property Mode() As Integer
        Get
            Return m_lMode
        End Get
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property

    Public Property EventCnt() As Integer
        Get
            Return m_lEventCnt
        End Get
        Set(ByVal Value As Integer)
            m_lEventCnt = Value
        End Set
    End Property

    Public Property RiskCodeId() As Integer
        Get
            Return m_lRiskCodeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCodeId = Value
        End Set
    End Property

    Public Property RiskGroupId() As Integer
        Get
            Return m_lRiskGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskGroupId = Value
        End Set
    End Property

    Public Property SlotNumber() As Integer
        Get
            Return m_lSlotNumber
        End Get
        Set(ByVal Value As Integer)
            m_lSlotNumber = Value
        End Set
    End Property

    Public Property FileNumber() As Integer
        Get
            Return m_lFileNumber
        End Get
        Set(ByVal Value As Integer)
            m_lFileNumber = Value
        End Set
    End Property

    Public Property DocumentTypeId() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property

    Public Property WordVersion() As String
        Get
            Return m_sWordVersion
        End Get
        Set(ByVal Value As String)
            m_sWordVersion = Value

            SetWordVersionDependentVariables()
        End Set
    End Property

    Public Property FieldEndMarker() As String
        Get
            Return m_sFieldEndMarker
        End Get
        Set(ByVal Value As String)
            m_sFieldEndMarker = Value.Trim()
        End Set
    End Property

    Public Property DocFileExtension() As String
        Get
            Return m_sDocFileExtension
        End Get
        Set(ByVal Value As String)
            m_sDocFileExtension = Value.Trim()
        End Set
    End Property

    Public Property FieldStartMarker() As String
        Get
            Return m_sFieldStartMarker
        End Get
        Set(ByVal Value As String)
            m_sFieldStartMarker = Value.Trim()
            m_iFieldMarkerLength = m_sFieldStartMarker.Length
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_lSourceId
        End Get
        Set(ByVal Value As Integer)
            m_lSourceId = Value
        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    Public WriteOnly Property DocumentTemplateDescription() As String
        Set(ByVal Value As String)

            m_sDocumentTemplateDesc = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' ***** Mandatory *****************************************

            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=txtCode, _
            ''        lFormat:=PMFormatString, _
            ''        lFieldType:=PMString, _
            ''        lMandatory:=PMMandatory)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If


            ' ***** Non-Mandatory *************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lEntityType, lEntityCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'We already have all the details we need but we need to set it up in
            'the business object else the deletion won't work.

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            lEntityType = PMBConst.PMBClientTextFile
            lEntityCnt = m_lPartyCnt

            If m_lInsuranceFileCnt > 0 Then
                lEntityType = PMBConst.PMBPolicyTextFile
                lEntityCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt > 0 Then
                lEntityType = PMBConst.PMBClaimTextFile
                lEntityCnt = m_lClaimCnt
            End If


            m_lReturn = m_oBusiness.GetDetails(vLockMode:=gPMConstants.PMELockMode.PMNoLock, vEntityTypeId:=lEntityType, vEntityCnt:=lEntityCnt, vSlotNumber:=m_lSlotNumber)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID, lEntityTypeID, lEntityCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            lEntityTypeID = PMBConst.PMBClientTextFile
            lEntityCnt = m_lPartyCnt

            If m_lInsuranceFileCnt > 0 Then
                lEntityTypeID = PMBConst.PMBPolicyTextFile
                lEntityCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt > 0 Then
                lEntityTypeID = PMBConst.PMBClaimTextFile
                lEntityCnt = m_lClaimCnt
            End If

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vEntityTypeId:=lEntityTypeID, vEntityCnt:=lEntityCnt, vSlotNumber:=m_lSlotNumber, vFileNumber:=m_lFileNumber)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    '            m_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, _
                    'vEntityTypeId:=lEntityTypeId, vEntityCnt:=lEntityCnt, _
                    'vSlotNumber:=m_lSlotNumber, vFileNumber:=m_lFileNumber)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditDelete(lRow:=lBusinessDataID)

                    ' {* USER DEFINED CODE (End) *}
            End Select

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

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            '    m_lReturn& = GetLookupValues()

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_bSetUp = True

            '    m_lReturn& = GetLookupDetails( _
            'sLookupTable:="document_type", _
            'ctlLookup:=cboType)

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            m_bSetUp = False

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyServerToClient
    '
    ' Description: copies the template from the server to the client
    '
    ' ***************************************************************** '
    Public Function CopyServerToClient() As Integer

        Dim result As Integer = 0
        Const VB_FileAccessError As Integer = 75

        Dim sServer As String = String.Empty
        Dim sClient, sTemp, sTemp2 As String
        Dim oTemplate As Word.Document
        Dim lEntityTypeID As Integer 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup
        Dim vlEntityCnt As Integer 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetPath(sPath:=sServer)

            'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup START
            If m_lOrigFileNumber > 0 Then
                lEntityTypeID = PMBConst.PMBClientTextFile ' Default to Client Text File
                vlEntityCnt = m_lPartyCnt

                If m_lInsuranceFileCnt > 0 Then
                    lEntityTypeID = PMBConst.PMBPolicyTextFile ' Set to Policy Text File
                    vlEntityCnt = m_lInsuranceFileCnt
                End If

                If m_lClaimCnt > 0 Then
                    lEntityTypeID = PMBConst.PMBClaimTextFile ' Set to Claim Text File
                    vlEntityCnt = m_lClaimCnt
                End If

                m_lReturn = CheckFileExistsAndAllowLinkDeletion(v_sServer:=sServer, v_lEntityTypeID:=lEntityTypeID, v_lEntityCnt:=vlEntityCnt, v_lSlotNumber:=m_lSlotNumber, v_lFileNumber:=m_lOrigFileNumber)
                If m_lReturn = gPMConstants.PMEReturnCode.PMRecordDeleted Then
                    ' User answerd No, so just quit
                    result = gPMConstants.PMEReturnCode.PMRecordDeleted
                    ' Set the interface status.
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    Return result
                ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Some other error
                    Return m_lReturn
                End If
            End If

            '    'DC140503 -ISS3108 -start -added following from 1.6.9
            '    'DJM 28/01/2002 : Check to see if file exists.
            '    m_lReturn = CheckFileExistsAndAllowLinkDeletion(sServer)
            '    If (m_lReturn <> PMTrue) Then
            '        CopyServerToClient = PMFalse
            '        Exit Function
            '    End If
            '    'DC140503 -end
            'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup END

            m_lReturn = SetZipDirectory()

            'DC140503 -ISS3108 -start -added following from 1.6.9
            'DJM 08/04/2003 : If directory can't be cleared then exit function.
            m_lReturn = EnsureClientDirectoryClear()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'DC140503 -end

            'Make sure the directory's there
            sTemp = FileSystem.Dir(m_sClient, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(m_sClient)
            End If

            m_lOldFileNumber = m_lFileNumber
            'DN 01/02/02 - Ensure use right name for zip file and docs
            If m_lFileNumber = 0 Then
                m_lFileNumber = m_lDocumentTemplateId
            End If

            sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lFileNumber) & ".zip"

            'Make sure the file's not there
            'RWH(26/07/2000) Change to .htm file.
            sTemp = m_sZIP_DIRECTORY & "\Doc " & CStr(m_lFileNumber) & "." & m_sDocFileExtension

            sTemp2 = FileSystem.Dir(sTemp, FileAttribute.Normal)
            If sTemp2 <> "" Then
                Kill(sTemp)
            End If

            File.Copy(sServer, sClient)

            If File.GetAttributes(sClient) <> FileAttribute.Normal Then
                File.SetAttributes(sClient, FileAttributes.Normal)
            End If

            m_lReturn = UnZip(sClient)

            'RWH(04/09/2000) - RSAIB Process 108.
            m_lReturn = CopyFilesFromZipTemp()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 12/07/02 - Convert document if not HTML
            If CheckFileTypeIsDoc() = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)

                oTemplate = m_oWord.Documents.Add()

                m_lReturn = CheckFileName(m_sClient, m_lFileNumber)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oTemplate = Nothing
                    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                    Return result
                End If

                'm_lReturn = ConvertDocument(oTemplate, m_lFileNumber)
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    result = gPMConstants.PMEReturnCode.PMFalse
                '    oTemplate = Nothing
                '    m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                '    Return result
                'End If

                'If template has been converted then save it back
                If m_lDocumentTemplateId <> 0 Then
                    m_lReturn = CopyTemplateClientToServer(m_lDocumentTemplateId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        oTemplate = Nothing
                        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
                        Return result
                    End If
                End If

                oTemplate = Nothing
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If Information.Err().Number = VB_FileAccessError Then
                sMessage = "User does not have access to Document server: '" & m_sServer & "'"
            Else
                sMessage = "Failed to copy template from server to client"
            End If

            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    'KN (CMG) Start 171002
    ' ***************************************************************** '
    ' Name: CopyTempDirToClient
    '
    ' Description: copies the template from the temporary directory to the client
    '
    ' ***************************************************************** '
    Public Function CopyTempDirToClient() As Integer

        Dim result As Integer = 0
        Dim sTemp As String
        'KN(CMG) Start 171002
        Dim sClientTempDir, sClientFileDest, sClientFileSource As String
        'KN (CMG) End 171002

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lMode = gSIRLibrary.ACMergeMode Then
                Return result
            End If

            'KN (CMG) Start 171002 - Copy files to back up directory

            sClientFileSource = "Doc " & m_lFileNumber & "." & m_sDocFileExtension
            sClientFileDest = "Doc " & m_lFileNumber & "." & m_sDocFileExtension & "temp"
            sClientTempDir = m_sClient & "\" & sClientFileDest

            'Make sure the directory's there
            sTemp = FileSystem.Dir(sClientTempDir, FileAttribute.Directory)

            'Delete temp dir
            Directory.Delete(sClientTempDir)

            m_bTempDir = False

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy temporary folder to client", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTempDirToClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'KN (CMG) End 171002

    ' ***************************************************************** '
    ' Name: CopyClientToServer
    '
    ' Description: copies the template from the client to the server
    '
    ' ***************************************************************** '
    Public Function CopyClientToServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = String.Empty
        Dim sClient, sTemp As String
        Dim lTemp, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lMode = gSIRLibrary.ACMergeMode Then
                Return result
            End If

            'RWH(04/09/2000) - RSAIB Process 108.
            'Use new absolute directory to zip & unzip files.
            CopyFilesToZipTemp()

            'Make sure the directory's there
            sTemp = FileSystem.Dir(m_sServer, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(m_sServer)
            End If

            m_lReturn = GetAndCreatePath(sPath:=sServer)

            'DN 29/01/02 - Create Text File with correct number
            lTemp = m_lOrigFileNumber \ 500
            lTemp2 = m_lOrigFileNumber - (lTemp * 500)

            'RWH(04/09/2000) - RSAIB Process 108.
            'sClient = m_sZIP_DIRECTORY & "\Doc " & m_lOldFileNumber & ".zip"
            sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(lTemp2) & ".zip"

            m_lReturn = Zip(sClient)

            File.Copy(sClient, sServer, True)

            'Delete the local copy
            Kill(sClient)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClauseNumbersInDoc
    '
    ' Description: Searches thru' document line by line, compiling list
    '               of clauses called.
    '
    ' History: 22/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetClauseNumbersInDoc) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetClauseNumbersInDoc() As Integer
    'Dim result As Integer = 0
    'Dim iFileNum, iClauseStart As Integer
    'Dim sCurrentLine, sClauseStartMarker, sClauseCode As String
    'Dim iClauseEnd As Integer
    'Dim sDoc As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sDoc = m_sClient & "\Doc " & CStr(m_lDocumentTemplateId) & "." & m_sDocFileExtension
    '
    'sClauseStartMarker = m_sFieldStartMarker & "CL_"
    '
    ' Open the chosen template document
    'iFileNum = FileSystem.FreeFile()
    'FileSystem.FileOpen(iFileNum, sDoc, OpenMode.Input)
    '
    'Read in each line of document and analyse 1 by 1.
    'Do While Not FileSystem.EOF(iFileNum)
    '
    'sCurrentLine = FileSystem.LineInput(iFileNum)
    '
    'Debug.WriteLine(sCurrentLine)
    '
    'Check for Clause field present on this line.
    'iClauseStart = (sCurrentLine.IndexOf(sClauseStartMarker) + 1)
    '        If (iFieldStart <> 0) Then
    'If iClauseStart <> 0 Then
    'If clause is present then extract number.
    'sClauseCode = sCurrentLine.Substring(iClauseStart + sClauseStartMarker.Length - 1)
    'iClauseEnd = (sClauseCode.IndexOf(m_sFieldEndMarker) + 1)
    'sClauseCode = sClauseCode.Substring(0, iClauseEnd - 1)
    '
    'If Information.IsArray(m_vDocClauseLinkArray) Then
    ''ReDim Preserve m_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0) + 1)
    'Else
    ''ReDim m_vDocClauseLinkArray(0)
    'End If
    'm_vDocClauseLinkArray(m_vDocClauseLinkArray.GetUpperBound(0)) = sClauseCode
    '
    'End If
    '
    'Loop 
    '
    'FileSystem.FileClose(iFileNum)
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClauseNumbersInDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClauseNumbersInDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: CopyFilesFromZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesFromZipTemp() As Integer
        Dim result As Integer = 0
        Dim sTemp, sParentFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Move unzipped file to m_sClient
            sTemp = FileSystem.Dir(m_sZIP_DIRECTORY & "\", FileAttribute.Normal)

            'Make sure file exists, i.e. has been copied from server successfully.
            If FileSystem.Dir(m_sZIP_DIRECTORY & "\" & sTemp, FileAttribute.Normal) = "" Then
                MessageBox.Show("Failed to copy files to client." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Make sure files on server are in correct format.", "Document Template", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            FileSystem.Rename(m_sZIP_DIRECTORY & "\" & sTemp, m_sClient & "\" & sTemp)

            'Check for dependencies and move them to the client directory if they exist.
            sParentFile = sTemp.Substring(0, sTemp.Length - 4)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesFromZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesFromZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyFilesToZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesToZipTemp() As Integer
        Dim result As Integer = 0
        Dim sClient, sParentFile As String
        Dim lTemp, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Copy files to ZipTemp.
            'This gives us an absolute directory to zip & unzip to as apposed to the
            'client-specific processing directory.
            sClient = FileSystem.Dir(m_sClient & "\*." & m_sDocFileExtension, FileAttribute.Normal)
            '    sClient = Dir(m_sClient & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension)
            If sClient <> "" Then

                'RWH(18/10/2000) If we are using Word 97 we need to break
                'browsers link to our document.
                'If m_sWordVersion.Substring(0, 1) = "8" Then

                '    WebBrowser1.Navigate(New Uri("about:blank"))
                '    WebBrowser1.Refresh()
                '    Application.DoEvents()
                'End If

                'Copy parent file to ZipTemp.
                FileSystem.Rename(m_sClient & "\" & sClient, m_sZIP_DIRECTORY & "\" & sClient)

                'Check for dependencies and copy them to the temp zip directory if they exist.
                sParentFile = m_sClient & "\" & sClient
                sParentFile = sParentFile.Substring(0, sParentFile.Length - 4)

                'MKW100703 PN1978 Process Dependancies upon first save (in order to zip).
                If m_lOldFileNumber > 0 Then
                    m_lReturn = UpdateTemplateNumberAndDependencies(m_sZIP_DIRECTORY, m_lOldFileNumber, m_lFileNumber)
                Else
                    lTemp = m_lFileNumber \ 500
                    lTemp2 = m_lFileNumber - (lTemp * 500)
                    'm_lReturn = UpdateTemplateNumberAndDependencies(m_sZIP_DIRECTORY, m_lDocumentTemplateId, m_lFileNumber)
                    m_lReturn = UpdateTemplateNumberAndDependencies(m_sZIP_DIRECTORY, m_lDocumentTemplateId, lTemp2)
                End If
                '        'DN 16/01/02 - Rename file with correct number
                '        m_lReturn = CheckFileHasCorrectName(m_sZIP_DIRECTORY, m_lFileNumber)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesToZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckFileHasCorrectName
    '
    ' Description: Templates were formerly created with 'Doc 0' name
    '               and not converted to the correct number until
    '               they were first edited or merged. We need to check
    '               that the file we have unzipped has the correctname.
    '               If not, then we must convert it.
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckFileHasCorrectName) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckFileHasCorrectName(ByRef sPath As String, ByVal lCorrectFileNumber As Integer) As Integer
    'Dim result As Integer = 0
    'Dim sFileName, sFileNumber As String
    'Dim lTemp, lTemp2 As Integer
    'Dim sCorrectedFile, sTemp As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Get current file name.
    'sFileName = FileSystem.Dir(sPath & "\*.*", FileAttribute.Normal)
    'If sFileName <> "" Then
    'sTemp = sFileName.Substring(0, sFileName.Length - 4)
    '
    'Extract file number.
    'Do While (Not sTemp.EndsWith(" ")) And (sTemp.Length > 0)
    'sFileNumber = sTemp.Substring(sTemp.Length - 1) & sFileNumber
    'sTemp = sTemp.Substring(0, sTemp.Length - 1)
    '
    'Loop 
    '
    'DN 29/01/02 - Create Text File with correct number
    'lTemp = lCorrectFileNumber \ 500
    'lTemp2 = lCorrectFileNumber - (lTemp * 500)
    '
    'sCorrectedFile = sTemp & CStr(lTemp2) & ".htm"
    'FileSystem.Rename(sPath & "\" & sFileName, sPath & "\" & sCorrectedFile)
    '
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileHasCorrectName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileHasCorrectName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: EnsureClientDirectoryClear
    '
    ' Description: Ensures document processing directory is clear. Any
    '               files in this directory will be copied to a
    '               date-stamped backup directory and deleted from the
    '               processing directory.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function EnsureClientDirectoryClear() As Integer

        Dim result As Integer = 0
        Dim sFile As String
        Dim sDepDir As String
        Dim lAnswer As DialogResult
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFile = FileSystem.Dir(m_sClient & "\*.*", FileAttribute.Normal)
            If sFile <> "" Then

                'DC140503 -ISS3108 -start -added following from 1.6.9
                'DJM 08/04/2003 : Ask user if they have other documents open.
                sMessage = "Files have been found in your temp directory." & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Do you have any other documents open?"

                lAnswer = MessageBox.Show(sMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                If lAnswer = System.Windows.Forms.DialogResult.Yes Then
                    sMessage = "This document will not be opened." & Strings.Chr(13) & Strings.Chr(10)
                    sMessage = sMessage & "Close down the open document to be able to open this one."
                    MessageBox.Show(sMessage, "Cancelling letter", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'DC140503 -end

                'If files exist in our processing directory remove them
                Do While (sFile <> "")

                    'Move file to backup directory.
                    Kill(m_sClient & "\" & sFile)
                    sFile = FileSystem.Dir(m_sClient & "\*.*", FileAttribute.Normal)
                Loop

            End If

            'Empty everything from temp zip directory. We don't care what's in there.
            sFile = FileSystem.Dir(m_sZIP_DIRECTORY & "\*.*", FileAttribute.Normal)
            Do While (sFile <> "")
                Kill(m_sZIP_DIRECTORY & "\" & sFile)
                sDepDir = sFile.Substring(0, sFile.Length - 4)
                If Directory.Exists(m_sZIP_DIRECTORY & "\" & sDepDir) Then
                    Kill(m_sZIP_DIRECTORY & "\" & sDepDir & "\*.*")
                    Directory.Delete(m_sZIP_DIRECTORY & "\" & sDepDir)
                End If

                sFile = FileSystem.Dir(m_sZIP_DIRECTORY & "\*.*", FileAttribute.Normal)
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnsureClientDirectoryClear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnsureClientDirectoryClear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteClient
    '
    ' Description: deletes the template from the client
    '
    ' Changes: RWH(01/08/2000) Ammended to deal with htm documents.
    '          RWH(31/08/2000) Remove dependencies if they exist.
    '
    ' ***************************************************************** '
    Public Function DeleteClient() As Integer
        Dim result As Integer = 0
        Dim sParentFile, sClient As String
        Dim lTemp, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(03/08/2000) Removed use of OLE container.
            If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                ShutItDown()
            End If

            'RWH(18/10/2000) If we are using Word 97 we need to break
            'browsers link to our document.
            'If m_sWordVersion.Substring(0, 1) = "8" Then

            '    WebBrowser1.Navigate(New Uri("about:blank"))
            '    WebBrowser1.Refresh()
            '    Application.DoEvents()
            'End If

            'DN 29/01/02 - Delete Text File with correct number
            lTemp = m_lOldFileNumber \ 500
            lTemp2 = m_lOldFileNumber - (lTemp * 500)

            sClient = m_sClient & "\Doc " & CStr(lTemp2) & "." & m_sDocFileExtension

            If FileSystem.Dir(sClient, FileAttribute.Normal) <> "" Then
                Kill(sClient)

                'RWH(31/08/2000) RSAIB Process 108. Remove HTML dependencies.
                'Check for dependencies and remove them if they exist.
                sParentFile = m_sClient & "\Doc " & CStr(lTemp2)

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete text file from client", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteServer
    '
    ' Description: deletes the text file from the server
    '
    ' ***************************************************************** '
    Public Function DeleteServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC140503 -ISS3108 -start -added following from 1.6.9
            'DJM 28/01/2003 : Revert m_lfilenumber back to original value before getting file path for deletion.
            m_lFileNumber = m_lOrigFileNumber
            'DC140503 -end

            m_lReturn = GetPath(sPath:=sServer)

            Kill(sServer)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete text file from server", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UnZip(ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim sFileIn, sFileOut, sTemp As String
        Dim bSuccess As Boolean

        'MKW100703 PN1978 START - Retrieve dependancies aswell.
        Dim sDependencyDir As String = String.Empty
        Dim bDepDirNeeded As Boolean
        'MKW100703 PN1978 END - Retrieve dependancies aswell.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFileIn = sPath

            'MKW100703 PN1978 START - Retrieve dependancies aswell.

            sFileOut = sPath.Substring(0, sPath.Length - 3) & "xml"

            'Make sure the file's _not_ there
            sTemp = FileSystem.Dir(sFileOut, FileAttribute.Normal)
            If sTemp <> "" Then
                Kill(sFileOut)
            End If

            bSuccess = g_oZipper.UnZipFile(sZipFileName:=sFileIn, sDestDirectory:=m_sZIP_DIRECTORY)

            If Not bSuccess Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create a dependency directory if neccessary
            If bDepDirNeeded And Not Directory.Exists(sDependencyDir) Then
                Directory.CreateDirectory(sDependencyDir)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip the document", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function Zip(ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim sFileIn, sFileOut As String
        'KN (CMG) Start 171002
        Dim sParentFile As String
        'KN (CMG) End 171002

        Try

            sFileIn = sPath.Substring(0, sPath.Length - 3) & m_sDocFileExtension
            sFileOut = sPath

            iTemp = g_oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)

            If Not iTemp Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'KN (CMG) Start 171002
            'Enable zipping up of multiple files to include HTML dependencies.
            'Check for dependencies and add them to the zip file if they exist.
            sParentFile = sFileIn.Substring(0, sPath.Length - 4)

            Kill(sFileIn)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to zip the document", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MergeDocument
    '
    ' Description: merges the document
    '
    ' ***************************************************************** '
    Public Function MergeDocument() As Integer
        Dim result As Integer = 0
        Dim oDocManager As iPMBDocManager.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DJM 24/04/2002 : Check now done in General.GetInterfaceDetails so that
            '                 this can be used with the merge button.
            '    If (m_lMode = ACNormalMode) Then
            '    If (m_iTask <> 20) Then
            '        Exit Function
            '    End If

            Dim temp_oDocManager As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oDocManager, sClassName:="iPMBDocManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oDocManager = temp_oDocManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("error getting doc manager", Application.ProductName)
                oDocManager = Nothing
                Return result
            End If

            'm_lReturn = o.SetProcessModes(vTask:=PMEdit)


            oDocManager.PartyCnt = m_lPartyCnt

            oDocManager.InsuranceFileCnt = m_lInsuranceFileCnt

            oDocManager.ClaimCnt = m_lClaimCnt


            oDocManager.DocumentTemplateId = m_lDocumentTemplateId

            oDocManager.DocumentTypeId = m_lDocumentTypeId


            oDocManager.FileNumber = m_lFileNumber


            oDocManager.Client = m_sClient 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup


            m_lReturn = oDocManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("error start", Application.ProductName)
                oDocManager = Nothing
                Return result
            End If


            oDocManager.Dispose()
            oDocManager = Nothing
            m_bSpoolMessage = True

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to merge the document", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetBrowser
    '
    ' Description: sets the Browser control
    '
    ' ***************************************************************** '
    Public Function SetBrowser() As Integer

        Dim result As Integer = 0
        Dim sClient As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No solution no. 50

            sClient = m_sClient & "\Doc " & CStr(m_lFileNumber) & "." & m_sDocFileExtension

            m_sClientDocument = sClient

            uctPreviewDocControl1.Visible = True

            If File.Exists(sClient) Then
                uctPreviewDocControl1.Filename = sClient
                uctPreviewDocControl1.ProcessInterface()

                uctPreviewDocControl1.Visible = True
            Else
                uctPreviewDocControl1.Visible = False
            End If

            'WebBrowser1.Navigate(New Uri(sClient))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Browser control", vApp:=ACApp, vClass:=ACClass, vMethod:="SetBrowser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            'We already have all the data
            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetNext( _
            'vDocumentTemplateId:=m_lDocumentTemplateId, vCode:=m_sDocumentTemplateCode, _
            'vDescription:=m_sDocumentTemplateDescription, vDocumentTypeId:=m_lDocumentTypeId, _
            'vSlotNumber:=m_vSlotNumber, vRiskCodeId:=m_vRiskCodeId, vRiskGroupId:=m_vRiskGroupId)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retreive the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim bEditable As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

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

            ' Position Edit and New controls
            If Not cmdNavigate.Visible Then
                cmdEdit.Left = cmdNavigate.Left
            Else
                cmdEdit.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdNavigate.Left) + VB6.PixelsToTwipsX(cmdNavigate.Width) + 105)
            End If

            cmdClose.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdEdit.Left) + VB6.PixelsToTwipsX(cmdEdit.Width) + 105)

            'DJM 23/04/2002 : Added for merge button
            cmdMerge.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdClose.Left) + VB6.PixelsToTwipsX(cmdClose.Width) + 105)

            If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then
                cmdEdit.Visible = False
                cmdClose.Visible = False
                'DJM 23/04/2002 : Added for merge button
                cmdMerge.Visible = False
            End If

            'DC140503 -ISS3108 -start -added back as with 1.6.9
            'DJM 27/05/2002 : Allowed merge button to appear when adding a text file.
            'DJM 23/04/2002 : Added for merge button
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cmdMerge.Visible = False
            End If
            'DC140503 -end

            'DJM 25/02/2004 : Check if the default template is editable.
            m_lReturn = IsTemplateEditable(bEditable)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not bEditable Then
                cmdEdit.Enabled = False
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'Tomo240200
            '    m_lReturn = GetRegSettings(m_sServer, "S4B", "DP", "Server", "\\sforb\PM\Documents")
            m_lReturn = GetServer()
            '    m_lReturn = GetRegSettings(m_sClient, "S4B", "DP", "Client", "c:\Temp")
            m_lReturn = GetClient()

            cmdClose.Enabled = False

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
            ReDim m_ctlTabFirstLast(1, 0)

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

            'm_ctlTabFirstLast(ACControlStart, 0) = WebBrowser1
            'm_ctlTabFirstLast(ACControlEnd, 0) = WebBrowser1

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                    "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then

                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDelete
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: EditDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function EditDocument() As Integer
        'RWH(27/07/2000) Modified to launch word without OLE container
        'and display html document.
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        'Store current state of buttons
        m_bButtonNavigate = cmdNavigate.Enabled
        m_bButtonEdit = cmdEdit.Enabled
        m_bButtonClose = cmdClose.Enabled
        m_bButtonMerge = cmdMerge.Enabled
        m_bButtonOK = cmdOK.Enabled
        m_bButtonCancel = cmdCancel.Enabled
        m_lButtonHelp = cmdHelp.Enabled

        'Set all the buttons to disabled.
        cmdNavigate.Enabled = False
        cmdEdit.Enabled = False
        cmdClose.Enabled = False
        cmdMerge.Enabled = False
        cmdOK.Enabled = False
        cmdCancel.Enabled = False
        cmdHelp.Enabled = False
        If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
            If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = OpenDocument()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                lReturn = CType(LaunchOurDoc(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'Show the field manager
        If m_lMode = gSIRLibrary.ACNormalMode Then

            'DC101003 -PN7246 -check if macro not there
            Try

                'Run Field Manager Macro to launch floating Field Manager app.
                m_oWord.Run("Normal.PMFieldManager.ShowFieldManagerMacro")

                'DC101003 -PN7246 -trap problem with protected file

            Catch
            End Try





        End If

        m_oWord.WindowState = Word.WdWindowState.wdWindowStateMaximize

        'DN 01/02/02 - Force into PageView instead of WebPage
        m_oWord.ActiveWindow.View.Type = Word.WdViewType.wdPrintView

        'Set Word to visible last to ensure it is on top.
        m_oWord.Visible = True
        'To keep word on top
        Me.SendToBack()

        'Wait until the word instance we opened, is closed.
        Do While OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue
            Sleep(500) 'Do nothing for half a second
            Application.DoEvents()
        Loop

        Try
            iPMFunc.ForceForegroundWindow(Me.Handle.ToInt32())

        Catch
        End Try



        'Refresh the preview window - Use SetBrowser as it only continues once the document has been fully refreshed.
        SetBrowser()

        'WebBrowser1.Refresh()
        cmdNavigate.Enabled = m_bButtonNavigate
        cmdEdit.Enabled = m_bButtonEdit
        cmdClose.Enabled = m_bButtonClose
        cmdMerge.Enabled = m_bButtonMerge
        cmdOK.Enabled = m_bButtonOK
        cmdCancel.Enabled = m_bButtonCancel
        cmdHelp.Enabled = m_lButtonHelp

        Application.DoEvents()

        m_bIsDocChanged = True

        Return result

Err_EditDocument:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MailDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (MailDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function MailDocument() As Integer
    'RWH(01/08/2000) Modified to deal with html documents.
    '
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim sMsg As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
    'If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
    '            Set m_oDocument = m_oWord.Documents.Open(m_sClientDocument, _
    ''                                                    ConfirmConversions:=False)
    'm_lReturn = OpenDocument()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Else
    'lReturn = CType(LaunchOurDoc(), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    'End If
    '
    'm_oWord.WindowState = Word.WdWindowState.wdWindowStateMaximize
    '
    'Set Word to visible last to ensure it is on top.
    'm_oWord.Visible = True
    '
    'm_lReturn = SetDocumentVariables()
    'm_oWord.Run("Normal.PMDocumentManager.PMBEmailDocument")
    '
    'm_lReturn = ShutItDown()
    '
    'm_bSpoolMessage = False
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'Select Case Information.Err().Number
    'Case Else
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    'End Select
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function PrintDocument() As Integer

        Dim result As Integer = 0
        Dim lEventCnt As Integer = Nothing
        Dim lDocumentCnt As Integer = Nothing
        Dim vInsuranceFolderCnt As Integer = Nothing
        Dim vInsuranceFileCnt As Integer = Nothing
        Dim vClaimCnt As Integer = Nothing
        Dim vDocumentTypeId As Integer = Nothing
        Dim iOptionNumber As Integer = Nothing
        Dim sOptionValue As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    OpenDocument()
                Else
                    lReturn = CType(LaunchOurDoc(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Run Document Manager Macro to print document.
            m_oWord.Run("Normal.PMDocumentManager.PMBPrintDocument")

            lReturn = ShutItDown()

            Return result

            'We don't add it to FileMaster in this program, but let's leave the code, just in case...

            'Add it to FileMaster, returning the document cnt

            'First check if FileMaster is installed

            iOptionNumber = 10 ' possibly use a set of constants?


            m_lReturn = m_oBusiness.getOption(r_iOptionNumber:=iOptionNumber, v_sOptionValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If

            If sOptionValue.Trim() = "1" Then
                'It goes into FileMaster
            End If

            'Now create the event...

            vDocumentTypeId = PMBConst.PMBClientTextFile

            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = m_lInsuranceFolderCnt
            End If

            If m_lInsuranceFileCnt = 0 Then

                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = m_lInsuranceFileCnt
                vDocumentTypeId = PMBConst.PMBPolicyTextFile
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = m_lClaimCnt
                vDocumentTypeId = PMBConst.PMBClaimTextFile
            End If

            'Add the created event


            m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocumentCnt, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=vDocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDocument, v_dtEventDate:=DateTime.Today)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintDocumentSilent
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function PrintDocumentSilent() As Integer

        Dim result As Integer = 0
        Dim sOptionValue As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(03/08/2000) Replaced OLE control stuff.
            If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMFalse Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    '            Set m_oDocument = m_oWord.Documents.Open(m_sClientDocument, _
                    ''                                                    ConfirmConversions:=False)
                    m_lReturn = OpenDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    lReturn = CType(LaunchOurDoc(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Run Document Manager Macro to print document.
            m_oWord.Run("Normal.PMDocumentManager.PMBPrintDocumentSilent")

            lReturn = ShutItDown()

            'Print used to send to FileMaster, but not any more.  Should it still?

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocumentSilent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ArchiveDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ArchiveDocument() As Integer

        'Dim lEventCnt As Long
        Dim result As Integer = 0
        Dim vInsuranceFolderCnt As Integer
        Dim vInsuranceFileCnt As Integer
        Dim vClaimCnt As Integer
        Dim iOptionNumber As Integer
        Dim sOptionValue As String = ""
        Dim lDocNumber, lEventTypeId As Integer
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add it to FileMaster, returning the document cnt

            'First check if FileMaster is installed

            iOptionNumber = 10 ' possibly use a set of constants?

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sOptionValue.Trim() <> "1") Then
                MessageBox.Show("DocuMaster is not enabled", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDescription = Interaction.InputBox("Event description for document?", "Event Description", "Text File - " & m_sDocumentTemplateDesc)

            If sDescription.Trim() = "" Then
                sDescription = m_sDocumentTemplateDesc
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If sOptionValue.Trim() = "1" Then
                'It goes into FileMaster
                m_lReturn = UpdateFileMaster(lDocNumber:=lDocNumber)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If
            End If

            'Now create the event...

            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = m_lInsuranceFolderCnt
            End If

            If m_lInsuranceFileCnt = 0 Then

                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = m_lClaimCnt
            End If

            If m_lEventCnt = 0 Then
                lEventTypeId = PMBConst.PMBEventDocument
            Else
                lEventTypeId = PMBConst.PMBEventTransaction
            End If



            m_lReturn = m_oBusiness.CreateEvent(r_lEventCnt:=EventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=vInsuranceFolderCnt, v_vInsuranceFileCnt:=vInsuranceFileCnt, v_vClaimCnt:=vClaimCnt, v_vDocumentCnt:=lDocNumber, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DocumentTypeId, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=lEventTypeId, v_dtEventDate:=DateTime.Today, v_sDescription:="Raised Document - " & sDescription)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_bSpoolMessage = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SpoolDocument
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '           : 30/01/2001 Tinny - change it to public and add optional parameters
    '             for description and document to be spooled
    ' ***************************************************************** '
    Public Function SpoolDocument(Optional ByVal v_sDesc As String = "", Optional ByVal v_sDocName As String = "") As Integer

        Dim result As Integer = 0
        Dim vPartyCnt As Integer
        Dim vInsuranceFolderCnt As Integer
        Dim vInsuranceFileCnt As Integer
        Dim vClaimCnt, lTemp, lTemp2 As Integer
        Dim sServer, sTemp, sDescription As String
        Dim sSpoolDoc As String = String.Empty
        Dim sSpoolZip As String = String.Empty
        Dim iTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lPartyCnt = 0 Then

                vPartyCnt = Nothing
            Else
                vPartyCnt = m_lPartyCnt
            End If

            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = m_lInsuranceFolderCnt
            End If

            If m_lInsuranceFileCnt = 0 Then

                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = m_lInsuranceFileCnt
            End If

            If m_lClaimCnt = 0 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = m_lClaimCnt
            End If

            If v_sDesc <> "" Then
                sDescription = v_sDesc
            Else
                sDescription = Interaction.InputBox("What description for this item", Me.Text, "Text File")
            End If

            If sDescription.Trim() = "" Then
                sDescription = "Text File"
            End If

            If v_sDocName <> "" Then
                m_sClientDocument = v_sDocName
                sSpoolDoc = v_sDocName
            End If

            'TN20010202 - don't bother to set up document variables if we are spooling a report (ie Crystal generated)
            If m_lMode <> gSIRLibrary.ACSpoolReportMode Then
                'RWH(11/09/2000) Just put this line back in as I do all CTAF's stuff in 'SetDocVariables'
                'Check it with him before checking in.
                'Do these here in case it's mailed from the spooler
                m_lReturn = SetDocumentVariables()

                'RWH(12/01/2001) Save document as standard word doc to be spooled.
                sSpoolDoc = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "xml"

                m_oDocument.SaveAs(FileName:=sSpoolDoc, FileFormat:=11)
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_oDocument.Close()

                m_oDocument = Nothing

                'AJM 22/02/01 - ensure that we quit word after spooling document
                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

                '        m_lReturn = ShutItDown
                '        If (m_lReturn& <> PMTrue) Then
                '            SpoolDocument = PMFalse
                '            Exit Function
                '        End If
            End If

            '    ' CTAF 020800 - This was cut'n'paste from MailDocument
            '    If Not OLE1.AppIsRunning Then
            '
            '        'This one causes a Dr Watson error...
            '        'OLE1.DoVerb vbOLEInPlaceActivate
            '        OLE1.DoVerb vbOLEShow
            '
            '        'CT 30/08/00 Added Doevents to produce a pause before opening Word document. Previously the
            '        'program was trying to set word variables before the document had finished opening
            '        'thus causing error. I know that DoEvents is not an ideal solution but the use of a timer resulted in its own problems
            '        'and  doevents actually works
            '
            '        iCount = DoEvents
            '
            '        'Mail it
            '        Select Case m_sClass
            '        Case "Word.Document.8", "Word.Document.9"
            '            'Do these here in case it's mailed from the spooler
            '            m_lReturn = SetDocumentVariables()
            '        End Select
            '
            '        m_lReturn = ShutItDown
            '        If (m_lReturn& <> PMTrue) Then
            '            SpoolDocument = PMFalse
            '            Exit Function
            '        End If
            '
            '    End If

            If m_oDocSpooler Is Nothing Then
                Dim temp_m_oDocSpooler As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocSpooler, "bSIRDocSpooler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oDocSpooler = temp_m_oDocSpooler

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Document Spooler object", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDocSpooler.DirectAdd(vDocumentSpoolerId:=m_lSpoolNumber, vDocumentTypeId:=m_lDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=sDescription, vSpoolLevelInd:=1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lTemp = m_lSpoolNumber \ 1000

            lTemp2 = m_lSpoolNumber - (lTemp * 1000)

            'Build it up and make sure all sub-directories are there...
            sServer = m_sServer & "\Spooled Documents"

            sTemp = FileSystem.Dir(sServer, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sServer)
            End If

            sServer = m_sServer & "\Spooled Documents" & "\Company " & CStr(g_iSourceID)

            sTemp = FileSystem.Dir(sServer, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sServer)
            End If

            sServer = m_sServer & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000")

            sTemp = FileSystem.Dir(sServer, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sServer)
            End If

            sServer = m_sServer & "\Spooled Documents" & "\Company " & CStr(g_iSourceID) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"


            ''''''''''''''''''OLD''''''''''''''''''''''''''''''''
            '    'RWH(04/09/2000) - RSAIB Process 108.
            '    'Use new absolute directory to zip & unzip files.
            '    CopyFilesToZipTemp
            '
            ''    sClient = m_sClient & "\Doc " & m_lOldDocumentTemplateId & ".zip"
            '    sClient = m_sZIP_DIRECTORY & "\Doc " & m_lOldDocumentTemplateId & ".zip"
            '
            '    m_lReturn = Zip(sClient)
            '
            '    FileCopy sClient, sServer
            '
            '    'Delete the local copy
            '    Kill sClient
            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''NEW''''''''''''''''''''''''''''''''''''''''''
            'RWH(12/01/2001) Store spooled doc as standard word doc instead of html.
            sSpoolZip = m_sClientDocument.Substring(0, m_sClientDocument.Length - 3) & "zip"

            iTemp = g_oZipper.ZipFile(sFileIn:=sSpoolDoc, sFileOut:=sSpoolZip)

            If Not iTemp Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            File.Copy(sSpoolZip, sServer)

            Dim sTmp As String = ""

            'Tidy up directory.
            sTmp = FileSystem.Dir(m_sClient & "\", FileAttribute.Normal)
            Do While sTmp <> ""
                Kill(m_sClient & "\" & sTmp)
                sTmp = FileSystem.Dir()
            Loop
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' History: 03/09/1999 Tomo - Created.
    '          03/08/2000 RWH  - Removed use of OLE container control
    '                            to facilitate use of HTML documents.
    ' ***************************************************************** '

    ' CJB 080903
    ' NOTE: This function is not used at present since it is only called from
    '       ArchiveDocument which is not called itself and is Private. If this
    '       code is ever used then sPartyName and sInsuranceFileRef need values
    '       passing in the m_oSIRDOCAPI.AddDocument call to ensure blank
    '       folders don't get created in DME.

    Private Function UpdateFileMaster(ByRef lDocNumber As Integer) As Integer
        Dim result As Integer = 0
        'Changes as per VB code
        'Dim ReadRegistry(,,) As Object
        Dim sOptionValue As String = ""
        Dim sKeywords() As String
        Dim sClient, sDocType, sPageType, sDocName, sTemp As String
        Dim sServer As String = String.Empty
        Dim sErrorMessage As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSIRDOCAPI Is Nothing Then
                Dim temp_m_oSIRDOCAPI As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oSIRDOCAPI, "bSIRDOCAPI.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oSIRDOCAPI = temp_m_oSIRDOCAPI

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            'It's not used, but we need to define it anyway...
            ReDim sKeywords(0)

            sClient = m_sClient & "\Doc " & CStr(IIf(m_lDocumentTemplateId = 0, m_lFileNumber, m_lDocumentTemplateId)) & ".xml"

            m_lReturn = SetDocumentVariables()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDocument.SaveAs(FileName:=sClient, FileFormat:=11)
            m_oDocument.Close()

            m_oDocument = Nothing

            m_oWord.Application.Quit()
            m_oWord = Nothing

            sDocType = "D" 'Letter - maybe configurable?
            sPageType = "DOC" 'Rich Text Format - maybe configurable?
            ' m_lReturn = m_oBusiness.GetDescription(lDocumentTemplateId:=m_lDocumentTemplateId, _
            'sDocumentTemplateDescription:=m_sDocumentTemplateDescription)

            ' Get the location of the export directory
            ' sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\PM\SiriusSolutions\Client", "PrntFileDir"))
            sTemp = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "PrntFileDir"))
            If sTemp = "Not Found" Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to find the registry entry", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the PrntFileDir directory location")

                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' create a temp directory
            sTemp = sTemp.Trim()
            If sTemp.EndsWith("\") Then
                sServer = sTemp & "DocArchiveTemp"
            Else
                sServer = sTemp & "\DocArchiveTemp"
            End If

            ' create a user specific directory
            sServer = sServer & "\" & g_oObjectManager.UserName.Trim()

            m_lReturn = CreateFolderTree(sServer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create Directory ( " & sServer & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            sServer = sServer & "\Doc " & CStr(m_lDocumentTemplateId) & ".xml"

            'Copy the document to the server
            m_lReturn = bPMDocFunctions.CopyFile(sClient, sServer, True, False, sErrorMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Client To Server." & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Source File      : " & sClient & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Destination File : " & sServer & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'It's not used, but we need to define it anyway...
            ReDim sKeywords(0)

            sDocName = m_sDocumentTemplateDesc 'Need to get this from the document type table...

            ' RFC25/10/01 - Changed lInsuranceFileID to FolderID to work with new version of bSIRDocAPI

            m_lReturn = m_oSIRDOCAPI.AddDocument(lPartyId:=m_lPartyCnt, sPartyName:="", lInsuranceFolderId:=m_lInsuranceFolderCnt, sInsuranceFileRef:="", lClaimId:=0, sClaimRef:="", lFSAComplaintFolderCnt:=0, sFSAComplaintReference:="", sDocType:=sDocType, sPageType:=sPageType, sDocName:=sDocName, sFilename:=sServer, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=lDocNumber)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFileMaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OurInstanceOfWordIsRunning
    '
    ' Description: Checks to see if the instance of word we created to
    '               edit or print a document is still running.
    '
    ' History: 03/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OurInstanceOfWordIsRunning() As Integer

        Dim sTest As String = ""

        Try

            m_lReturn = bPMDocFunctions.IsWindow(m_lWordHwnd)


            If m_lReturn = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try




        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    '
    ' Name: OurDocIsRunning
    '
    ' Description: Checks to see if our document is still open. Does this by
    '               trying to access ActiveDocument property of module level
    '               Word object. If an error occurs, we assume document has
    '               already been closed down.
    '
    ' History: 01/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OurDocIsRunning() As Integer

        Dim result As Integer = 0
        Dim sTest As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                For iCount As Integer = 1 To m_oWord.Documents.Count
                    sTest = m_oWord.Documents.Item(iCount).FullName
                    If String.Compare(sTest.Trim(), m_sClientDocument.Trim()) = 0 Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Exit For
                    End If
                Next iCount
            End If

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function ShutItDown() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'is our word session still open
            m_lReturn = bPMDocFunctions.IsWindow(m_lWordHwnd)

            'yeap its still open
            If m_lReturn <> 0 Then

                If OurDocIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_oWord.Documents.Close(SaveChanges:=True)
                End If

                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to shut it down", vApp:=ACApp, vClass:=ACClass, vMethod:="ShutItDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPath
    '
    ' Description:
    '
    ' History: 24/08/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetPath(ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim lTemp, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sServer.Trim() = "" Then
                m_lReturn = GetServer()
            End If

            If m_lFileNumber <> 0 Then
                lTemp = m_lFileNumber \ 500

                lTemp2 = m_lFileNumber - (lTemp * 500)

                sTemp = "Client Text Files"

                If m_lInsuranceFileCnt > 0 Then
                    sTemp = "Policy Text Files"
                End If

                If m_lClaimCnt > 0 Then
                    sTemp = "Claim Text Files"
                End If

                'DN 01/02/02 - Use correct file number
                m_lOrigFileNumber = m_lFileNumber
                m_lFileNumber = lTemp2

                'CT 27/11/00 take into account that it may not be the global source but whatever was passed in
                '        sPath = m_sServer _
                ''                & "\" & sTemp _
                ''                & "\Company " & g_iSourceID _
                ''                & "\Slot " & m_lSlotNumber _
                ''                & "\" & Format(lTemp, "000") _
                ''                & "\" & Format(lTemp2, "000") & ".zip"


                sPath = m_sServer & "\" & sTemp & "\Slot " & CStr(m_lSlotNumber) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

                Return result
            End If

            m_lReturn = GetDocument()

            If m_lDocumentTemplateId <> 0 Then
                sPath = m_sServer & "\Type " & CStr(m_lDocumentTypeId) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"
                Return result
            End If

            sPath = m_sServer & "\blankxml.zip"

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAndCreatePath
    '
    ' Description:
    '
    ' History: 24/08/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetAndCreatePath(ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim sTemp, sText As String
        Dim lTemp, lTemp2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lOrigFileNumber = 0 Then
                m_lOrigFileNumber = m_lFileNumber
            End If

            lTemp = m_lOrigFileNumber \ 500

            lTemp2 = m_lOrigFileNumber - (lTemp * 500)

            sText = "Client Text Files"

            If m_lInsuranceFileCnt > 0 Then
                sText = "Policy Text Files"
            End If

            If m_lClaimCnt > 0 Then
                sText = "Claim Text Files"
            End If

            sPath = m_sServer

            'Make sure the directory's there
            sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = sPath & "\" & sText

            'Make sure the directory's there
            sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sPath)
            End If

            'CT 27/11/00 change path to use correct one for branch rather than default to global
            '    sPath = sPath _
            ''            & "\Company " & g_iSourceID

            '    sPath = sPath _
            ''        & "\Company " & SourceId
            'Make sure the directory's there
            sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = sPath & "\Slot " & CStr(m_lSlotNumber)

            'Make sure the directory's there
            sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = sPath & "\" & StringsHelper.Format(lTemp, "000")

            'Make sure the directory's there
            sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = sPath & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAndCreatePath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAndCreatePath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDocument
    '
    ' Description:
    '
    ' History: 24/08/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocument() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            result = m_oBusiness.GetDocument(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimCnt:=m_lClaimCnt, v_lRiskCodeId:=m_lRiskCodeId, v_lRiskGroupId:=m_lRiskGroupId, v_lSlotNumber:=m_lSlotNumber, v_lSourceID:=m_lSourceId, r_lDocumentTemplateId:=m_lDocumentTemplateId, r_lDocumentTypeId:=m_lDocumentTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClient
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '          MKW281003 PN7287 1.8.5 to 1.8.6 Catchup
    ' ***************************************************************** '
    Private Function GetClient() As Integer

        'Dim sClient As String

        'Dim eRegSettingRoot As PMERegSettingRoot
        'Dim eRegSettingLevel As PMERegSettingLevel
        'Dim eProductFamily As PMEProductFamily

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sClient.Trim() > "" Then
                Return result
            End If

            m_lReturn = GetClientDirectory(m_sClient, True)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_bUniqueClientDirNeedsDeleting = True
                Return result
            End If

            m_bUniqueClientDirNeedsDeleting = False

            '    eRegSettingRoot = pmeRSRLocalMachine
            '    eProductFamily = pmePFSiriusSolutions
            '    eRegSettingLevel = pmeRSLClient
            '
            '    sClient = ""
            '
            '    m_lReturn& = GetPMRegSetting( _
            'v_lPMERegSettingRoot:=eRegSettingRoot, _
            'v_lPMEProductFamily:=eProductFamily, _
            'v_lPMERegSettingLevel:=eRegSettingLevel, _
            'v_sSettingName:="DocClient", _
            'r_sSettingValue:=sClient)

            '    If (m_lReturn& <> PMTrue) Then
            '        LogMessage _
            'iType:=PMLogOnError, _
            'sMsg:="Unable to get Client from Registry.", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="GetClient", _
            'vErrNo:=Err.Number, _
            'vErrDesc:=Err.Description
            '        GetClient = PMFalse
            '        Exit Function
            '    Else
            ''Tomo290200
            ''For citrix, when everyone has the same client PC.
            ''        m_sClient = sClient
            '        m_sClient = sClient & "\" & Trim$(g_oObjectManager.UserName)
            '    End If
            '
            '    Exit Function

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sServer.Trim() > "" Then
                Return result
            End If

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            sServer = ""

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sServer = sServer
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenDocument
    '
    ' Description:
    '
    ' History: 28/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OpenDocument() As Integer
        'Dim bConfirmConversions As Boolean

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    bConfirmConversions = Options.ConfirmConversions
            '    DoEvents

            m_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)
            Application.DoEvents()

            '    Options.ConfirmConversions = bConfirmConversions
            '    DoEvents

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LaunchOurDoc
    '
    ' Description:  Runs word and sets required document as current.
    '
    ' History: 01/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function LaunchOurDoc() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Launch Word.
            m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)

            'Open current document.
            m_lReturn = OpenDocument()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'For Debug.
            m_oWord.Visible = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LaunchOurDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchOurDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (End)

    ' ***************************************************************** '
    '
    ' Name: SetDocumentVariables
    '
    ' Description:
    '
    ' History: 06/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function SetDocumentVariables() As Integer

        Dim result As Integer = 0
        Dim oDocument As Word.Document
        Dim aVar As Word.Variable
        Dim lCompanyIdIndex, lPartyCntIndex, lInsuranceFolderCntIndex, lInsuranceFileCntIndex, lClaimCntIndex, lDocumentTypeIdIndex, lDocumentTypeDescriptionIndex, lFormatIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(03/08/2000) Replaced OLE container stuff.
            '    Set oDocument = OLE1.object.Application.ActiveDocument
            If OurDocIsRunning() <> gPMConstants.PMEReturnCode.PMTrue Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    '            Set m_oDocument = m_oWord.Documents.Open(m_sClientDocument, _
                    ''                                                    ConfirmConversions:=False)
                    m_lReturn = OpenDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    '            m_lReturn = LaunchOurDoc()
                    m_oWord = New Word.Application()
                    '            Set m_oDocument = m_oWord.Documents.Open(m_sClientDocument, _
                    ''                                                    ConfirmConversions:=False)
                    m_lReturn = OpenDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If
            oDocument = m_oDocument


            'Get any already stored values - there shouldn't be any, but if there are and we
            'don't check we'll get an error when assigning them
            For Each aVar2 As Word.Variable In oDocument.Variables
                aVar = aVar2
                Select Case aVar.Name
                    Case "CompanyId"
                        lCompanyIdIndex = aVar.Index
                    Case "PartyCnt"
                        lPartyCntIndex = aVar.Index
                    Case "InsuranceFolderCnt"
                        lInsuranceFolderCntIndex = aVar.Index
                    Case "InsuranceFileCnt"
                        lInsuranceFileCntIndex = aVar.Index
                    Case "ClaimCnt"
                        lClaimCntIndex = aVar.Index
                    Case "DocumentTypeId"
                        lDocumentTypeIdIndex = aVar.Index
                    Case "DocumentTypeDescription"
                        lDocumentTypeDescriptionIndex = aVar.Index
                    Case "FMFormat"
                        lFormatIndex = aVar.Index
                End Select
            Next aVar2

            aVar = Nothing

            If lCompanyIdIndex = 0 Then
                oDocument.Variables.Add(Name:="CompanyId", Value:=g_iSourceID)
            Else
                oDocument.Variables.Item(lCompanyIdIndex).Value = CStr(g_iSourceID)
            End If

            If lPartyCntIndex = 0 Then
                oDocument.Variables.Add(Name:="PartyCnt", Value:=m_lPartyCnt)
            Else
                oDocument.Variables.Item(lPartyCntIndex).Value = CStr(m_lPartyCnt)
            End If

            If m_lInsuranceFolderCnt <> 0 Then
                If lInsuranceFolderCntIndex = 0 Then
                    oDocument.Variables.Add(Name:="InsuranceFolderCnt", Value:=m_lInsuranceFolderCnt)
                Else
                    oDocument.Variables.Item(lInsuranceFolderCntIndex).Value = CStr(m_lInsuranceFolderCnt)
                End If
            End If

            If m_lInsuranceFileCnt <> 0 Then
                If lInsuranceFileCntIndex = 0 Then
                    oDocument.Variables.Add(Name:="InsuranceFileCnt", Value:=m_lInsuranceFileCnt)
                Else
                    oDocument.Variables.Item(lInsuranceFileCntIndex).Value = CStr(m_lInsuranceFileCnt)
                End If
            End If

            If m_lClaimCnt <> 0 Then
                If lClaimCntIndex = 0 Then
                    oDocument.Variables.Add(Name:="ClaimCnt", Value:=m_lClaimCnt)
                Else
                    oDocument.Variables.Item(lClaimCntIndex).Value = CStr(m_lClaimCnt)
                End If
            End If

            If lDocumentTypeIdIndex = 0 Then
                oDocument.Variables.Add(Name:="DocumentTypeId", Value:=m_lDocumentTypeId)
            Else
                oDocument.Variables.Item(lDocumentTypeIdIndex).Value = CStr(m_lDocumentTypeId)
            End If

            If lDocumentTypeDescriptionIndex = 0 Then
                oDocument.Variables.Add(Name:="DocumentTypeDescription", Value:=m_sDocumentTemplateDescription)
            Else
                oDocument.Variables.Item(lDocumentTypeDescriptionIndex).Value = m_sDocumentTemplateDescription
            End If

            If lFormatIndex = 0 Then
                oDocument.Variables.Add(Name:="FMFormat", Value:="RTF")
            Else
                oDocument.Variables.Item(lFormatIndex).Value = "RTF"
            End If

            oDocument = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDocumentVariables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDocumentVariables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Events (Begin)

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim msgReply As DialogResult

        ' Click event of the Cancel button.

        Try

            If m_bIsDocChanged Then
                m_lReturn = MergeDocument()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            If m_lMode = gSIRLibrary.ACMergeMode Then
                If m_bSpoolMessage Then
                    msgReply = MessageBox.Show("Do you wish to spool this document", Me.Text, MessageBoxButtons.YesNoCancel)

                    Select Case msgReply
                        Case System.Windows.Forms.DialogResult.Yes
                            m_lReturn = SpoolDocument()
                        Case System.Windows.Forms.DialogResult.No
                        Case System.Windows.Forms.DialogResult.Cancel
                            Exit Sub
                    End Select
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            If m_bIsDocChanged Then
                msgReply = MessageBox.Show("Do You Wish To Archive This Document", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If msgReply = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = ArchiveDocument()
                End If
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'KN (CMG) Start 171002
                If m_bTempDir Then
                    m_lReturn = CopyTempDirToClient()
                End If
                'KN (CMG) End 171002
                m_lReturn = DeleteClient()

                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        m_lReturn = ShutItDown()

        cmdClose.Enabled = False

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If m_bTempDir Then
            m_lReturn = CopyTempDirToClient()
        End If

        m_lReturn = EditDocument()

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    'DJM 23/04/2002 : Added for merge button
    Private Sub cmdMerge_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMerge.Click
        'KN (CMG) Start 171002
        Dim sClientTempDir, sClientFileDest, sClientFileSource, sTemp As String
        Dim FSO As Object 'Microsoft Scripting Runtime Reference

        'KN (CMG) end 171002


        Try

            'KN (CMG) Start 171002 - Copy files to back up directory
            FSO = New Object()

            'DJM 06/01/2004 : Also back up pre-merge document if slot 11 policy text file (market presentation).
            'DJM 19/03/2003 : Only back up pre-merge document if slot 1 policy text file (risk register).

            If ((m_lDocumentTemplateId = 0 And m_lDocumentTypeId = 0) Or (m_lFileNumber <> 0)) And ((m_lSlotNumber = 1 Or m_lSlotNumber = 11) And m_lInsuranceFileCnt > 0 And (m_lClaimCnt = 0 Or Convert.IsDBNull(m_lClaimCnt) Or IsNothing(m_lClaimCnt))) Then

                sClientFileSource = "Doc " & m_lFileNumber & "." & m_sDocFileExtension
                sClientFileDest = "Doc " & m_lFileNumber & "." & m_sDocFileExtension & "temp"
                sClientTempDir = m_sClient & "\" & sClientFileDest

                'Make sure the directory's there
                sTemp = FileSystem.Dir(sClientTempDir, FileAttribute.Directory)
                If sTemp = "" Then
                    m_bTempDir = True
                    Directory.CreateDirectory(sClientTempDir)
                    File.Copy(m_sClient & "\" & sClientFileSource, sClientTempDir & "\" & sClientFileSource)

                End If

            End If

            'KN (CMG) End 171002

            m_lReturn = MergeDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = SetBrowser()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            FSO = Nothing 'KN (CMG) 171002
            m_bIsDocChanged = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to merge document", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMergeClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            FSO = Nothing 'KN (CMG) 171002

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim msgReply As DialogResult

        ' Click event of the OK button.

        Try

            If m_bIsDocChanged Then
                m_lReturn = MergeDocument()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If


            If m_lMode = gSIRLibrary.ACMergeMode Then
                If m_bSpoolMessage Then
                    msgReply = MessageBox.Show("Do you wish to spool this document", Me.Text, MessageBoxButtons.YesNoCancel)

                    Select Case msgReply
                        Case System.Windows.Forms.DialogResult.Yes
                            m_lReturn = SpoolDocument()
                        Case System.Windows.Forms.DialogResult.No
                        Case System.Windows.Forms.DialogResult.Cancel
                            Exit Sub
                    End Select
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            If m_bIsDocChanged Then
                msgReply = MessageBox.Show("Do You Wish To Archive This Document", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If msgReply = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = ArchiveDocument()
                End If
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If Task = gPMConstants.PMEComponentAction.PMDelete Then
                    m_lReturn = DeleteServer()
                    m_lReturn = DeleteClient()
                Else
                    'KN (CMG) Start 171002
                    If m_bTempDir Then
                        m_lReturn = CopyTempDirToClient()
                    End If
                    'KN (CMG) End 171002
                    m_lReturn = CopyClientToServer()
                End If

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTextFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBTextFile.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'KN (CMG) Start 171002
            m_bTempDir = False
            'KN (CMG) End 171002

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub


    Public Sub frmInterfaceLoad()

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

            'Initialise the user control.
            uctPreviewDocControl1.Initialise()

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = m_lReturn 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup

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
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                ShutItDown()
            End If

            If Not (m_oSIRDOCAPI Is Nothing) Then
                ' Terminate the API object

                m_oSIRDOCAPI.Dispose()
                ' Destroy the instance of the API object
                ' from memory.
                m_oSIRDOCAPI = Nothing
            End If

            If Not (m_oDocSpooler Is Nothing) Then
                ' Temrminate the spooler object

                m_oDocSpooler.Dispose()
                ' Destroy the instance of the spooler object
                ' from memory.
                m_oDocSpooler = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup START
            If m_bUniqueClientDirNeedsDeleting Then
                m_lReturn = DelDirectory(m_sClient)
                m_sClient = ""
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup END

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

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch 
        '
        '
        '
        'Exit Sub
        'End Try


    End Sub


    'Private Sub WebBrowser1_DocumentCompleted(ByVal eventSender As Object, ByVal eventArgs As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
    '    'Developer Guide No.176
    '    Dim URL As String = eventArgs.Url.ToString()

    '    WebBrowser1.Stop()

    'End Sub

    Private Function SetZipDirectory() As Integer
        'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup

        Dim result As Integer = 0
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sZIP_DIRECTORY <> "" Then
                Return result
            End If

            ' SET - ISS6605 - get the DocZipPMDir path
            m_lReturn = GetZipDirectory(r_sZipDirectory:=sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to find DocZipTemp directory", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            m_sZIP_DIRECTORY = sTemp

            'DC140503 -ISS3108 -start -added from 1.6.9
            'Get the PMDIR path
            'DJM 13/02/2003 : First check for full path in registry.
            'sTemp = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\PM", "PMDIR")
            'sTemp = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\PM\SIRIUSSOLUTIONS\CLIENT", "DOCZIPTEMP")
            'DC140503 -end

            '    'TF120803 - Use the reg setting from the build (!)
            '    sTemp = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\PM\SIRIUSSOLUTIONS\CLIENT", "DocZipPMDir")

            '    'Make sure we have an install path
            '    If ((sTemp = "Not Found") Or (Trim(sTemp) = "")) Then
            '        'TF120803 - Now use the default PM directory
            '        sTemp = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\PM", "PMDIR")
            '        'If still empty, something seriously wrong
            '        If ((sTemp = "Not Found") Or (Trim(sTemp) = "")) Then
            '            'sTemp = "C:\Program Files"
            '            SetZipDirectory = PMFalse
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Unable to obtain registry setting for ZIP directory.", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="SetZipDirectory"
            '
            '            Exit Function
            '        Else
            '            'Set to PM folder + default + username
            '            m_sZIP_DIRECTORY = sTemp & "\PM\DocZipTemp\" & g_sUserName 'MKW150703 PN5359
            '        End If
            '    Else
            '        'Is OK, just add username
            '        m_sZIP_DIRECTORY = sTemp & "\" & g_sUserName
            '    End If
            '
            '
            '    sTemp = Dir(m_sZIP_DIRECTORY, vbDirectory)
            '
            '    If (sTemp = "") Then
            '        MkDir m_sZIP_DIRECTORY
            '
            '        'MKW150703 PN5359 START
            '        ' did we succeed...?
            '        sTemp = Dir(m_sZIP_DIRECTORY, vbDirectory)
            '
            '        If (sTemp = "") Then
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="SetZipDirectory Failed", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="SetZipDirectory", _
            ''                vErrNo:=0, _
            ''                vErrDesc:="Unable to create the directory (" & m_sZIP_DIRECTORY & _
            ''                    ")"
            '
            '            SetZipDirectory = pmerror
            '            Exit Function
            '        End If
            '        'MKW150703 PN5359 END
            '
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckFileType
    '
    ' Description: Opens file downloaded from server & checks first 2
    '               bytes to ensure this is a standard .doc document
    '               This is to see whether it is necessary to carry
    '               out a conversion.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckFileTypeIsDoc() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim lFileCount, lFileNum As Integer
        Dim sLine As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sFile = FileSystem.Dir(m_sClient & "\*.*", FileAttribute.Normal)
            If sFile <> "" Then
                lFileCount = 1
                Do While (FileSystem.Dir() <> "")
                    lFileCount += 1
                Loop
            End If

            If lFileCount > 1 Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Else
                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)

                sLine = FileSystem.InputString(lFileNum, 2)


                Select Case sLine.ToUpper()
                    Case "<H"
                        'Do Nothing

                    Case Else
                        result = gPMConstants.PMEReturnCode.PMTrue

                End Select

            End If
            FileSystem.FileClose(lFileNum)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileTypeIsDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Open a document, resolve the fields, and return the
    ' document object
    Private Function ConvertDocument(ByRef oDocument As Word.Document, ByVal lFileNo As Integer) As Integer

        Dim result As Integer = 0
        Dim oBookmark As Word.Bookmark
        Dim sFieldCode, sNewMergeField As String
        Dim iSep As Integer
        Dim sFieldType As String = String.Empty
        Dim sFieldName As String = String.Empty
        Dim sQuestion As String = String.Empty

        'DJM 30/08/2002 : When converting document, put markers in properly.
        Const c_sFieldStartMarker As String = "<@"
        Const c_sFieldEndMarker As String = "@>"

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo Err_ConvertDocument
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sFileName As String = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".xml"

        ' Open the chosen template document
        oDocument = m_oWord.Documents.Open(sFileName)

        ' Get the active window
        Dim oActiveWindow As Word.Window = oDocument.ActiveWindow

        ' Get the bookmarks collection
        Dim oBookmarks As Word.Bookmarks = oDocument.Bookmarks

        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        ' Reget the bookmarks collection
        oBookmarks = oDocument.Bookmarks

        If oBookmarks.Count = 0 Then
            oBookmarks = Nothing
            oActiveWindow = Nothing
            Return result
        End If

        ' Load the bookmarks into an array
        For Each oBookmark2 As Word.Bookmark In oBookmarks
            oBookmark = oBookmark2

            ' Get the field code for the bookmark
            sFieldCode = oBookmark.Name

            ' Determine the field type
            iSep = (sFieldCode.IndexOf("_"c) + 1)
            If iSep > 0 Then
                sFieldType = sFieldCode.Substring(0, iSep - 1)
            End If

            ' Select the bookmark so it can be overwritten.
            oBookmark.Select()


            Select Case sFieldType
                Case DbTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    ' Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & DbTag & Separator & sFieldName & c_sFieldEndMarker

                    oActiveWindow.Selection.Text = sNewMergeField

                Case QuestionTag

                    sQuestion = oActiveWindow.Selection.Text

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & QuestionTag & Separator & sQuestion & c_sFieldEndMarker

                    oActiveWindow.Selection.Text = sNewMergeField

                Case LoopTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    ' Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & LoopTag & Separator & sFieldName & c_sFieldEndMarker

                    oActiveWindow.Selection.Text = sNewMergeField


                Case EndLoopTag
                    ' extract the field name
                    sFieldName = sFieldCode.Substring(sFieldCode.Length - (sFieldCode.Length - iSep))

                    ' Strip off the file name at the beginning
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(iSep)
                    End If

                    ' Strip off the id character at the end
                    iSep = (sFieldName.IndexOf("_"c) + 1)
                    If iSep > 0 Then
                        sFieldName = sFieldName.Substring(0, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = c_sFieldStartMarker & EndLoopTag & Separator & sFieldName & c_sFieldEndMarker

                    oActiveWindow.Selection.Text = sNewMergeField


                Case Else
                    'oBookmark.Range.Text = "Invalid Bookmark"

            End Select

        Next oBookmark2

        'Update the fields
        Word_Global_definst.ActiveDocument.Fields.Update()

        ' Return the document


        Return result

Err_ConvertDocument:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckFileName
    '
    ' Description: Templates were formerly created with 'Doc 0' name
    '               and not converted to the correct number until
    '               they were first edited or merged. We need to check
    '               that the file we have unzipped has the correctname.
    '               If not, then we must convert it.
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckFileName(ByRef sPath As String, ByVal lCorrectFileNumber As Integer) As Integer
        Dim result As Integer = 0
        Dim sFileName, sCorrectedFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get current file name.
            sFileName = FileSystem.Dir(sPath & "\*.*", FileAttribute.Normal)
            If sFileName <> "" Then
                sCorrectedFile = "Doc " & lCorrectFileNumber & ".xml"
                FileSystem.Rename(sPath & "\" & sFileName, sPath & "\" & sCorrectedFile)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveDocumentAsHTML
    '
    ' Description:
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'Public Function SaveDocumentAsHTML(ByRef oTemplate As Word.Document, ByVal lFileNo As Integer) As Integer
    '    Dim result As Integer = 0
    '    Dim sFileName As String = ""
    '    Dim iFormat As Integer

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        sFileName = m_sClient & "\" & "Doc " & CStr(lFileNo) & ".htm"

    '        oTemplate.SaveAs(sFileName, Word.WdSaveFormat.wdFormatHTML)
    '        oTemplate.Close()


    '        Kill(m_sClient & "\" & "*.Doc")

    '        Return result

    '    Catch excep As System.Exception



    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error Message
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveDocumentAsHTML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocumentAsHTML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Return result

    '    End Try
    'End Function

    ' **********************************************************
    ' DJM 17/07/2002
    '
    ' Function to check if the word object is valid (ie, has it Quit or not)
    '
    ' **********************************************************
    'UPGRADE_NOTE: (7001) The following declaration (IsWordValid) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsWordValid() As Boolean
    '
    'Dim result As Boolean = False
    'Dim sTemp As String = ""
    '
    'Try 
    '
    'result = True
    '
    ' Try and get the name of the object
    'sTemp = m_oWord.Name
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    'Return False
    'End Try
    '
    'End Function

    ' ***************************************************************** '
    ' Name: CopyTemplateClientToServer
    '
    ' Description: copies the template from the client to the server
    '
    ' ***************************************************************** '
    Public Function CopyTemplateClientToServer(ByRef lFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sServer, sClient, sTemp As String
        Dim sParentFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            CopyTemplateFilesToZipTemp()

            'Make sure the directory's there
            sTemp = FileSystem.Dir(m_sServer, FileAttribute.Directory)
            If sTemp = "" Then
                Directory.CreateDirectory(m_sServer)
            End If

            sServer = m_sServer & "\Type " & CStr(m_lDocumentTypeId) & "\Doc " & CStr(m_lDocumentTemplateId) & ".zip"

            sClient = m_sZIP_DIRECTORY & "\Doc " & CStr(lFileNumber) & ".zip"

            m_lReturn = Zip(sClient)

            File.Copy(sClient, sServer)

            'Delete the local copy
            Kill(sClient)

            sParentFile = m_sZIP_DIRECTORY & "\Doc " & CStr(lFileNumber)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTemplateClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyTemplateFilesToZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyTemplateFilesToZipTemp() As Integer
        Dim result As Integer = 0
        Dim sClient, sParentFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Copy files to ZipTemp.
            sClient = FileSystem.Dir(m_sClient & "\*." & m_sDocFileExtension, FileAttribute.Normal)
            If sClient <> "" Then

                'Copy parent file to ZipTemp.
                File.Copy(m_sClient & "\" & sClient, m_sZIP_DIRECTORY & "\" & sClient)

                'Check for dependencies and copy them to the temp zip directory if they exist.
                sParentFile = m_sClient & "\" & sClient
                sParentFile = sParentFile.Substring(0, sParentFile.Length - 4)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyTemplateFilesToZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTemplateFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Private Function CheckFileExistsAndAllowLinkDeletion(sServer As String) As Long
    '' ***************************************************************** '
    ''
    '' Name: CheckFileExistsAndAllowLinkDeletion
    ''
    '' Description:
    ''
    '' History: DJM 28/01/2003 Created
    ''
    '' ***************************************************************** '
    'Dim oDocTemplate As Object
    '
    '    On Error GoTo Err_CheckFileExistsAndAllowLinkDeletion
    '
    '    CheckFileExistsAndAllowLinkDeletion = PMTrue
    '
    '    If Dir(sServer) = "" Then
    '        'File does not exist
    '        If MsgBox("The document requested does not exist. " & _
    ''            " Would you like to remove the database reference to this document?" _
    ''            , vbExclamation + vbYesNo, "Document Manager") = vbYes Then
    '
    '
    '            If (m_lFileNumber <> 0) Then
    '
    '                m_iTask = PMDelete
    '
    '                m_lReturn = InterfaceToBusiness()
    '
    '                m_lReturn = m_oBusiness.Update()
    '
    '                ' Check for errors.
    '                If (m_lReturn& <> PMTrue) Then
    '                   ' Failed to update the details
    '                   CheckFileExistsAndAllowLinkDeletion = PMFalse
    '
    '                   ' Log Error.
    '                   LogMessage _
    ''                       iType:=PMLogError, _
    ''                       sMsg:="Failed to update the details", _
    ''                       vApp:=ACApp, _
    ''                       vClass:=ACClass, _
    ''                       vMethod:="ProcessCommand"
    '                End If
    '
    '            ElseIf (m_lDocumentTemplateId <> 0) Then
    '
    '                m_lReturn& = g_oObjectManager.GetInstance( _
    ''                    oObject:=oDocTemplate, _
    ''                    sClassName:="bSIRDocTemplate.Business", _
    ''                    vInstanceManager:="ClientManager")
    '
    '                If (m_lReturn& <> PMTrue) Then
    '                    CheckFileExistsAndAllowLinkDeletion = PMFalse
    '                    MsgBox "Unable to get instance of bSIRDocTemplate.Business", vbCritical, ACApp
    '                    Exit Function
    '                End If
    '
    '                m_lReturn = oDocTemplate.DeleteDocumentLink(m_lDocumentTemplateId)
    '
    '                oDocTemplate.Terminate
    '
    '                Set oDocTemplate = Nothing
    '
    '                If (m_lReturn& <> PMTrue) Then
    '                    CheckFileExistsAndAllowLinkDeletion = PMFalse
    '                    MsgBox "Delete Document Link Failed", vbCritical, ACApp
    '                    Exit Function
    '                End If
    '            End If
    '        End If
    '
    '        CheckFileExistsAndAllowLinkDeletion = PMRecordDeleted
    '
    '    Else
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_CheckFileExistsAndAllowLinkDeletion:
    '
    '    CheckFileExistsAndAllowLinkDeletion = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="CheckFileExistsAndAllowLinkDeletion Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CheckFileExistsAndAllowLinkDeletion", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    'End Function
    Private Function CheckFileExistsAndAllowLinkDeletion(ByVal v_sServer As String, ByVal v_lEntityTypeID As Integer, ByVal v_lEntityCnt As Integer, ByVal v_lSlotNumber As Integer, ByVal v_lFileNumber As Integer) As Integer
        'Inserted MKW281003 PN7287 1.8.5 to 1.8.6 Catchup
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If FileSystem.Dir(v_sServer, FileAttribute.Normal) = "" Then
                'File does not exist
                If MessageBox.Show("The document requested does not exist. " & Strings.Chr(13) & Strings.Chr(10) & _
                                    " Path : " & v_sServer & Strings.Chr(13) & Strings.Chr(10) & _
                                    " Would you like to remove the database reference to this document?", "Client Manager - Text Files", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then


                    m_lReturn = m_oBusiness.DeleteTextFileDocumentLink(v_lEntityTypeID:=v_lEntityTypeID, v_lEntityCnt:=v_lEntityCnt, v_lSlotNumber:=v_lSlotNumber, v_lFileNumber:=v_lFileNumber)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMRecordDeleted
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete TextFile Link Failed for TextFile." & Strings.Chr(13) & Strings.Chr(10) & _
                                            "Entity Type : " & CStr(v_lEntityTypeID) & Strings.Chr(13) & Strings.Chr(10) & _
                                            "Entity Cnt  : " & CStr(v_lEntityCnt) & Strings.Chr(13) & Strings.Chr(10) & _
                                            "Slot Number : " & CStr(v_lSlotNumber) & Strings.Chr(13) & Strings.Chr(10) & _
                                            "File Number : " & CStr(v_lFileNumber), vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileExistsAndAllowLinkDeletion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                Else
                    ' Return PMCancel when template does not exist and user answers no
                    result = gPMConstants.PMEReturnCode.PMCancel
                    ' Set the interface status.
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Return result
                End If
            Else
                Return result
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileExistsAndAllowLinkDeletion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileExistsAndAllowLinkDeletion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result


        Return result
    End Function


    Private Function UpdateTemplateNumberAndDependencies(ByRef sPath As String, ByRef lOldId As Integer, ByRef lNewId As Integer) As Integer
        Dim result As Integer = 0
        Dim sOldClient, sNewClient, sParentFile As String

        Dim sOldDocRef As String = "Doc%20" & lOldId

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(01/09/2000) - RSAIB Process 108. Rename template to correct
            'number now.
            sOldClient = sPath & "\Doc " & CStr(lOldId) & "." & m_sDocFileExtension

            sNewClient = sPath & "\Doc " & CStr(lNewId) & "." & m_sDocFileExtension

            If sOldClient = sNewClient Then
                Return result
            End If

            FileSystem.Rename(sOldClient, sNewClient)

            'Check for dependencies and rename directory.
            sParentFile = sOldClient.Substring(0, sOldClient.Length - 4)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTemplateNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTemplateNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DJM 25/02/2004
    Private Function IsTemplateEditable(ByRef r_bEditable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            result = m_oBusiness.GetDocument(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimCnt:=m_lClaimCnt, v_lRiskCodeId:=m_lRiskCodeId, v_lRiskGroupId:=m_lRiskGroupId, v_lSlotNumber:=m_lSlotNumber, v_lSourceID:=m_lSourceId, r_bEditable:=r_bEditable)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsTemplateEditable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTemplateEditable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetFilePath(ByRef sPath As String) As Integer
        Return GetPath(sPath)
    End Function

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
    ' ***************************************************************** '
    '
    ' Name: SetWordVersionDependentVariables
    '
    ' Description: To deal with different versions of word which may
    '               behave very differently and store documents in
    '               differing formats.
    '
    ' History: 18/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function SetWordVersionDependentVariables() As Integer

        Dim result As Integer = 0
        'Dim sVersion As String
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            SetWordVersionDependentVariables = gPMConstants.PMEReturnCode.PMTrue

            m_sDocFileExtension = "xml"

            m_sFieldStartMarker = "&lt;@"
            m_sFieldEndMarker = "@&gt;"

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetWordVersionDependentVariables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetWordVersionDependentVariables", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=excep)
            Return result
        End Try
    End Function
End Class
