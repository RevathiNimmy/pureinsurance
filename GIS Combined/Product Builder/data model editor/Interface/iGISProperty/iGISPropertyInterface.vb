Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Modified by Archana Tokas on 4/30/2010 10:49:57 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 05/05/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' RKS 26/04/2005 : 354-Standard Wording Control Enchancements
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'Modified by Archana Tokas on 31/03/2010 02:12:30 added refer developer guide no.50
    Dim objFrmInterface As frmInterface
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lGISPropertyId As Integer
    Private m_lGISObjectId As Integer
    Private m_sPropertyName As String = ""
    Private m_sColumnName As String = ""
    Private m_lDataType As Integer
    Private m_lIsInputProperty As Integer
    Private m_lIsIdentifyingProperty As Integer
    Private m_lIsPrimaryKey As Integer
    Private m_lGISListId As Object
    ' developer guide no. 101
    Private m_lPolarisProperty As Object
    Private m_lIsDeleted As CheckState
    Private m_lIsSearchProperty As CheckState
    Private m_sPMLookupTableName As Object
    Private m_lPartyTypeId As Object
    Private m_lPMUSumInsuredType As Object
    Private m_lPMUStdWordingType As Object
    Private m_lGISUserDefHeaderId As Object
    Private m_lPMUProductId As Object
    Private m_lIndexLinkingId As Integer
    Private m_lEditFlags As Integer
    'Private m_vSpecialsType As Integer
    'Private m_vSpecialsTypeReference As Integer
    Private m_vSpecialsType As Object
    Private m_vSpecialsTypeReference As Object
    'Tomo151002
    Private m_lIsComment As Object

    Private m_vPartyTypeArray(,) As Object
    Private m_vSumInsuredTypeArray(,) As Object
    Private m_vDocumentFilterArray(,) As Object
    Private m_vPMLookupList(,) As Object
    Private m_vGISUserDefHeaderArray(,) As Object
    Private m_vProductArray(,) As Object
    Private m_vIndexLinkingArray(,) As Object

    Private m_vGISProperty(,) As Object

    Private m_lIsNonGIS As Integer
    Private m_lIsInMISExport As CheckState
    Private m_lIsFormattedText As CheckState
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    Private m_lSwiftIntegration As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_sGisDataModel As String = ""
    Private m_lGISDataModelTypeID As Integer

    Private m_lISClaim360Display As Integer


    ' PRIVATE Data Members (End)
    Private m_lIsChaseCycleProperty As CheckState

    Private m_bDisableClaim360Display As Boolean

    Public Property IsChaseCycleProperty() As CheckState
        Get
            Return m_lIsChaseCycleProperty
        End Get
        Set(ByVal Value As CheckState)


            m_lIsChaseCycleProperty = CInt(Value)
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property GISDataModelTypeID() As Integer
        Set(ByVal Value As Integer)
            m_lGISDataModelTypeID = Value
        End Set
    End Property

    Public WriteOnly Property GisDataModel() As String
        Set(ByVal Value As String)
            m_sGisDataModel = Value
        End Set
    End Property

    Public Property EditFlags() As Integer
        Get
            Return m_lEditFlags
        End Get
        Set(ByVal Value As Integer)
            m_lEditFlags = Value
        End Set
    End Property

    ' developer guide no. 71
    Public Property SpecialsType() As Integer
        'Public Property SpecialsType() As Object
        Get
            Return m_vSpecialsType
        End Get
        Set(ByVal Value As Integer)
            'Set(ByVal Value As Object)

            m_vSpecialsType = CInt(Value)


            If Convert.IsDBNull(m_vSpecialsType) Or IsNothing(m_vSpecialsType) Then
                m_vSpecialsType = (GISSharedPropertyConstants.ACOSpecialNone)
            End If
        End Set
    End Property
    ' developer guide no. 71
    Public Property SpecialsTypeReference() As Object
        Get
            Return m_vSpecialsTypeReference
        End Get
        Set(ByVal Value As Object)

            m_vSpecialsTypeReference = (Value)
        End Set
    End Property

    Public Property GISPropertyId() As Integer
        Get
            Return m_lGISPropertyId
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyId = Value
        End Set
    End Property

    Public Property GISObjectId() As Integer
        Get
            Return m_lGISObjectId
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectId = Value
        End Set
    End Property

    Public Property PropertyName() As String
        Get
            Return m_sPropertyName
        End Get
        Set(ByVal Value As String)
            m_sPropertyName = Value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return m_sColumnName
        End Get
        Set(ByVal Value As String)
            m_sColumnName = Value
        End Set
    End Property

    Public Property DataType() As Integer
        Get
            Return m_lDataType
        End Get
        Set(ByVal Value As Integer)
            m_lDataType = Value
        End Set
    End Property

    Public Property IsInputProperty() As Integer
        Get
            Return m_lIsInputProperty
        End Get
        Set(ByVal Value As Integer)
            m_lIsInputProperty = Value
        End Set
    End Property

    Public Property IsIdentifyingProperty() As Integer
        Get
            Return m_lIsIdentifyingProperty
        End Get
        Set(ByVal Value As Integer)
            m_lIsIdentifyingProperty = Value
        End Set
    End Property

    Public Property IsPrimaryKey() As Integer
        Get
            Return m_lIsPrimaryKey
        End Get
        Set(ByVal Value As Integer)
            m_lIsPrimaryKey = Value
        End Set
    End Property

    Public Property GISListId() As Object
        Get
            Return m_lGISListId
        End Get
        Set(ByVal Value As Object)


            m_lGISListId = Value
        End Set
    End Property
    ' developer guide no. 101
    Public Property PolarisPropertyId() As Object

        Get
            Return m_lPolarisProperty
        End Get
        Set(ByVal Value As Object)

            m_lPolarisProperty = (Value)
        End Set
    End Property

    Public Property IsDeleted() As CheckState
        Get
            Return m_lIsDeleted
        End Get
        Set(ByVal Value As CheckState)


            m_lIsDeleted = CInt(Value)
        End Set
    End Property

    Public Property IsSearchProperty() As CheckState
        Get
            Return m_lIsSearchProperty
        End Get
        Set(ByVal Value As CheckState)


            m_lIsSearchProperty = CInt(Value)
        End Set
    End Property

    Public Property PMLookupTableName() As Object
        Get
            Return m_sPMLookupTableName
        End Get
        Set(ByVal Value As Object)


            m_sPMLookupTableName = Value
        End Set
    End Property

    Public Property PartyTypeId() As Object
        Get
            Return m_lPartyTypeId
        End Get
        Set(ByVal Value As Object)


            m_lPartyTypeId = Value
        End Set
    End Property

    Public Property PMUSumInsuredType() As Object
        Get
            Return m_lPMUSumInsuredType
        End Get
        Set(ByVal Value As Object)


            m_lPMUSumInsuredType = Value
        End Set
    End Property

    Public Property PMUStdWordingType() As Object
        Get
            Return m_lPMUStdWordingType
        End Get
        Set(ByVal Value As Object)


            m_lPMUStdWordingType = Value
        End Set
    End Property

    Public Property GISUserDefHeaderId() As Object
        Get
            Return m_lGISUserDefHeaderId
        End Get
        Set(ByVal Value As Object)


            m_lGISUserDefHeaderId = Value
        End Set
    End Property

    Public Property PMUProductId() As Object
        Get
            Return m_lPMUProductId
        End Get
        Set(ByVal Value As Object)


            m_lPMUProductId = Value
        End Set
    End Property

    Public Property IndexLinkingId() As Integer
        Get
            Return m_lIndexLinkingId
        End Get
        Set(ByVal Value As Integer)

            m_lIndexLinkingId = CInt(Value)
        End Set
    End Property
    Public Property PartyTypeArray() As Object
        Get
            Return VB6.CopyArray(m_vPartyTypeArray)
        End Get
        Set(ByVal Value As Object)
            m_vPartyTypeArray = Value
        End Set
    End Property

    Public Property SumInsuredTypeArray() As Object
        Get
            Return VB6.CopyArray(m_vSumInsuredTypeArray)
        End Get
        Set(ByVal Value As Object)
            m_vSumInsuredTypeArray = Value
        End Set
    End Property

    Public Property DocumentFilterArray() As Object
        Get
            Return VB6.CopyArray(m_vDocumentFilterArray)
        End Get
        Set(ByVal Value As Object)
            m_vDocumentFilterArray = Value
        End Set
    End Property
    Public Property PMLookupList() As Object
        Get
            Return VB6.CopyArray(m_vPMLookupList)
        End Get
        Set(ByVal Value As Object)
            m_vPMLookupList = Value
        End Set
    End Property

    Public Property GISUserDefHeaderArray() As Object
        Get
            Return VB6.CopyArray(m_vGISUserDefHeaderArray)
        End Get
        Set(ByVal Value As Object)
            m_vGISUserDefHeaderArray = Value
        End Set
    End Property

    Public Property ProductArray() As Object
        Get
            Return VB6.CopyArray(m_vProductArray)
        End Get
        Set(ByVal Value As Object)
            m_vProductArray = Value
        End Set
    End Property

    Public Property IndexLinkingArray() As Object
        Get
            Return VB6.CopyArray(m_vIndexLinkingArray)
        End Get
        Set(ByVal Value As Object)
            m_vIndexLinkingArray = Value
        End Set
    End Property

    Public Property IsNonGIS() As Integer
        Get
            Return m_lIsNonGIS
        End Get
        Set(ByVal Value As Integer)
            m_lIsNonGIS = Value
        End Set
    End Property

    Public Property GISProperty() As Object
        Get
            Return VB6.CopyArray(m_vGISProperty)
        End Get
        Set(ByVal Value As Object)
            m_vGISProperty = Value
        End Set
    End Property

    Public Property IsInMISExport() As Integer
        Get
            Return m_lIsInMISExport
        End Get
        Set(ByVal Value As Integer)
            m_lIsInMISExport = Value
        End Set
    End Property
    Public Property IsFormattedText() As Integer
        Get
            Return m_lIsFormattedText
        End Get
        Set(ByVal Value As Integer)
            m_lIsFormattedText = Value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property
    '1 for swift mode 0 for normal
    Public WriteOnly Property SwiftIntegration() As Integer
        Set(ByVal Value As Integer)
            m_lSwiftIntegration = Value
        End Set
    End Property
    Public Property IsClaim360Display() As Integer
        Get
            Return m_lISClaim360Display
        End Get
        Set(ByVal Value As Integer)
            m_lISClaim360Display = Value
        End Set
    End Property
    Public Property DisableClaim360Display() As Boolean

        Get
            Return m_bDisableClaim360Display
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableClaim360Display = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername = .UserName
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case PMKeyNameID
                    '                m_iNameID% = CInt(vKeyArray(PMKeyValue, lRow&))

                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameNavigatorTitle1

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNavigatorTitle

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0
        objFrmInterface = New frmInterface


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        'developer guide no.50
        With objFrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .GISPropertyId = GISPropertyId
            .GISObjectId = GISObjectId
            .PropertyName = PropertyName
            .ColumnName = ColumnName
            .DataType = DataType
            .IsInputProperty = IsInputProperty
            .IsIdentifyingProperty = IsIdentifyingProperty
            .IsPrimaryKey = IsPrimaryKey
            .PolarisPropertyId = PolarisPropertyId
            .IsDeleted = IsDeleted
            .IsSearchProperty = IsSearchProperty
            .IndexLinkingId = IndexLinkingId
            '.EffectiveDate = EffectiveDate
            .EditFlags = EditFlags
            .SpecialsType = SpecialsType
            .SpecialsTypeReference = SpecialsTypeReference
            .IsChaseCycleProperty = IsChaseCycleProperty
            .IsNonGIS = IsNonGIS
            'developer guide no.24
            .PartyTypeArray = VB6.CopyArray(PartyTypeArray)
            .SumInsuredTypeArray = VB6.CopyArray(SumInsuredTypeArray)
            .DocumentFilterArray = VB6.CopyArray(DocumentFilterArray)
            .GISUserDefHeaderArray = VB6.CopyArray(GISUserDefHeaderArray)
            .ProductArray = VB6.CopyArray(ProductArray)
            .IndexLinkingArray = VB6.CopyArray(IndexLinkingArray)

            .GISProperty = VB6.CopyArray(m_vGISProperty)
            'SJ 21/01/2004 - start
            .GisDataModel = m_sGisDataModel
            'SJ 21/01/2004 - end
            ' {* USER DEFINED CODE (End) *}
            .SwiftIntegration = m_lSwiftIntegration
            .GISDataModelTypeID = m_lGISDataModelTypeID
            .PMLookupList = VB6.CopyArray(m_vPMLookupList)
            .IsInMISExport = m_lIsInMISExport
            .IsFormattedText = m_lIsFormattedText
            .IsClaim360Display = IsClaim360Display
            .DisableClaim360Display = DisableClaim360Display



        End With

        ' Load the instance of the interface into memory.

        ' Check if we have had an error so far.
        If objFrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = objFrmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        'developer guide no.50
        With objFrmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}

            GISPropertyId = .GISPropertyId
            GISObjectId = .GISObjectId
            PropertyName = .PropertyName
            ColumnName = .ColumnName
            DataType = .DataType
            IsInputProperty = .IsInputProperty
            IsIdentifyingProperty = .IsIdentifyingProperty
            IsPrimaryKey = .IsPrimaryKey
            PolarisPropertyId = .PolarisPropertyId
            IsDeleted = .IsDeleted
            IsSearchProperty = .IsSearchProperty
            IndexLinkingId = .IndexLinkingId
            EditFlags = .EditFlags
            SpecialsType = .SpecialsType
            SpecialsTypeReference = .SpecialsTypeReference
            IsInMISExport = .IsInMISExport
            IsFormattedText = .IsFormattedText
            ' {* USER DEFINED CODE (End) *}
            IsChaseCycleProperty = .IsChaseCycleProperty
            IsClaim360Display = .IsClaim360Display
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'developer guide no.50
        objFrmInterface.Close()
        objFrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        'developer guide no.50
        VB6.ShowForm(objFrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'developer guide no.50
            If objFrmInterface.ErrorNumber <> 0 Then
                result = objFrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

