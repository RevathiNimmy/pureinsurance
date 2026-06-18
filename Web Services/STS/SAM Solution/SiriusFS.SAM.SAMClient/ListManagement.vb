Option Strict On

#Region " Imports"
' Custom Imports
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports System.Web.Caching
Imports System.Data.SqlClient
Imports System.IO
#End Region


Public Class ListManagement

    Private _oCache As System.Web.Caching.Cache
    Protected Const EffDateKey As String = "EffectiveDate"

#Region " New"

    Public Sub New(ByRef oCache As System.Web.Caching.Cache)

        Dim EffectiveDate As Date
        Dim EffDateFile As String

        If (oCache Is Nothing) Then
            Throw New Exception("No Instance of Cache Supplied")
        Else
            _oCache = oCache
        End If

        ' Try and get the Effective Date from the Cache
        EffectiveDate = CType(_oCache("EffectiveDate"), Date)

        ' Do we have the value
        If EffectiveDate < #1/1/1899# Then

            ' No so build the Path
            'EffDateFile = Directory.GetCurrentDirectory + "\EffectiveDate.txt"
            EffDateFile = System.Configuration.ConfigurationSettings.AppSettings("SourceLocation")
            EffDateFile = CType(IIf(Right(EffDateFile, 1) = "\", EffDateFile, EffDateFile & "\"), String)
            EffDateFile = EffDateFile & "EffectiveDate.txt"

            ' Try to read it from the File
            Try
                EffectiveDate = CType(ReadTextFile(EffDateFile), Date)
            Catch
                EffectiveDate = Now
            End Try

            ' Add the EffectiveDate with a Dependency on the File
            _oCache.Insert(EffDateKey, EffectiveDate, New CacheDependency(EffDateFile))

        End If

    End Sub
#End Region

#Region " GetPMFullUDL"

    Public Function GetPMFullUDL(ByVal PMTableName As String) As DataView

        ' SqlDataReader that will hold the returned results		
        Dim ds As DataSet
        Dim dv As DataView
        Dim EffectiveDate As DateTime
        Dim dependencyKey(0) As String
        Dim CacheKey As String

        ' Append "_Effective" because we will have two Lists for this Tablename,
        ' the effective list and the deleted list.
        CacheKey = PMTableName + "_Effective"

        ds = CType(_oCache(CacheKey), DataSet)

        If ds Is Nothing Then

            Dim oSTS As New STSListManagement.ListManagement()

            EffectiveDate = CType(_oCache(EffDateKey), Date)

            ds = oSTS.GetPMUserDefListEffective(PMTableName, EffectiveDate)

            If (Not ds Is Nothing) Then

                ds.Tables(0).DefaultView.AllowEdit = False

                ' Make the List Depenent on the Effective Date
                dependencyKey(0) = EffDateKey

                ' Create a new depency
                Dim dependency As New CacheDependency(Nothing, dependencyKey)

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, ds, dependency)

            End If

        End If

        ' Regardless of where we got it from, return the DefaultView from the First DataTable
        Return ds.Tables(0).DefaultView

    End Function
#End Region

#Region " GetPMFullUDLDeleted"

    Private Function GetPMFullUDLDeleted(ByVal PMTableName As String) As DataView

        ' SqlDataReader that will hold the returned results		
        Dim ds As DataSet
        Dim dv As DataView
        Dim dependencyKey(0) As String
        Dim CacheKey As String

        ' Append "_Deleted" because we will have two Lists for this Tablename,
        ' the effective list and the deleted list.
        CacheKey = PMTableName + "_Deleted"

        ds = CType(_oCache(CacheKey), DataSet)

        If ds Is Nothing Then

            Dim oSTS As New STSListManagement.ListManagement()

            ds = oSTS.GetPMUserDefListDeleted(PMTableName)

            If (Not ds Is Nothing) Then

                ds.Tables(0).DefaultView.AllowEdit = False

                ' Make the List Depenent on the Effective Date
                dependencyKey(0) = EffDateKey

                ' Create a new depency
                Dim dependency As New CacheDependency(Nothing, dependencyKey)

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, ds, dependency)

            End If

        End If

        ' Regardless of where we got it from, return the DefaultView from the First DataTable
        Return ds.Tables(0).DefaultView

    End Function
#End Region

#Region " GetPMSingleUDLByID"

    Public Function GetPMSingleUDLByID(ByVal PMTableName As String, ByVal ID As Int32) As DataView

        ' Data View that will hold the returned results		
        Dim dv As DataView
        Dim dvID As DataView
        Dim sFilter As String

        ' Get the DataView For the table
        dv = GetPMFullUDL(PMTableName)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If dv Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for PMTable : " + PMTableName)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Specify the Filter
        sFilter = PMTableName + "_ID = " + ID.ToString

        ' Create a New Data View filtered by the ID
        dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        ' The ID might not still be effective
        If dvID Is Nothing Then

            ' Not Found so look in the Deleted Items for the same Table Name

            ' Get the DataView For the table
            dv = GetPMFullUDLDeleted(PMTableName)

            ' If we have not get the data set, either directly from the database
            ' or from the cache then error.
            If dv Is Nothing Then
                Dim ex As New Exception("No Deleted List retrieved for PMTable : " + PMTableName)
                ExceptionManager.Publish(ex)
                Throw ex
            End If

            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        End If

        If dvID Is Nothing Then
            Dim ex As New Exception("No Effective or Deleted row found for PMTable : " + PMTableName + " ID : " + ID.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Return the New Data View filtered by the ID Supplied
        Return dvID

    End Function
#End Region

#Region " GetPMSingleUDLByCode"

    Public Function GetPMSingleUDLByCode(ByVal PMTableName As String, ByVal Code As String) As DataView

        ' Data View that will hold the returned results		
        Dim dv As DataView
        Dim dvID As DataView
        Dim sFilter As String

        ' Get the DataView For the table
        dv = GetPMFullUDL(PMTableName)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If dv Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for PMTable : " + PMTableName)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Specify the Filter
        sFilter = "Code = '" + Code + "'"

        ' Create a New Data View filtered by the ID
        dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        ' The ID might not still be effective
        If dvID Is Nothing Then

            ' Not Found so look in the Deleted Items for the same Table Name

            ' Get the DataView For the table
            dv = GetPMFullUDLDeleted(PMTableName)

            ' If we have not get the data set, either directly from the database
            ' or from the cache then error.
            If dv Is Nothing Then
                Dim ex As New Exception("No Deleted List retrieved for PMTable : " + PMTableName)
                ExceptionManager.Publish(ex)
                Throw ex
            End If

            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        End If

        If dvID Is Nothing Then
            Dim ex As New Exception("No Effective or Deleted row found for PMTable : " + PMTableName + " Code : " + Code)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Return the New Data View filtered by the ID Supplied
        Return dvID

    End Function
#End Region


#Region " GetGISFullUDL"

    Public Function GetGISFullUDL(Optional ByVal GisUserDefHeaderID As Int32 = -1, _
                                  Optional ByVal GisUserDefHeaderCode As String = "") As DataView

        ' SqlDataReader that will hold the returned results		
        Dim ds As DataSet
        Dim dv As DataView
        Dim dependencyKey(0) As String
        Dim EffectiveDate As Date
        Dim CacheKey As String

        If (GisUserDefHeaderID = -1 And GisUserDefHeaderCode = "") Then
            Dim ex As New Exception("You must supply an ID or Code for a list to retrieve.")
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Append "_Effective" because we will have two Lists for this Tablename,
        ' the effective list and the deleted list.
        If (GisUserDefHeaderID <> -1) Then
            CacheKey = GisUserDefHeaderID.ToString + "_Effective"
        Else
            CacheKey = GisUserDefHeaderCode + "_Effective"
        End If

        ds = CType(_oCache(CacheKey), DataSet)

        If ds Is Nothing Then

            Dim oSTS As New STSListManagement.ListManagement()

            EffectiveDate = CType(_oCache(EffDateKey), Date)
            
            ds = oSTS.GetGISUserDefListEffective( _
                                GisUserDefHeaderID:=GisUserDefHeaderID, _
                                GisUserDefHeaderCode:=GisUserDefHeaderCode, _
                                EffectiveDate:=EffectiveDate)

            If (Not ds Is Nothing) Then

                ds.Tables(0).DefaultView.AllowEdit = False

                ' Make the List Depenent on the Effective Date
                dependencyKey(0) = EffDateKey

                ' Create a new depency
                Dim dependency As New CacheDependency(Nothing, dependencyKey)

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, ds, dependency)

            End If

        End If

        ' Regardless of where we got it from, return the DefaultView from the First DataTable
        Return ds.Tables(0).DefaultView

    End Function
#End Region

#Region " GetGISSingleUDLByCode"

    Public Function GetGISSingleUDLByCode(ByVal GisUserDefHeaderID As Int32, ByVal Code As String) As DataView

        ' Data View that will hold the returned results		
        Dim dv As DataView
        Dim dvID As DataView
        Dim sFilter As String

        If (Code = "") Then
            Return Nothing
        End If

        ' Get the DataView For the table
        dv = GetGISFullUDL(GisUserDefHeaderID)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If dv Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for GISUserDefHeaderID : " + GisUserDefHeaderID.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Specify the Filter
        sFilter = "Code = " + Code

        ' Create a New Data View filtered by the ID
        dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        ' The ID might not still be effective
        If dvID Is Nothing Then

            ' Not Found so look in the Deleted Items for the same Table Name

            ' Get the DataView For the table
            dv = GetGISFullUDLDeleted(GisUserDefHeaderID)

            ' If we have not get the data set, either directly from the database
            ' or from the cache then error.
            If dv Is Nothing Then
                Dim ex As New Exception("No Deleted List retrieved for GISUserDefheaderID : " + GisUserDefHeaderID.ToString)
                ExceptionManager.Publish(ex)
                Throw ex
            End If

            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        End If

        If dvID Is Nothing Then
            Dim ex As New Exception("No Effective or Deleted row found for GISUserDefheaderID : " + GisUserDefHeaderID.ToString + " Code : " + Code)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Return the New Data View filtered by the ID Supplied
        Return dvID

    End Function

#End Region

#Region " GetGISSingleUDLByCode"

    Public Function GetGISSingleUDLById(ByVal GisUserDefHeaderID As Int32, ByVal Code As String) As DataView

        ' Data View that will hold the returned results		
        Dim dv As DataView
        Dim dvID As DataView
        Dim sFilter As String

        If (Code = "") Then
            Return Nothing
        End If

        ' Get the DataView For the table
        dv = GetGISFullUDL(GisUserDefHeaderID)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If dv Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for GISUserDefHeaderID : " + GisUserDefHeaderID.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Specify the Filter
        sFilter = "gis_user_def_detail_id = " + Code

        ' Create a New Data View filtered by the ID
        dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        ' The ID might not still be effective
        If dvID Is Nothing Then

            ' Not Found so look in the Deleted Items for the same Table Name

            ' Get the DataView For the table
            dv = GetGISFullUDLDeleted(GisUserDefHeaderID)

            ' If we have not get the data set, either directly from the database
            ' or from the cache then error.
            If dv Is Nothing Then
                Dim ex As New Exception("No Deleted List retrieved for GISUserDefheaderID : " + GisUserDefHeaderID.ToString)
                ExceptionManager.Publish(ex)
                Throw ex
            End If

            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

        End If

        If dvID Is Nothing Then
            Dim ex As New Exception("No Effective or Deleted row found for GISUserDefheaderID : " + GisUserDefHeaderID.ToString + " Code : " + Code)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Return the New Data View filtered by the ID Supplied
        Return dvID

    End Function

#End Region

#Region " GetGISFullUDLDeleted"

    Private Function GetGISFullUDLDeleted(ByVal GisUserDefHeaderID As Int32) As DataView

        ' SqlDataReader that will hold the returned results		
        Dim ds As DataSet
        Dim dv As DataView
        Dim dependencyKey(0) As String
        Dim EffectiveDate As Date
        Dim CacheKey As String

        ' Append "_Effective" because we will have two Lists for this Tablename,
        ' the effective list and the deleted list.
        CacheKey = GisUserDefHeaderID.ToString + "_Deleted"

        ds = CType(_oCache(CacheKey), DataSet)

        If ds Is Nothing Then

            Dim oSTS As New STSListManagement.ListManagement()

            ds = oSTS.GetGISUserDefListDeleted(GisUserDefHeaderID)

            If (Not ds Is Nothing) Then

                ds.Tables(0).DefaultView.AllowEdit = False

                ' Make the List Depenent on the Effective Date
                dependencyKey(0) = EffDateKey

                ' Create a new depency
                Dim dependency As New CacheDependency(Nothing, dependencyKey)

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, ds, dependency)

            End If

        End If

        ' Regardless of where we got it from, return the DefaultView from the First DataTable
        Return ds.Tables(0).DefaultView

    End Function
#End Region

#Region " Private Functions"
    Private Function ReadTextFile(ByVal path As String) As String
        Dim sr As System.IO.StreamReader
        Dim Contents As String
        sr = New StreamReader(path)
        Contents = sr.ReadToEnd()
        sr.Close()
        Return Contents
    End Function
#End Region


End Class



