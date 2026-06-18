Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUListMaint"

    ' Form Constants for Captions
    Public Const ACInterfaceCaption As Integer = 100
    Public Const ACMainTabTitle0 As Integer = 101
    Public Const ACMainTabTitle1 As Integer = 102
    Public Const ACLookupTypeCaption As Integer = 103
    Public Const ACNewValueCaption As Integer = 104
    Public Const ACValuesToAddCaption As Integer = 105
    Public Const ACStoredValuesCaption As Integer = 106
    Public Const ACComDialogSelectCaption As Integer = 107
    Public Const ACComDialogSaveCaption As Integer = 108
    Public Const ACFileTypeCaption As Integer = 109
    Public Const ACListFileCaption As Integer = 110
    Public Const ACOptionNewCaption As Integer = 111
    Public Const ACOptionExistingCaption As Integer = 112

    ' Button Constants for Captions
    Public Const ACHelpCaption As Integer = 200
    Public Const ACCancelCaption As Integer = 201
    Public Const ACOKCaption As Integer = 202
    Public Const ACRemoveCaption As Integer = 203
    Public Const ACUpdateCaption As Integer = 204
    Public Const ACSaveAsCaption As Integer = 205
    Public Const ACEditCaption As Integer = 206
    Public Const ACExitCaption As Integer = 207
    Public Const ACViewCaption As Integer = 208
    Public Const ACRemoveStoredCaption As Integer = 209

    'Constants for messages.
    Public Const ACSelectFileMsg As Integer = 300
    Public Const ACCancelMsg As Integer = 301
    Public Const ACWarnNotUpdatedMsg As Integer = 302
    Public Const ACWarnFileExistsMsg As Integer = 303
    Public Const ACIncludeNewValuesMsg As Integer = 304
    Public Const ACValueAlreadyExistsMsg As Integer = 305
    Public Const ACSaveErrorMsg As Integer = 306
    Public Const ACErrorAccessingBusObject As Integer = 307
    Public Const ACArchiveFailed As Integer = 308
    Public Const ACReleaseWarning As Integer = 309
    Public Const ACListManFailed As Integer = 310
    Public Const ACFailedRegistryGet As Integer = 311
    Public Const ACReleaseSuccess As Integer = 312
    Public Const ACFailedToGetReleasePath As Integer = 313
    Public Const ACFailedRegistrySet As Integer = 314
    Public Const ACUMRatesTable As Integer = 315
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sTitle As String = ""

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

    Public Const SIRClientListFilePath As String = "ClientListFilePath"
    Public Const SIRRegKeyListManagement As String = "ListManagement"

    'Public Const GEMServerListFilePath = "ServerListFilePath"
    'Public Const GEMRegKeyListManagement = "ListManagement"
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListFilePath As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListFilePathAndFile As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListFilePathIdx As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListFilePathDat As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListVersion As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sServerListPrefVersion As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bServerListFileCompressed As Boolean
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sGISDataModelCode As String = ""

    ' KB 010801 required for help text

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    Public Const ScreenHelpID As Integer = 4090
End Module