Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("InterfaceNoLogin_NET.InterfaceNoLogin")> _
Public NotInheritable Class InterfaceNoLogin
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Wrapper
    '
    ' Date: 16/09/1998
    '
    ' Description: Main public class of the Wrapper.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Initialise"

    Private Const acModule As String = "InterfaceNoLogin"

    Private m_vVehicleData() As Object
    Private _m_oCommon As Common = Nothing
    Private Property m_oCommon() As Common
        Get
            If _m_oCommon Is Nothing Then
                _m_oCommon = New Common()
            End If
            Return _m_oCommon
        End Get
        Set(ByVal Value As Common)
            _m_oCommon = value
        End Set
    End Property
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lMaxListItems As Integer

    'sj 02/02/2001 - start
    Public WriteOnly Property NumberOfVehicles() As Integer
        Set(ByVal Value As Integer)
            m_oCommon.NumberOfVehicles = Value
        End Set
    End Property
    Public WriteOnly Property VehicleListId() As String
        Set(ByVal Value As String)
            m_oCommon.VehicleListId = Value
        End Set
    End Property
    'sj 02/02/2001 - end
    Public WriteOnly Property MaxListItems() As Integer
        Set(ByVal Value As Integer)
            m_lMaxListItems = Value
        End Set
    End Property
    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_oCommon.ClassOfBusiness = Value
        End Set
    End Property

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

            ' removed all the object manager stuff - CL230699

            m_lMaxListItems = GEMMaxListItems

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oCommon.CloseFiles()
                m_oCommon = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetListIdsAndNames
    '
    ' Description:
    '
    ' History: 19/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetListIdsAndNames(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Return m_oCommon.GetListIdsAndNamesC(r_vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListIdsAndNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListIdsAndNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetVehicleList(ByRef r_vListData As Object, ByVal v_vMake As String, Optional ByVal v_vModelChoosen As Object = Nothing, Optional ByVal v_vYear As Object = Nothing, Optional ByVal v_vCC As Object = Nothing, Optional ByVal v_vDoors As Object = Nothing, Optional ByVal v_vFuelType As Object = Nothing, Optional ByVal v_vTransType As Object = Nothing) As Integer

        'DB 25/11/99 Added 3 extra filter parameters

        m_oCommon.MaxListItems = m_lMaxListItems






        Return m_oCommon.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=CStr(v_vModelChoosen), v_vYear:=CStr(v_vYear), v_vCC:=CStr(v_vCC), v_vDoors:=CStr(v_vDoors), v_vFuelType:=CStr(v_vFuelType), v_vTransType:=CStr(v_vTransType))

    End Function

    'DB 31/1/2000 Start

    'Added this method to call GetVehicleModels on the Common class

    Public Function GetVehicleModels(ByRef r_vListData() As Object, ByVal v_vMake As String) As Integer

        Try

            Dim vListData As Object
            Dim iPos As Integer
            Dim jPos As Integer
            Dim sModel As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sFirstWord As String = ""


            m_oCommon.MaxListItems = m_lMaxListItems
            lReturn = CType(m_oCommon.GetVehicleModels(r_vListData:=vListData, v_vMake:=v_vMake), gPMConstants.PMEReturnCode)


            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Get the simple model names only out of the vehicles array
                If Information.IsArray(vListData) Then


                    ReDim r_vListData(vListData.GetUpperBound(0))


                    For iIndex As Integer = vListData.GetLowerBound(0) To vListData.GetUpperBound(0)

                        'Get the model

                        sModel = Mid(CStr(vListData(iIndex)), 29, 42).Trim()

                        'Find the first word out of it (simple model name)
                        'if more than one word.
                        iPos = (sModel.IndexOf(" "c) + 1)

                        If iPos <> 0 Then
                            sFirstWord = sModel.Substring(0, Math.Min(sModel.Length, iPos)).Trim()

                            'If first word is just the make, take the second word
                            If v_vMake.Substring(0, 4).Trim().ToUpper() = sFirstWord.Substring(0, 4).Trim().ToUpper() Then

                                'Look for the 2nd word
                                jPos = Strings.InStr(iPos + 1, sModel, " ", CompareMethod.Text)
                                If jPos <> 0 Then
                                    '2nd word, if more than 2 words
                                    sModel = sModel.Substring(iPos, Math.Min(sModel.Length, jPos - (iPos + 1))).Trim().ToUpper()
                                Else
                                    'Just 2nd word
                                    sModel = sModel.Substring(iPos).Trim().ToUpper()
                                End If
                            Else
                                sModel = sFirstWord 'No Problem, 1st word is the model
                            End If
                        End If

                        'If not duff record then ...
                        If sModel <> "" Then

                            'Capitalize the first letter, and convert the rest to lower case
                            sModel = sModel.Substring(0, 1).ToUpper() & _
                                     sModel.Substring(1, sModel.Length - 1).ToLower()

                            'Put it into the return array

                            r_vListData(iIndex) = sModel

                        End If

                        'Debug.Print r_vListData(iIndex)
                    Next


                    'Shell sort the array
                    ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))

                End If

            End If


            Return lReturn

        Catch ex As Exception



            ' Error Section.
            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicleModels in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModels", excep:=ex)

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    'DB 31/1/2000 End

    ' CTAF 20040122 - Start
    ' We need to have the ByRef value as a variant as this method will
    ' be called from a script
    Public Function GetDescriptionFromABICodeScripting(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sDescTemp As String = ""

        result = GetDescriptionFromABICode(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=sDescTemp)

        r_sDescription = sDescTemp

        Return result
    End Function
    ' CTAF 20040122 - End

    Public Function GetDescriptionFromABICode(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode


        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetDescriptionFromABICodeC(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=r_sDescription), gPMConstants.PMEReturnCode)

    End Function

    Public Function GetABICodeFromDescription(ByVal v_sPropertyId As String, ByVal v_sDescription As String, ByRef r_sABICode As String) As gPMConstants.PMEReturnCode


        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetABICodeFromDescriptionC(v_sPropertyId:=v_sPropertyId, v_sDescription:=v_sDescription, r_sABICode:=r_sABICode), gPMConstants.PMEReturnCode)

    End Function

    ' CL090699 BEGIN-->
    'Developer Guide No 101
    Public Function GetListAndCodes(ByVal v_sPropertyId As String, ByRef r_vListData As Object, ByRef r_vListDataCode As Object, Optional ByVal v_vSearchString As Object = Nothing, Optional ByVal v_bMultiSearch As Boolean = False) As Integer

        Dim result As Integer = 0
        m_oCommon.MaxListItems = m_lMaxListItems

        'DB 16/2/2000 Start

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lReturn As gPMConstants.PMEReturnCode

        'If it's a occupation/employer's business search, we now need
        'to loop up to 3 times until we get less than 15 matches (hopefully).
        'First loop with a 3 character search, then 4, then 5 (stop at 5).

        If v_bMultiSearch Then
            For iNoChars As Integer = 3 To 5

                'If the number of characters is greater than the length of the search string,
                'don't continue in the loop - it isn't going to get any better.

                If iNoChars > v_vSearchString.Length Then
                    Exit For
                End If

                lReturn = CType(m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode, v_bMultiSearch:=True, v_iFirstNoChars:=iNoChars), gPMConstants.PMEReturnCode)

                If Information.IsArray(r_vListData) Then
                    If (r_vListData.GetUpperBound(0) + 1) <= 15 Then
                        result = lReturn
                        Exit For
                    End If
                Else
                    Return lReturn
                End If

            Next
        Else
            result = m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode, v_bMultiSearch:=False)
        End If

        'DB 16/2/2000 End

        Return result
    End Function
    ' <-- END CL090699

    ' CL090699 BEGIN-->
    ' This wrapper avoids binary imcompatibility
    'Developer Guide No 101
    Public Function GetList(ByVal v_sPropertyId As String, ByRef r_vListData As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        m_oCommon.MaxListItems = m_lMaxListItems
        Return m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString)

    End Function
    ' <-- END CL090699

    Public Function PopulateListControl(ByVal v_sPropertyId As String, ByRef r_oControl As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        m_oCommon.MaxListItems = m_lMaxListItems

        Return m_oCommon.PopulateListControlC(v_sPropertyId:=v_sPropertyId, r_oControl:=r_oControl, v_vSearchString:=CStr(v_vSearchString))

    End Function

    Public Function CheckListVersions(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        m_oCommon.NoLogin = True

        Return m_oCommon.CheckListVersionsC(v_sGisDataModelCode:=v_sGisDataModelCode, v_sSellerCode:=v_sSellerCode)

    End Function
    ' ***************************************************************** '
    ' Name: GetDescription (Standard Method)
    '
    ' Description: Returns a desc for a given property idand ABI code.
    '
    ' CL090699
    '
    ' ***************************************************************** '
    Public Function GetDescription(ByVal sPropertyId As String, ByVal sABICodeTarget As String, ByRef sDescription As String) As Integer

        m_oCommon.MaxListItems = m_lMaxListItems
        Return m_oCommon.GetDescriptionC(sPropertyId:=sPropertyId, sABICodeTarget:=sABICodeTarget, sDescription:=sDescription)

    End Function


    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetVehicleStyleList(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try




            m_oCommon.MaxListItems = m_lMaxListItems

            result = m_oCommon.GetVehicleListI4M(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=v_lYear)

            'Cache list for other filters (engine capacity, trim, transmission, vehicle)
            m_vVehicleData = VB6.CopyArray(r_vListData)

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then

                dicMatches = New Hashtable()

                For Each r_vListData_item As Object In r_vListData

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If Not dicMatches.ContainsKey(oVehicleListLine.Style) Then
                        dicMatches.Add(oVehicleListLine.Style, oVehicleListLine.Style & "#" & oVehicleListLine.StyleCode)
                    End If

                    oVehicleListLine = Nothing

                Next r_vListData_item

                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))

            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.

            bPMFunc.LogMessage("", CStr(gPMConstants.PMELogLevel.PMLogError), CInt("Failed to GetVehicleStyleList in InterfaceNoLogin"), ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function PopulateArrayWithDictionaryItems(ByVal v_dicTmpMatches As Hashtable, ByRef r_vListData() As Object) As Integer

        Dim result As Integer = 0


        'Convert dictionary to array

        If v_dicTmpMatches.Count = 0 Then
            r_vListData = VB6.CopyArray(Nothing)
        Else

            ReDim r_vListData(v_dicTmpMatches.Count - 1)

            For lCntr As Integer = 0 To (v_dicTmpMatches.Count - 1)


                'Modified by Archana Tokas on 4/27/2010 10:32:11 AM changes done as per .net requirement
                'r_vListData(lCntr) = CStr(v_dicTmpMatches.Values(lCntr))
                r_vListData(lCntr) = CStr(v_dicTmpMatches.Contains(lCntr))
            Next lCntr

        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Public Function GetVehicleModelsByYear(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_vYear As Integer) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try




            m_oCommon.MaxListItems = m_lMaxListItems

            result = m_oCommon.GetVehicleListI4M(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vYear:=v_vYear)

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then

                dicMatches = New Hashtable()
                For Each r_vListData_item As Object In r_vListData

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If Not dicMatches.ContainsKey(oVehicleListLine.ModelName) Then
                        dicMatches.Add(oVehicleListLine.ModelName, oVehicleListLine.ModelName)
                    End If

                    oVehicleListLine = Nothing

                Next r_vListData_item
                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))

            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.

            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicleModelsByYear in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetVehicleEngineCapacity(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try


            m_oCommon.MaxListItems = m_lMaxListItems

            'Check to see if vehicle selection list has been cached
            If Not Information.IsArray(m_vVehicleData) Then
                result = m_oCommon.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=CStr(v_lYear), v_vDoors:=v_vStyleCode.Substring(0, 1))
            Else
                r_vListData = VB6.CopyArray(m_vVehicleData)
            End If

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then
                dicMatches = New Hashtable()
                For Each r_vListData_item As Object In r_vListData

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If oVehicleListLine.IsInFilter(v_vStyleCode:=v_vStyleCode) Then
                        If Not dicMatches.ContainsKey(oVehicleListLine.Capacity) Then
                            dicMatches.Add(oVehicleListLine.Capacity, oVehicleListLine.Capacity & "#" & oVehicleListLine.CapacityCode)
                        End If
                    End If

                    oVehicleListLine = Nothing
                Next r_vListData_item
                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))

            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicleEngineCapacity in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetVehicleTrim(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try

            Dim lCC As Integer



            m_oCommon.MaxListItems = m_lMaxListItems

            lCC = CInt(v_vEngineType.Substring(0, v_vEngineType.Length - 1))

            If Not Information.IsArray(m_vVehicleData) Then
                result = m_oCommon.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=CStr(v_lYear), v_vDoors:=v_vStyleCode.Substring(0, 1), v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1), v_vCC:=CStr(lCC))
            Else
                r_vListData = VB6.CopyArray(m_vVehicleData)
            End If

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then

                dicMatches = New Hashtable()
                For Each r_vListData_item As Object In r_vListData
                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If oVehicleListLine.IsInFilter(v_vStyleCode:=v_vStyleCode, v_vCC:=lCC, v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1)) Then
                        If Not dicMatches.ContainsKey(oVehicleListLine.TrimName(v_sModel)) Then
                            dicMatches.Add(oVehicleListLine.TrimName(v_sModel), oVehicleListLine.TrimName(v_sModel))
                        End If
                    End If

                    oVehicleListLine = Nothing
                Next r_vListData_item
                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.

            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicleTrim in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetTransmissionType(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String, ByVal v_vTrimName As String) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try

            Dim lCC As Integer


            lCC = CInt(v_vEngineType.Substring(0, v_vEngineType.Length - 1))

            m_oCommon.MaxListItems = m_lMaxListItems

            'Check if search results have been cached
            If Not Information.IsArray(m_vVehicleData) Then
                result = m_oCommon.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=CStr(v_lYear), v_vDoors:=v_vStyleCode.Substring(0, 1), v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1), v_vCC:=CStr(lCC))
            Else
                r_vListData = VB6.CopyArray(m_vVehicleData)
            End If

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then

                dicMatches = New Hashtable()
                For Each r_vListData_item As Object In r_vListData

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If Strings.Len(oVehicleListLine.TransmissionType) <> 0 Then
                        If oVehicleListLine.IsInFilter(v_vStyleCode:=v_vStyleCode, v_vCC:=lCC, v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1), v_vTrimName:=v_vTrimName, v_sModel:=v_sModel) Then
                            If Not dicMatches.ContainsKey(oVehicleListLine.TransmissionType) Then
                                dicMatches.Add(oVehicleListLine.TransmissionType, oVehicleListLine.TransmissionType & "#" & oVehicleListLine.TransmissionTypeCode)
                            End If
                        End If
                    End If

                    oVehicleListLine = Nothing
                Next r_vListData_item
                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                If Information.IsArray(r_vListData) Then
                    ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.
            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetTransmissionType in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetVehicle(ByRef r_vListData() As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String, ByVal v_vTrimName As String, ByVal v_vTransmissionType As String) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Dim dicMatches As Hashtable
        Try


            Dim lCC As Integer

            lCC = CInt(v_vEngineType.Substring(0, v_vEngineType.Length - 1))

            m_oCommon.MaxListItems = m_lMaxListItems

            'Check if search results have been cached
            If Not Information.IsArray(m_vVehicleData) Then
                result = m_oCommon.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=CStr(v_lYear), v_vDoors:=v_vStyleCode.Substring(0, 1), v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1), v_vCC:=CStr(lCC), v_vTransType:=v_vTransmissionType)
            Else
                r_vListData = VB6.CopyArray(m_vVehicleData)
            End If

            'Process the returned array to format the output
            If Information.IsArray(r_vListData) Then

                dicMatches = New Hashtable()

                For Each r_vListData_item As Object In r_vListData

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(r_vListData_item))

                    'Check if value already exists, if not add to results
                    If oVehicleListLine.IsInFilter(v_vStyleCode:=v_vStyleCode, v_vCC:=lCC, v_vFuelType:=v_vEngineType.Substring(v_vEngineType.Length - 1), v_vTrimName:=v_vTrimName, v_sModel:=v_sModel, v_vTransmissionTypeCode:=v_vTransmissionType) Then

                        If Not dicMatches.ContainsKey(oVehicleListLine.DisplayLine) Then
                            dicMatches.Add(oVehicleListLine.DisplayLine, oVehicleListLine.DisplayLine)
                        End If
                    End If

                    oVehicleListLine = Nothing
                Next r_vListData_item

                m_lReturn = CType(PopulateArrayWithDictionaryItems(dicMatches, r_vListData), gPMConstants.PMEReturnCode)
                dicMatches = Nothing

                ShellSort(r_vListData, r_vListData.GetLowerBound(0), r_vListData.GetUpperBound(0))


            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            'Garbage Collection
            dicMatches = Nothing
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.

            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicle in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Public Function GetVehicleModelByID(ByRef r_sListData As String, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer, ByVal v_lVehicleID As Integer) As Integer

        Dim result As Integer = 0
        Dim oVehicleListLine As cVehicleListLine
        Try

            Dim vListData As Object



            m_oCommon.MaxListItems = m_lMaxListItems


            result = m_oCommon.GetVehicleListI4M(r_vListData:=vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_sModel, v_vYear:=v_lYear)

            'Process the returned array to format the output
            If Information.IsArray(vListData) Then


                For lCntr As Integer = vListData.GetLowerBound(0) To vListData.GetUpperBound(0)

                    oVehicleListLine = New cVehicleListLine()

                    oVehicleListLine.Load(CStr(vListData(lCntr)))
                    If oVehicleListLine.DisplayLine.IndexOf(CStr(v_lVehicleID)) + 1 Then
                        '--- Make delimited string for output
                        With oVehicleListLine
                            r_sListData = .StyleCode & _
                                          "#" & .CapacityCode & _
                                          "#" & .TrimName(v_sModel) & _
                                          "#" & .TransmissionTypeCode & _
                                          "#" & .DisplayLine
                            Exit For
                        End With
                    End If

                    oVehicleListLine = Nothing

                Next

            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            'Garbage Collection
            oVehicleListLine = Nothing

            Return result

        Catch ex As Exception



            ' Error Section.
            bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, "Failed to GetVehicleModelbyID in InterfaceNoLogin", ACApp, ACClass, "GetVehicleModelsByYear", excep:=ex)

            'Garbage Collection

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

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
        ' Error Section.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Wrapper entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

