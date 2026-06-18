Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module DMEditorSharedFuncs

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "DMEditorSharedFuncs"
    'Private m_oDefaultGisLists As Collection
    Private m_oDefaultGisLists As Hashtable

    ' ***************************************************************** '
    '
    ' Name: LoadGisLists
    '
    ' Description:
    '
    ' History: 21/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function LoadGisLists(ByRef cboGISListId As ComboBox, ByVal sGisDataModel As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACListId As Integer = 0
            Const ACListName As Integer = 1
            Dim oListManager As iGISListManager.InterfaceNoLogin
            Dim vListHeader(,) As Object
            Dim sListName As String

            cboGISListId.Items.Clear()

            'Assemble the list of default lists
            m_lReturn = CType(AssembleDefaultLists(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssembleDefaultLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists")
                Return result
            End If

            oListManager = New iGISListManager.InterfaceNoLogin()

            m_lReturn = CType(oListManager.CheckListVersions(sGisDataModel), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(AddDefaultLists(cboGISListId), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDefaultLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists")
                    Return result
                End If
                oListManager.Dispose()
                oListManager = Nothing
                m_oDefaultGisLists = Nothing
                '        LoadGisLists = PMError
                '        LogMessage _
                ''            iType:=PMLogOnError, _
                ''            sMsg:="m_oListManager.CheckListVersions Failed", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="LoadGisLists"
                '        m_lReturn = oListManager.Terminate
                '        Set oListManager = Nothing
                Return result
            End If

            m_lReturn = oListManager.GetListIdsAndNames(r_vResultArray:=vListHeader)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListManager.GetListIdsAndNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists")
                oListManager.Dispose()
                oListManager = Nothing
                m_oDefaultGisLists = Nothing
                Return result
            End If

            If Not Information.IsArray(vListHeader) Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No lists found", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists")
                oListManager.Dispose()
                oListManager = Nothing
                m_oDefaultGisLists = Nothing
                Return result
            End If

            'fix up header names if they werent in the file (names in file is new)

            For lCnt As Integer = 0 To vListHeader.GetUpperBound(1)

                sListName = CStr(vListHeader(ACListName, lCnt))

                vListHeader(ACListId, lCnt) = CStr(vListHeader(ACListId, lCnt)).Trim()
                If sListName <> "" AndAlso Strings.Asc(sListName(0)) = 0 Then

                    m_lReturn = CType(GetDefaultListName(v_sListid:=CStr(vListHeader(ACListId, lCnt)), r_sListName:=sListName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultListName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists")
                        Return result
                    End If

                    vListHeader(ACListName, lCnt) = sListName
                End If
            Next lCnt

            'sort list headers into alphabetical order
            QuicksortArrays(vListHeader, vListHeader.GetLowerBound(1), vListHeader.GetUpperBound(1), 1)

            'add headers/list ids to drop list

            For lCnt As Integer = 0 To vListHeader.GetUpperBound(1)
                Dim cboGISListId_NewIndex As Integer = -1

                cboGISListId_NewIndex = cboGISListId.Items.Add(CStr(vListHeader(ACListName, lCnt)))

                VB6.SetItemData(cboGISListId, cboGISListId_NewIndex, Conversion.Val(CStr(vListHeader(ACListId, lCnt))))
            Next lCnt


            oListManager.Dispose()
            oListManager = Nothing
            m_oDefaultGisLists = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadGisLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGisLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: AssembleDefaultLists
    '
    ' Description:
    '
    ' History: 21/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AssembleDefaultLists() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACListId As Integer = 0
            Const ACListName As Integer = 1

            Dim vArray(1) As Object

            'm_oDefaultGisLists = New Collection()
            m_oDefaultGisLists = New Hashtable()


            vArray(ACListName) = PMBConst.PMBListAlcoholText

            vArray(ACListId) = PMBConst.PMBListAlcoholCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListAlcoholCode, PMBConst.PMBListAlcoholText})


            vArray(ACListName) = PMBConst.PMBListBusinessText

            vArray(ACListId) = PMBConst.PMBListBusinessCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListBusinessCode, PMBConst.PMBListBusinessText})



            vArray(ACListName) = PMBConst.PMBListConvSentenceText

            vArray(ACListId) = PMBConst.PMBListConvSentenceCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListConvSentenceCode, PMBConst.PMBListConvSentenceText})



            vArray(ACListName) = PMBConst.PMBListConvStatusText

            vArray(ACListId) = PMBConst.PMBListConvStatusCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListConvStatusCode, PMBConst.PMBListConvStatusText})



            vArray(ACListName) = PMBConst.PMBListConvTypeText

            vArray(ACListId) = PMBConst.PMBListConvTypeCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListConvTypeCode, PMBConst.PMBListConvTypeText})



            vArray(ACListName) = PMBConst.PMBListEmploymentText

            vArray(ACListId) = PMBConst.PMBListEmploymentCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListEmploymentCode, PMBConst.PMBListEmploymentText})



            vArray(ACListName) = PMBConst.PMBListGenderText

            vArray(ACListId) = PMBConst.PMBListGenderCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListGenderCode, PMBConst.PMBListGenderText})


            vArray(ACListName) = PMBConst.PMBListMaritalStatusText

            vArray(ACListId) = PMBConst.PMBListMaritalStatusCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListMaritalStatusCode, PMBConst.PMBListMaritalStatusText})


            vArray(ACListName) = PMBConst.PMBListOccupationText

            vArray(ACListId) = PMBConst.PMBListOccupationCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListOccupationCode, PMBConst.PMBListOccupationText})



            vArray(ACListName) = PMBConst.PMBListPaymentMethodText

            vArray(ACListId) = PMBConst.PMBListPaymentMethodCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListPaymentMethodCode, PMBConst.PMBListPaymentMethodText})



            vArray(ACListName) = PMBConst.PMBListTimeUnitText

            vArray(ACListId) = PMBConst.PMBListTimeUnitCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListTimeUnitCode, PMBConst.PMBListTimeUnitText})



            vArray(ACListName) = PMBConst.PMBListTitleText

            vArray(ACListId) = PMBConst.PMBListTitleCode

            'm_oDefaultGisLists.Add(vArray, CStr(vArray(ACListId)))
            m_oDefaultGisLists.Add(CStr(vArray(ACListId)), New Object() {PMBConst.PMBListTitleCode, PMBConst.PMBListTitleText})


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssembleDefaultLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AssembleDefaultLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddDefaultLists
    '
    ' Description:
    '
    ' History: 21/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function AddDefaultLists(ByRef cboGISListId As ComboBox) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Const ACListId As Integer = 0
            ' Const ACListName As Integer = 1


            'For Each vArray As Object In m_oDefaultGisLists
            For Each vArray As DictionaryEntry In m_oDefaultGisLists
                Dim cboGISListId_NewIndex As Integer = -1

                'cboGISListId_NewIndex = cboGISListId.Items.Add(CStr(vArray(ACListName)))

                'VB6.SetItemData(cboGISListId, cboGISListId_NewIndex, CInt(vArray(ACListId)))
                cboGISListId_NewIndex = cboGISListId.Items.Add(New VB6.ListBoxItem(CStr(vArray.Value(1)), CInt(vArray.Key)))

            Next vArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDefaultLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDefaultLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function




    ' ***************************************************************** '
    ' Name: GetDefaultListName
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetDefaultListName(ByVal v_sListid As String, ByRef r_sListName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACListName As Integer = 1
            Dim vArray As Object

            vArray = m_oDefaultGisLists(v_sListid)
            If Not IsNothing(vArray) Then

                If vArray IsNot Nothing AndAlso CStr(vArray(ACListName)).Trim() <> "" Then

                    r_sListName = CStr(vArray(ACListName)).Trim()
                Else
                    r_sListName = v_sListid
                End If
            End If
            Return result

        Catch excep As System.Exception

            If Information.Err().Number = 5 Or Information.Err().Number = 9 Then
                r_sListName = v_sListid
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultListName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultListName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function



    ' Sort the items in array values() with bounds min and max.
    ' converted to use column to indicate the column on which to do the comparison by CLG
    Public Sub QuicksortArrays(ByRef list(,) As Object, ByVal min As Integer, ByVal max As Integer, ByVal column As Integer)

        Dim med_value(2) As Object
        Dim hi, lo, i As Integer

        Try

            ' If the list has no more than 1 element, it's sorted.
            If min >= max Then Exit Sub

            ' Pick a dividing item.
            i = CSng(Math.Floor((max - min + 1) * VBMath.Rnd() + min))


            med_value(0) = list(0, i)


            med_value(1) = list(1, i)

            ' Swap it to the front so we can find it easily.


            list(0, i) = list(0, min)


            list(1, i) = list(1, min)


            ' Move the items smaller than this into the left
            ' half of the list. Move the others into the right.
            lo = min
            hi = max
            Do
                ' Look down from hi for a value < med_value.



                Do While list(column, hi) >= med_value(column)
                    hi -= 1
                    If hi <= lo Then Exit Do
                Loop
                If hi <= lo Then


                    list(0, lo) = med_value(0)


                    list(1, lo) = med_value(1)
                    Exit Do
                End If

                ' Swap the lo and hi values.


                list(0, lo) = list(0, hi)


                list(1, lo) = list(1, hi)

                ' Look up from lo for a value >= med_value.
                lo += 1



                Do While list(column, lo) < med_value(column)
                    lo += 1
                    If lo >= hi Then Exit Do
                Loop
                If lo >= hi Then
                    lo = hi


                    list(0, hi) = med_value(0)


                    list(1, hi) = med_value(1)
                    Exit Do
                End If

                ' Swap the lo and hi values.


                list(0, hi) = list(0, lo)


                list(1, hi) = list(1, lo)
            Loop

            ' Sort the two sublists
            QuicksortArrays(list, min, lo - 1, column)
            QuicksortArrays(list, lo + 1, max, column)

        Catch




        End Try

    End Sub
End Module
