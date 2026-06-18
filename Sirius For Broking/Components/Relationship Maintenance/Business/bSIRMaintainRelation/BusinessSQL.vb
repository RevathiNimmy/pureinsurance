Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	
	Private Const ACClass As String = "BusinessSQL"
    'Developer Guide No.39
    Public Const ACAddSQL As String = "spu_Relationship_Type_add"
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "RelationshipAdd"

    'Developer Guide No.39
    Public Const ACUpdateSQL As String = "spu_Relationship_Type_update"
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "RelationshipUpdate"
    'Developer Guide No.39
    Public Const ACGetCaptionSQL As String = "spu_pm_caption_id_return"
	Public Const ACGetCaptionStored As Boolean = True
	Public Const ACGetCaptionName As String = "GetCaptionID"
End Module