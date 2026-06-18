Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
'developer guide no. 129
Imports SharedFiles
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
	' Date:  20/05/05
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' ************************************************
	' Username.
	Private m_sUsername As String = ""
	' Password.
	Private m_sPassword As String = ""
	' User ID
	Private m_iUserID As Integer
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTExportPFTrans"
	
	' Constant for the functions to identify
	' which class this is.
Private Const ACClass As String = "MainModule" 
	
	'Return Code
	'***********************************************
	' GLOBAL VARIABLE IN BAS MODULE
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_sFilename As String = ""
	Private m_sDocFilename As String = ""
	Private m_bLogFileOpen As Boolean
	Private m_oSBODatabase As dPMDAO.Database
	Private m_oOrionDatabase As dPMDAO.Database
	Private m_oCommissionPost As Object
	Private m_oSystemOption As Object
	Private m_oTransDetail As Object
	Private m_oPMLock As Object
	'DC160605 PN21780 added log for export
	Private m_sMessage As String = ""
	Private m_sInfo As String = ""
	
	'***********************************************
	'Field Positions
	Private m_lFinancePlanRefPos As Integer
	Private m_lTransTypePos As Integer
	Private m_lAddress1Pos As Integer
	Private m_lAddress2Pos As Integer
	Private m_lAddress3Pos As Integer
	Private m_lAddress4Pos As Integer
	Private m_lAddress5Pos As Integer
	Private m_lPostCodePos As Integer
	Private m_lTotalGrossPremiumPos As Integer
	Private m_lValueOfInterestPos As Integer
	
	Private m_sFinanceProvider As String = ""
	Private m_sEDIDirectoryPathName As String = ""
	Private m_sDate As String = ""
	Private m_sProcessedEDIFilesDirectory As String = ""
	Private m_oFSO As Object
	Private m_oFolder As DirectoryInfo
	Private m_oSubFolder As DirectoryInfo
	Private m_oFile As FileInfo
	Private m_oFSO2 As Object
	Private m_oFolder2 As DirectoryInfo
	Private m_oSubFolder2 As DirectoryInfo
	Private m_oFile2 As FileInfo
	Private m_sAddress1 As String = ""
	Private m_sAddress2 As String = ""
	Private m_sAddress3 As String = ""
	Private m_sAddress4 As String = ""
	Private m_sAddress5 As String = ""
	Private m_sPostCode As String = ""
	Private m_cTotalGrossPremium As Decimal
	Private m_cValueOfInterest As Decimal
	Private m_cTransAmount As Decimal
	
	Private Const ACPlanReference As Integer = 0
	Private Const ACSchemeCode As Integer = 1
	Private Const ACBranchCode As Integer = 2
	Private Const ACClientReference As Integer = 3
	Private Const ACPartyTypeCode As Integer = 4
	Private Const ACBusinessName As Integer = 5
	Private Const ACPartyTitle As Integer = 6
	Private Const ACPartyForename As Integer = 7
	Private Const ACPartySurname As Integer = 8
	Private Const ACClientAddress1 As Integer = 9
	Private Const ACClientAddress2 As Integer = 10
	Private Const ACClientAddress3 As Integer = 11
	Private Const ACClientAddress4 As Integer = 12
	Private Const ACClientTown As Integer = 13
	Private Const ACClientPostCode As Integer = 14
	Private Const ACInceptionDate As Integer = 15
	Private Const ACRenewalDate As Integer = 16
	Private Const ACBankSortCode As Integer = 17
	Private Const ACBankAccountNo As Integer = 18
	Private Const ACBankAccountName As Integer = 19
	Private Const ACAmountToFinance As Integer = 20
	Private Const ACDepositAmount As Integer = 21
	Private Const ACNoOfInstallments As Integer = 22
	Private Const ACInstallmentAmount As Integer = 23
	Private Const ACTransType As Integer = 24
	
	Public Sub Main()
		
		m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			
			m_lReturn = CType(Process(), gPMConstants.PMEReturnCode)
			
		End If
		
		m_lReturn = CType(Terminate(), gPMConstants.PMEReturnCode)
		
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: Initialise
	'
	' Description:
	'
	'
	' ***************************************************************** '
    Private Function Initialise() As Integer
        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim vUserData(,) As Object
        Dim sOrionPath As String = ""
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC160605 PN21780 added log for export
            m_sMessage = "Fatal error in initialise"
            'Temporary thing for now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="OrionPath", r_sSettingValue:=sOrionPath), gPMConstants.PMEReturnCode)

            sDate = DateTimeHelper.ToString(DateTime.Today)
            While sDate.IndexOf("/"c) >= 0
                sDate = sDate.Substring(0, sDate.IndexOf("/"c)) & Mid(sDate, (sDate.IndexOf("/"c) + 1) + 1)
            End While


            'Initialise Objects
            m_oSBODatabase = New dPMDAO.Database()

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=m_oSBODatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'DC160605 PN21780 added log for export
                m_sMessage = "Failed to Get S4B Database"
                RaiseError("Initalise", m_sMessage)
            End If

            'Get Link TO Orion Database
            m_oOrionDatabase = New dPMDAO.Database()

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=m_oOrionDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DC160605 PN21780 added log for export
                m_sMessage = "Failed to Get Orion Database"
                result = gPMConstants.PMEReturnCode.PMFalse
                RaiseError("Initalise", m_sMessage)
            End If

            ' Set Username and Password
            m_sUsername = "siriuscomm"
            m_sPassword = "NGMBMKqSMcc5"

            With m_oSBODatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="Username", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="Password", vValue:=m_sPassword, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Get the User details
                m_lReturn = .SQLSelect(sSQL:=ACSelectUserDetailsSQL, sSQLName:=ACSelectUserDetailsName, bStoredProcedure:=ACSelectUserDetailsStored, vResultArray:=vUserData)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'DC160605 PN21780 added log for export
                    m_sMessage = "Unable to find User ID for User Sirius"
                    Return result
                End If
                If Not Information.IsArray(vUserData) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'DC160605 PN21780 added log for export
                    m_sMessage = "Unable to find user ID for User Sirius"
                    Return result
                End If
            End With


            m_iUserID = CInt(vUserData(0, 0))

            m_iLanguageID = CInt(vUserData(1, 0))
            m_iSourceID = 1
            m_iCurrencyID = 26
            m_iLogLevel = 9

            'Check to ensure this application is not already running, we cannot just use PMLock because
            'this only works for locks between users, not for the same user.
            m_lReturn = m_oSBODatabase.SQLSelect(sSQL:="SELECT lock_value FROM PMLock WHERE lock_name='" & ACApp & "'", sSQLName:="Check Lock", bStoredProcedure:=False, vResultArray:=vUserData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Information.IsArray(vUserData) Then
                'Drop out as another instance is running

                'DC160605 PN21780 added log for export
                m_sMessage = "PMLock beleives Export is already being run."
                m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Generate a lock record
            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(m_oPMLock, "bPMLock.User", ACApp, m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_oSBODatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", Initialise, Failed to initialise bPMLock")
            End If


            m_lReturn = m_oPMLock.LockKey(sKeyName:=ACApp, vKeyValue:=1, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Drop a line in the other log to inform user of multiple process launch
                Throw New System.Exception(m_lReturn.ToString() + ", Initialise, Failed to get an exclusive lock. This instance will terminate.")
            Else
                'We have exclusive access, start a new log now
                Try
                    File.Delete(m_sFilename)
                Catch ex As Exception

                End Try

            End If

            'Need to get the path name from the registry
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMERegSettingLevel.pmeRSLServer, "EDIPathName", m_sEDIDirectoryPathName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                'DC160605 PN21780 added log for export
                m_sMessage = "Failed to get EDI directory from registry."
                m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get EDI directory from registry: HKLM\PM\SiriusSolutions\Server\EDIPathName", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'if the path is not found in the registry then exit the function
            If m_sEDIDirectoryPathName = "" Then
                'this means that the path has not been specified by the user
                result = gPMConstants.PMEReturnCode.PMNotFound

                'DC160605 PN21780 added log for export
                m_sMessage = "EDI directory not set in the registry. : HKLM\PM\SiriusSolutions\Server\EDIPathName"
                m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EDI directory is not set in the registry: HKLM\PM\SiriusSolutions\Server\EDIPathName", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'if the directory doesn't exist then create the directory
            If Not gPMFunctions.FolderExists(m_sEDIDirectoryPathName) Then
                m_lReturn = CType(CreateDirectories(m_sEDIDirectoryPathName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Failed to creat EDI directory."
                    m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create EDI directory: " & m_sEDIDirectoryPathName, vApp:=ACApp, vClass:=ACClass, vMethod:="Process", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If

            If Not m_sEDIDirectoryPathName.EndsWith("\") Then
                m_sEDIDirectoryPathName = m_sEDIDirectoryPathName & "\"
            End If

            'Set Date
            m_sDate = StringsHelper.Format(DateTimeHelper.ToString(DateTime.Now), "YYYYMMDD_HHMM")

            ' Create Directory To Move Processed EDI Files To
            m_sProcessedEDIFilesDirectory = m_sEDIDirectoryPathName & m_sDate

            'if the directory doesn't exist then create the directory
            If Not gPMFunctions.FolderExists(m_sProcessedEDIFilesDirectory) Then
                m_lReturn = CType(CreateDirectories(m_sProcessedEDIFilesDirectory), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Failed to create 'processed' EDI directory."
                    m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create EDI directory: " & m_sProcessedEDIFilesDirectory, vApp:=ACApp, vClass:=ACClass, vMethod:="Process", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If

            m_sFilename = m_sProcessedEDIFilesDirectory & "\ExportPFTrans_" & m_sDate & ".log"


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)
        Finally

        End Try

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: Process
    '
    ' Description: Process EDI Files
    '
    ' Edit History
    '
    ' ***************************************************************** '

    Private Function Process() As Integer
        Dim result As Integer = 0
        Dim sFileName, sPathAndFile As String
        Dim vFinanceDataPos As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to obtaining Finance Plan Data Position."


            m_lReturn = CType(GetFinanceDataPos(vFinanceDataPos), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'DC160605 PN21780 added log for export
                m_sMessage = "Prior to obtaining Setting Data Positions."


                m_lFinancePlanRefPos = CInt(vFinanceDataPos(0, 0))

                m_lTransTypePos = CInt(vFinanceDataPos(1, 0))

                m_lAddress1Pos = CInt(vFinanceDataPos(2, 0))

                m_lAddress2Pos = CInt(vFinanceDataPos(3, 0))

                m_lAddress3Pos = CInt(vFinanceDataPos(4, 0))

                m_lAddress4Pos = CInt(vFinanceDataPos(5, 0))

                m_lPostCodePos = CInt(vFinanceDataPos(6, 0))

                m_lTotalGrossPremiumPos = CInt(vFinanceDataPos(7, 0))

                m_lValueOfInterestPos = CInt(vFinanceDataPos(8, 0))

                'DC250505 Set Finance Provider
                m_sFinanceProvider = "PC"

                m_oFSO = New Object()
                m_oFolder = New DirectoryInfo(m_sEDIDirectoryPathName)

                'DC160605 PN21780 added log for export
                m_sMessage = "Prior to processing each edi file."

                ' Process Each File
                For Each m_oFile3 As FileInfo In m_oFolder.GetFiles
                    m_oFile = m_oFile3

                    sFileName = m_oFile.Name

                    If sFileName.StartsWith("s-tradan.") Then

                        sPathAndFile = m_sEDIDirectoryPathName & sFileName

                        'DC160605 PN21780 added log for export
                        m_sMessage = "Prior to processing file " & sFileName.Trim() & "."

                        m_lReturn = CType(ProcessFile(sPathAndFile), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to moving processed file " & sFileName.Trim() & "."
                            'developer guide no. 73
                            'm_oFile.Move(m_sProcessedEDIFilesDirectory & "\" & sFileName)
                            m_oFile.MoveTo(m_sProcessedEDIFilesDirectory & "\" & sFileName)

                        End If

                    End If

                Next m_oFile3

            End If

            m_oFile = Nothing
            m_oFolder = Nothing
            m_oSubFolder = Nothing
            m_oFSO = Nothing



            m_lReturn = m_oPMLock.UnlockKey(sKeyName:=ACApp, vKeyValue:=1, iUserID:=m_iUserID)

            Return result

        Catch



            result = gPMConstants.PMEReturnCode.PMFalse

            'DC160605 PN21780 added log for export
            m_sInfo = "Fatal Error In 'Process'."
            m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

            m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

            'DC160605 PN21780 unlock even if there is an error

            m_lReturn = m_oPMLock.UnlockKey(sKeyName:=ACApp, vKeyValue:=1, iUserID:=m_iUserID)

            Return result
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: Close called components
    '
    '
    ' ***************************************************************** '
    Private Function Terminate() As Integer
        Dim result As Integer = 0
        Static bTerminated As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        'DC160605 PN21780 added log for export
        m_sMessage = "Fatal error in Terminate"

        ' If we have already Terminated then exit

        If bTerminated Then
            Return result
        Else
            bTerminated = True
        End If

        If Not (m_oSBODatabase Is Nothing) Then
            m_lReturn = m_oSBODatabase.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PM Data Access Object
            m_oSBODatabase = Nothing
        End If

        If Not (m_oOrionDatabase Is Nothing) Then
            m_lReturn = m_oOrionDatabase.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PM Data Access Object
            m_oOrionDatabase = Nothing
        End If

        If m_bLogFileOpen Then
            FileSystem.FileClose(1)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetFinanceDataPos (Private)
    '
    ' Description: Get Finance Data Position
    '
    ' ***************************************************************** '
    Public Function GetFinanceDataPos(ByRef vFinanceDataPos(,) As Object) As Integer 

        Dim result As Integer = 0 
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oSBODatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=ACGetFinanceDataPosSQL, sSQLName:=ACGetFinanceDataPosName, bStoredProcedure:=ACGetFinanceDataPosStored, vResultArray:=vFinanceDataPos, lNumberRecords:=gPMConstants.PMAllRecords)

                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMError
                End If

                If Not Information.IsArray(vFinanceDataPos) Then
                    Return gPMConstants.PMEReturnCode.PMError
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFinanceDataPos Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinanceDataPos", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFinanceDetails (Private)
    '
    ' Description: Get Finance Details
    '
    ' ***************************************************************** '
    Public Function GetFinanceDetails(ByVal v_sFinancePlanRef As String, ByRef vFinanceDetails(,) As Object) As Integer 

        Dim result As Integer = 0 
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oSBODatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="sFinancePlanRef", vValue:=v_sFinancePlanRef.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetFinanceDetailsSQL, sSQLName:=ACGetFinanceDetailsName, bStoredProcedure:=ACGetFinanceDetailsStored, vResultArray:=vFinanceDetails, lNumberRecords:=gPMConstants.PMAllRecords)

                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(vFinanceDetails) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFinanceDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinanceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateDirectories
    '
    ' Description:
    '
    ' History: 03/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Private Function CreateDirectories(ByRef sPathName As String) As Integer

        Dim result As Integer = 0
        Dim iPosition As Integer
        Dim vNameOfDir As String = ""
        Dim iStartPos, iEndPos As Integer
        Dim vDriveLetter As String = ""
        Dim vDirectory, sPathString As String
        Dim lReturn As gPMConstants.PMEReturnCode

        

            result = gPMConstants.PMEReturnCode.PMTrue

            sPathString = sPathName
            iStartPos = 4
            iEndPos = sPathString.Length

            vDriveLetter = Mid(sPathString, 1, 3)
            vDirectory = vDriveLetter

            For iCount As Integer = iStartPos To iEndPos

                iPosition = Strings.InStr(iStartPos, sPathString, "\")

                If iPosition = 0 Then
                    vNameOfDir = Mid(sPathString, iStartPos, iEndPos)
                    vDirectory = vDirectory & "\" & vNameOfDir
                    If Not Directory.Exists(vDirectory) Then
                        Directory.CreateDirectory(vDirectory)
                    End If
                    Exit For
                End If

                vNameOfDir = Mid(sPathString, iStartPos, iPosition - iStartPos)

                If iCount = 4 Then
                    vDirectory = vDirectory & vNameOfDir
                Else
                    vDirectory = vDirectory & "\" & vNameOfDir
                End If

                If Not Directory.Exists(vDirectory) Then
                    Directory.CreateDirectory(vDirectory)
                End If

                iStartPos = iPosition + 1

            Next iCount

            lReturn = gPMConstants.PMEReturnCode.PMTrue

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessFile
    '
    ' Description:
    '
    ' History: 03/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessFile(ByVal v_sFilePathAndName As String) As Integer

        Dim result As Integer = 0
        Dim iFileNum As Integer
        Dim sOldEDIString, sTransType, sFinancePlanRef, sStripString As String
        Dim vFinanceDetails As Object

        'DC270705 PN22630 if fail, log error and follow correct on error routine
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to processing file " & v_sFilePathAndName.Trim() & "."

            ' Open the chosen template document
            iFileNum = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNum, v_sFilePathAndName, OpenMode.Input)

            'Read in each line of document and analyse 1 by 1.
            Do While Not FileSystem.EOF(iFileNum)

                'DC160605 PN21780 added log for export
                m_sMessage = "Prior to reading line from " & v_sFilePathAndName.Trim() & "."

                sOldEDIString = FileSystem.LineInput(iFileNum)

                'get the finance provider code
                sStripString = sOldEDIString

                'DC140605 make sure checks reset
                sFinancePlanRef = ""
                sTransType = ""

                'DC140605 only process if contains commas
                If sStripString.IndexOf(","c) >= 0 Then

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Finance Plan Reference."

                    For iLoop As Integer = 1 To m_lFinancePlanRefPos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    sFinancePlanRef = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Transaction Type."

                    'get the transaction type
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lTransTypePos + 1

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    sTransType = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Address Line 1."

                    'get the address line 1
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lAddress1Pos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_sAddress1 = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Address Line 2."

                    'get the address line 2
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lAddress2Pos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_sAddress2 = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Address Line 3."

                    'get the address line 3
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lAddress3Pos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_sAddress3 = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Address Line 4."

                    'get the address line 4
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lAddress4Pos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_sAddress4 = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    m_sAddress5 = ""

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Postcode."

                    'get the postcode
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lPostCodePos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_sPostCode = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Transaction Amount."

                    'get the TotalGrossPremium
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lTotalGrossPremiumPos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_cTotalGrossPremium = CDec(sStripString.Substring(0, sStripString.IndexOf(","c)).Trim())

                    'get the ValueOfInterest
                    sStripString = sOldEDIString

                    For iLoop As Integer = 1 To m_lValueOfInterestPos

                        sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)

                    Next iLoop

                    m_cValueOfInterest = CDec(sStripString.Substring(0, sStripString.IndexOf(","c)).Trim())

                    m_cTransAmount = m_cTotalGrossPremium - m_cValueOfInterest

                Else

                    'DC160605 PN21780 added log for export
                    m_sInfo = v_sFilePathAndName.Trim() & " not processed. Doesnt contain commas."
                    m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                End If

                'DC250505 be more specific as to what is valid Premium Credit plan
                If sFinancePlanRef <> "" And ((sTransType = "NB" Or sTransType = "RN") Or (sTransType = "RC" Or sTransType = "M")) Or sTransType = "AP" Or sTransType = "MT" Then

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to getting Finance Plan Details."


                    m_lReturn = CType(GetFinanceDetails(sFinancePlanRef, vFinanceDetails), gPMConstants.PMEReturnCode)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        'DC160605 PN21780 added log for export
                        m_sMessage = "Prior to writing to EDI export file for " & v_sFilePathAndName.Trim() & "."


                        m_lReturn = CType(WriteEDIFile(sTransType, vFinanceDetails), gPMConstants.PMEReturnCode)

                        'DC160605 PN21780 added log for export
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            m_sInfo = v_sFilePathAndName.Trim() & " processed successfully."
                            m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                        Else

                            m_sInfo = "Failed to write EDI Export file for " & v_sFilePathAndName.Trim() & "."
                            m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                            result = gPMConstants.PMEReturnCode.PMFalse

                        End If

                    Else

                        'DC160605 PN21780 added log for export
                        m_sInfo = "Failed to get finance plan details for " & v_sFilePathAndName.Trim() & "."
                        m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                        result = gPMConstants.PMEReturnCode.PMFalse

                    End If

                    'DC250505 identify that file was not processed
                Else

                    'DC160605 PN21780 added log for export
                    m_sInfo = v_sFilePathAndName.Trim() & " not processed. Not Premium Credit."
                    m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                    result = gPMConstants.PMEReturnCode.PMFalse

                End If

            Loop

            FileSystem.FileClose(iFileNum)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'DC160605 PN21780 added log for export
            m_sInfo = "Fatal Error In 'ProcessFile', when processing " & v_sFilePathAndName.Trim()
            m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

            m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteEDIFile
    '
    ' Description:
    '
    ' History: DC 200505 Created
    '
    ' ***************************************************************** '
    Private Function WriteEDIFile(ByVal v_sTrans As String, ByVal v_vFinanceDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sFinanceProvider As String = ""
        Dim iFileNumber, iFileNumber2 As Integer
        Dim sEDIString, sFilePathAndName, sFileName, sTrans As String
        Dim bFileFound As Boolean
        Dim sTempFileName As String = ""
        Dim lNextBatchNumber As Integer
        Dim sStripString As String = ""
        'Header Record Details
        Dim sHeaderIdentifier As String
        'Header Values
        Dim sSchemeCode, sBranchCode, sBatchNumber As String
        Dim lItemCount As Integer
        Dim cBatchValue As Decimal
        'Body Details
        Dim sClientReference, sTitle, sForename, sSurname, sBusinessName, sAddress1, sAddress2, sAddress3, sAddress4, sAddress5, sPostCode, sInceptionDate, sRenewalDate, sBankSortCode, sBankAccountNo, sBankAccountName, sPremium, sPremiumSign As String
        Dim cPremium, cPremiumTotal As Decimal
        Dim sPriorDeposit, sInstallmentAmount, sInstallmentNumber As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to writing EDI Export File"

            sHeaderIdentifier = "H"


            sFinanceProvider = CStr(v_vFinanceDetails(ACPlanReference, 0)).Trim()

            sSchemeCode = CStr(v_vFinanceDetails(ACSchemeCode, 0)).Trim()

            sBranchCode = CStr(v_vFinanceDetails(ACBranchCode, 0)).Trim()

            sClientReference = CStr(v_vFinanceDetails(ACClientReference, 0)).Trim().Substring(0, 16)

            If CStr(v_vFinanceDetails(ACPartyTypeCode, 0)).Trim() = "PC" Then
                sBusinessName = ""

                sTitle = CStr(v_vFinanceDetails(ACPartyTitle, 0)).Trim()

                sForename = CStr(v_vFinanceDetails(ACPartyForename, 0)).Trim()

                sSurname = CStr(v_vFinanceDetails(ACPartySurname, 0)).Trim()
            End If

            If CStr(v_vFinanceDetails(ACPartyTypeCode, 0)).Trim() = "CC" Then

                sBusinessName = CStr(v_vFinanceDetails(ACBusinessName, 0)).Trim()
                sTitle = ""
                sForename = ""
                sSurname = ""
            End If

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to Setting up Address lines."

            'DC250505 address now taken from the original s-tradans file
            'sAddress1 = Left$(Trim(v_vFinanceDetails(ACClientAddress1, 0)), 35)
            'sAddress2 = Left$(Trim(v_vFinanceDetails(ACClientAddress2, 0)), 35)
            'sAddress3 = Left$(Trim(v_vFinanceDetails(ACClientAddress3, 0)), 35)
            'sAddress4 = Left$(Trim(v_vFinanceDetails(ACClientAddress4, 0)), 35)
            'sAddress5 = Left$(Trim(v_vFinanceDetails(ACClientTown, 0)), 15)
            'sPostCode = Trim(v_vFinanceDetails(ACClientPostCode, 0))
            sAddress1 = m_sAddress1.Substring(0, 35)
            sAddress2 = m_sAddress2.Substring(0, 35)
            sAddress3 = m_sAddress3.Substring(0, 35)
            sAddress4 = m_sAddress4.Substring(0, 35)
            sAddress5 = m_sAddress5.Substring(0, 35)
            sPostCode = m_sPostCode.Substring(0, 35)

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to Setting up Dates."

            'DC250505 changed from YYYY to YY

            sInceptionDate = CDate(CStr(v_vFinanceDetails(ACInceptionDate, 0)).Trim()).ToString("ddMMyy")

            sRenewalDate = CDate(CStr(v_vFinanceDetails(ACRenewalDate, 0)).Trim()).ToString("ddMMyy")

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to Setting up Bank Details."

            'DC250505 added quotes around sort code and account number as excel removes leading spaces

            If CStr(v_vFinanceDetails(ACBankSortCode, 0)).Trim() = "" Then
                sBankSortCode = "000000"
            Else

                sBankSortCode = StringsHelper.Format(CInt(CStr(v_vFinanceDetails(ACBankSortCode, 0)).Trim()), "000000")
            End If

            If CStr(v_vFinanceDetails(ACBankAccountNo, 0)).Trim() = "" Then
                sBankAccountNo = "00000000"
            Else

                sBankAccountNo = StringsHelper.Format(CInt(CStr(v_vFinanceDetails(ACBankAccountNo, 0)).Trim()), "00000000")
            End If
            'DC250505 had ACBankAccountNo incorrectly

            sBankAccountName = CStr(v_vFinanceDetails(ACBankAccountName, 0)).Trim().Substring(0, 18)

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to Setting up Premium figures."

            'DC250505 set amounts correctly
            'first the deposit

            cPremium = CDec(CStr(v_vFinanceDetails(ACDepositAmount, 0)).Trim())
            sPriorDeposit = StringsHelper.Format(cPremium, "0.00")
            cPremiumTotal -= Math.Abs(cPremium)
            'then the actual premium
            cPremium = m_cTransAmount
            'DC260505 new field for checking whether AP or RP
            sPremiumSign = StringsHelper.Format(cPremium, "0.00")
            sPremium = StringsHelper.Format(Math.Abs(cPremium), "0.00")
            cPremiumTotal += Math.Abs(m_cTransAmount)
            'then the instalment amount

            cPremium = CDec(CStr(v_vFinanceDetails(ACInstallmentAmount, 0)).Trim())
            sInstallmentAmount = StringsHelper.Format(cPremium, "0.00")


            sInstallmentNumber = StringsHelper.Format(CDec(CStr(v_vFinanceDetails(ACNoOfInstallments, 0)).Trim()), "0")

            'as there is an issue where renewal plans are coming through as NB trans type,
            'this will ensure from the rates selected whether it is definitely a renewal

            If CStr(v_vFinanceDetails(ACTransType, 0)).Trim() = "REN" And v_sTrans = "NB" Then
                v_sTrans = "RN"
            End If

            Select Case v_sTrans.Trim().ToUpper()
                Case "NB"
                    sTrans = "NB"
                Case "M", "MT", "AP"
                    sTrans = "MTA"
                Case "RC"
                    sTrans = "CAN"
                Case "RN"
                    sTrans = "RN"
                    'DC250505 for change of address
            End Select

            'DC250505 use common code for finance provider
            Dim TempDate As Date
            sFileName = m_sFinanceProvider & "_" & sTrans & "_EDI_Export_" & (IIf(DateTime.TryParse(DateTimeHelper.ToString(DateTime.Now), TempDate), TempDate.ToString("yyyyMMdd"), DateTimeHelper.ToString(DateTime.Now))) & ".csv"
            sFilePathAndName = m_sEDIDirectoryPathName & sFileName

            m_oFSO2 = New Object()
            m_oFolder2 = New DirectoryInfo(m_sEDIDirectoryPathName)

            'Move file being processed to a temp file ...
            sTempFileName = m_sEDIDirectoryPathName & "Temp_" & sFileName

            bFileFound = False

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to creating Temp File."

            For Each m_oFile3 As FileInfo In m_oFolder.GetFiles
                m_oFile2 = m_oFile3

                'Its The File We Need ...
                If m_oFile2.Name = sFileName Then
                    'developer guide no. 73
                    'm_oFile2.Move(sTempFileName)
                    m_oFile2.MoveTo(sTempFileName)

                    bFileFound = True

                    'Now get the header details .....
                    'Open EDI File
                    iFileNumber = FileSystem.FreeFile()
                    FileSystem.FileOpen(iFileNumber, sTempFileName, OpenMode.Input)

                    'Read in each line of document and analyse 1 by 1.
                    Do While Not FileSystem.EOF(iFileNumber)

                        sEDIString = FileSystem.LineInput(iFileNumber)

                        'DC160605 PN21780 added log for export
                        m_sMessage = "Prior to getting Header Details."

                        'Its The Header Details ...
                        If sEDIString.Substring(0, 1) = sHeaderIdentifier Then

                            'Get whole record ...
                            sStripString = sEDIString

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to getting Header Scheme Code."

                            'Get the Scheme Code ...
                            sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)
                            sSchemeCode = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to getting Header Branch Code."

                            'Get the Branch Code ...
                            sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)
                            sBranchCode = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to getting Header Batch Number."

                            'Get the Batch Number ...
                            sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)
                            sBatchNumber = sStripString.Substring(0, sStripString.IndexOf(","c)).Trim()

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to getting Header Item Count."

                            'Get the Item Count ...
                            sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)
                            lItemCount = CInt(sStripString.Substring(0, sStripString.IndexOf(","c)).Trim())

                            'DC160605 PN21780 added log for export
                            m_sMessage = "Prior to getting Header Batch Value."

                            'Get the Batch Value ...
                            sStripString = sStripString.Substring(sStripString.IndexOf(","c) + 1)
                            'DC270505 for cancellation batchs that have no value
                            If sStripString.Trim() = "" Then
                                cBatchValue = 0
                            Else

                                cBatchValue = CDec(sStripString.Trim())

                            End If

                        End If

                    Loop

                    FileSystem.FileClose(iFileNumber)

                End If

            Next m_oFile3

            m_oFile2 = Nothing
            m_oFolder2 = Nothing
            m_oSubFolder2 = Nothing
            m_oFSO2 = Nothing

            '*** SET UP HEADER DETAILS IF NEW HEADER

            If Not bFileFound Then

                'DC160605 PN21780 added log for export
                m_sMessage = "Prior to getting next batch number."

                m_lReturn = CType(GetNextEDIBatchNumber(sTrans, lNextBatchNumber), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    Select Case v_sTrans.Trim().ToUpper()
                        Case "NB"
                            sBatchNumber = "N" & StringsHelper.Format(lNextBatchNumber, "0000")
                        Case "M", "AP", "MT"
                            sBatchNumber = "T" & StringsHelper.Format(lNextBatchNumber, "0000")
                        Case "RC"
                            sBatchNumber = "C" & StringsHelper.Format(lNextBatchNumber, "0000")
                        Case "RN"
                            sBatchNumber = "R" & StringsHelper.Format(lNextBatchNumber, "0000")
                    End Select

                    m_lReturn = CType(UpdateNextEDIBatchNumber(sTrans), gPMConstants.PMEReturnCode)

                    lItemCount = 0

                    cBatchValue = 0

                Else

                    'DC160605 PN21780 added log for export
                    m_sInfo = "Failed to get next batch number."
                    m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

            End If

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to setting up line for temporary file."

            '*** WRITE NEW RECORD TO TEMP EDI FILE ***

            'Open the temp file to write new record to it ...
            iFileNumber = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNumber, sTempFileName, OpenMode.Append)

            'Build the EDI String
            sEDIString = ""


            Select Case v_sTrans.Trim().ToUpper()
                Case "NB"

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to setting up NB line for temporary file."

                    sEDIString = sEDIString & sBatchNumber & ","
                    sEDIString = sEDIString & sSchemeCode & ","
                    'DC250505 removed branch code
                    'sEDIString = sEDIString & sBranchCode & ","
                    sEDIString = sEDIString & ","
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sTitle & ","
                    sEDIString = sEDIString & sForename & ","
                    sEDIString = sEDIString & sSurname & ","
                    sEDIString = sEDIString & sBusinessName & ","
                    sEDIString = sEDIString & sAddress1 & ","
                    sEDIString = sEDIString & sAddress2 & ","
                    sEDIString = sEDIString & sAddress3 & ","
                    sEDIString = sEDIString & sAddress4 & ","
                    sEDIString = sEDIString & sAddress5 & ","
                    sEDIString = sEDIString & sPostCode & ","
                    sEDIString = sEDIString & sInceptionDate & ","
                    sEDIString = sEDIString & sRenewalDate & ","
                    sEDIString = sEDIString & sBankSortCode & ","
                    sEDIString = sEDIString & sBankAccountNo & ","
                    sEDIString = sEDIString & sBankAccountName & ","
                    sEDIString = sEDIString & sPremium & ","
                    sEDIString = sEDIString & sPriorDeposit & ","
                    sEDIString = sEDIString & sInstallmentAmount & ","
                    sEDIString = sEDIString & sInstallmentNumber & ",,,"

                Case "M", "AP", "MT"

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to setting up MTA line for temporary file."

                    sEDIString = sEDIString & sBatchNumber & ","
                    sEDIString = sEDIString & sSchemeCode & ",,"
                    'DC250505 removed branch code
                    'sEDIString = sEDIString & sBranchCode & ",,"
                    'DC260505 client ref and new client ref to be the same
                    'sEDIString = sEDIString & ",,"
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sTitle & ","
                    sEDIString = sEDIString & sForename & ","
                    sEDIString = sEDIString & sSurname & ","
                    sEDIString = sEDIString & sBusinessName & ","
                    sEDIString = sEDIString & sPostCode & ","
                    'DC250505 added mta type
                    If v_sTrans = "AP" Then
                        sEDIString = sEDIString & ","
                    Else
                        'DC260505 check new field for whether AP or RP
                        If CDec(sPremiumSign) < 0 Then
                            sEDIString = sEDIString & "RA,"
                        Else
                            sEDIString = sEDIString & "AA,"
                        End If

                    End If

                    sEDIString = sEDIString & sAddress1 & ","
                    sEDIString = sEDIString & sAddress2 & ","
                    sEDIString = sEDIString & sAddress3 & ","
                    sEDIString = sEDIString & sAddress4 & ","
                    sEDIString = sEDIString & sAddress5 & ","
                    sEDIString = sEDIString & sPostCode & ","
                    sEDIString = sEDIString & sPremium & ",,"

                Case "RC"

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to setting up CAN line for temporary file."

                    sEDIString = sEDIString & sBatchNumber & ","
                    sEDIString = sEDIString & sSchemeCode & ","
                    'DC250505 removed branch code
                    'sEDIString = sEDIString & sBranchCode & ","
                    sEDIString = sEDIString & ","
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sTitle & ","
                    sEDIString = sEDIString & sForename & ","
                    sEDIString = sEDIString & sSurname & ","
                    sEDIString = sEDIString & sBusinessName & ","
                    sEDIString = sEDIString & sPostCode & ",,"

                Case "RN"

                    'DC160605 PN21780 added log for export
                    m_sMessage = "Prior to setting up REN line for temporary file."

                    sEDIString = sEDIString & sBatchNumber & ","
                    sEDIString = sEDIString & sSchemeCode & ","
                    'DC250505 removed branch code
                    'sEDIString = sEDIString & sBranchCode & ",,"
                    'DC020605 use reference for both CLIENT REFERENCE & NEW REFERENCE
                    'sEDIString = sEDIString & ",,"
                    sEDIString = sEDIString & ","
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sClientReference & ","
                    sEDIString = sEDIString & sTitle & ","
                    sEDIString = sEDIString & sForename & ","
                    sEDIString = sEDIString & sSurname & ","
                    sEDIString = sEDIString & sBusinessName & ","
                    sEDIString = sEDIString & sPostCode & ","
                    sEDIString = sEDIString & sRenewalDate & ","
                    sEDIString = sEDIString & sPremium & ",,,"

            End Select

            'Write the new record to file ...
            FileSystem.PrintLine(iFileNumber, sEDIString)
            FileSystem.FileClose(iFileNumber)

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to writing header to export file."

            '*** WRITE NEW HEADER RECORD TO EDI FILE ***

            'Open the new edi file to create new header and copy temp records to it ...
            iFileNumber = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNumber, sFilePathAndName, OpenMode.Append)

            'Set new values for header record ...
            lItemCount += 1
            cBatchValue += cPremiumTotal

            'Build the EDI String For New Header
            sEDIString = ""

            'Build up header records ...
            sEDIString = sEDIString & "H,"
            sEDIString = sEDIString & sSchemeCode & ","
            'DC250505 removed branch code
            'sEDIString = sEDIString & sBranchCode & ","
            sEDIString = sEDIString & ","
            sEDIString = sEDIString & sBatchNumber & ","
            sEDIString = sEDIString & CStr(lItemCount) & ","

            'DC270505 do not need batch value for cancellations
            If v_sTrans.Trim() <> "RC" Then

                sEDIString = sEDIString & StringsHelper.Format(Math.Abs(cBatchValue), "0.00")

            End If

            'Write the header record to file ...
            FileSystem.PrintLine(iFileNumber, sEDIString)

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to copying lines from temporary file to export file."

            '*** COPY RECORDS FROM TEMP EDI FILE TO EDI FILE ***

            'Open EDI File
            iFileNumber2 = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNumber2, sTempFileName, OpenMode.Input)

            'Read in each line of file ...
            Do While Not FileSystem.EOF(iFileNumber2)

                sEDIString = ""

                sEDIString = FileSystem.LineInput(iFileNumber2)

                'Ignore old header record ...
                If sEDIString.Substring(0, 1) <> sHeaderIdentifier Then

                    'Copy record to file ...
                    FileSystem.PrintLine(iFileNumber, sEDIString)

                End If

            Loop

            FileSystem.FileClose(iFileNumber2)
            FileSystem.FileClose(iFileNumber)

            'DC160605 PN21780 added log for export
            m_sMessage = "Prior to deleting temporary file."

            '*** REMOVE THE TEMPORARY FILE ***

            m_oFSO2 = New Object()
            m_oFolder2 = New DirectoryInfo(m_sEDIDirectoryPathName)

            File.Delete(sTempFileName)
            m_oFSO2 = Nothing
            m_oFolder2 = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteEDIFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteEDIFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'DC160605 PN21780 added log for export
            m_sInfo = "Fatal Error In 'WriteEDIFile'."
            m_lReturn = CType(ProcessLog(sMessage:=m_sInfo), gPMConstants.PMEReturnCode)

            m_lReturn = CType(ProcessLog(sMessage:=m_sMessage), gPMConstants.PMEReturnCode)

            Return result
        End Try
    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetNextEDIBatchNumber (Private)
    '
    ' Description: Get Next EDI Batch Number
    '
    ' ***************************************************************** '
    Public Function GetNextEDIBatchNumber(ByVal v_sBatchType As String, ByRef lNextBatchNumber As Integer) As Integer 

        Dim result As Integer = 0 
        Dim vNextBatchNumber(,) As Object 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oSBODatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="sBatchType", vValue:=v_sBatchType.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetNextEDIBatchNumberSQL, sSQLName:=ACGetNextEDIBatchNumberName, bStoredProcedure:=ACGetNextEDIBatchNumberStored, vResultArray:=vNextBatchNumber, lNumberRecords:=gPMConstants.PMAllRecords)

                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(vNextBatchNumber) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            lNextBatchNumber = CInt(vNextBatchNumber(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextEDIBatchNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextEDIBatchNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: UpdateNextEDIBatchNumber (Private)
	'
	' Description: Update Next EDI Batch Number
	'
	' ***************************************************************** '
	Public Function UpdateNextEDIBatchNumber(ByVal v_sBatchType As String) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With m_oSBODatabase
				
				.Parameters.Clear()
				
				m_lReturn = .Parameters.Add(sName:="sBatchType", vValue:=v_sBatchType.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = .SQLSelect(sSQL:=ACUpdateNextEDIBatchNumberSQL, sSQLName:=ACUpdateNextEDIBatchNumberName, bStoredProcedure:=ACUpdateNextEDIBatchNumberStored)
				
				result = m_lReturn
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateNextEDIBatchNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNextEDIBatchNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function ProcessLog(ByRef sMessage As String) As Integer
		Dim lFile As Integer
		
		If Not m_bLogFileOpen Then
			m_bLogFileOpen = True
			lFile = FileSystem.FreeFile()
			FileSystem.FileOpen(lFile, m_sFilename, OpenMode.Output)
			FileSystem.PrintLine(lFile, sMessage)
			FileSystem.FileClose(lFile)
		Else
			lFile = FileSystem.FreeFile()
			FileSystem.FileOpen(lFile, m_sFilename, OpenMode.Append)
			FileSystem.PrintLine(lFile, sMessage)
			FileSystem.FileClose(lFile)
		End If
		
		Exit Function
		
	End Function
End Module
