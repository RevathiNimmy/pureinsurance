Option Strict Off
Option Explicit On
Imports System
Module PMUserSourceSQL
    ' ***************************************************************** '
    ' Module: PMUserSourceSQL
    '
    ' Date: 14th April 2000
    '
    ' Description: Contains the SQL Statements to manipulate the
    '              PMUser_Source table
    '
    ' Edit History:
    '
    ' DAK190600 - only get sources that are not deleted
    ' ***************************************************************** '

    'SQL Statements

    ' Select PMUser_Source SQL
    Public Const ACSelectPMUserSourceStored As Boolean = False
    Public Const ACSelectPMUserSourceName As String = "SelectPMUserSource"
    'DAK220500
    'DAK190600
    Public Const ACSelectPMUserSourceSQL As String = "SELECT s.source_id, s.[code], s.[description], s.country_id " & _
                                                     "FROM   source s " & _
                                                     "WHERE  source_id NOT IN " & _
                                                     "(SELECT source_id FROM pmuser_source WHERE [user_id] = {user_id})" & _
                                                     " AND is_deleted = 0   AND (source_id IN (SELECT source_id from Product_Source WHERE product_id={product_id}) OR {product_id} = 0) ORDER BY s.[description]"

    'Alix - 28/11/2002 - Issue 1445

    Public Const ACSelectPMUserSourceIncDeletedSQL As String = "SELECT s.source_id, s.[code], " & _
                                                               "CASE is_deleted WHEN 1 THEN Rtrim(s.[description]) + ' (Closed)' " & _
                                                               "WHEN 0 THEN s.[description] " & _
                                                               "END [description], s.country_id " & _
                                                               "FROM   source s " & _
                                                               "WHERE  source_id NOT IN " & _
                                                               "(SELECT source_id FROM pmuser_source WHERE [user_id] = {user_id})   AND (source_id IN (SELECT source_id from Product_Source WHERE product_id={product_id}) OR {product_id} = 0) ORDER BY s.[description]"

    ' Add PMUser_Source SQL
    Public Const ACAddPMUserSourceStored As Boolean = False
    Public Const ACAddPMUserSourceName As String = "AddPMUserSource"
    Public Const ACAddPMUserSourceSQL As String = "INSERT INTO PMUser_Source (user_id, source_id) " & _
                                                  "VALUES( {user_id} , {source_id} )"

    ' Delete PMUser_Source SQL
    Public Const ACDeletePMUserSourceStored As Boolean = False
    Public Const ACDeletePMUserSourceName As String = "DeletePMUserSource"
    Public Const ACDeletePMUserSourceSQL As String = "DELETE FROM PMUser_Source " & _
                                                     "WHERE user_id = {user_id} " & _
                                                     "AND source_id = {source_id} "
End Module