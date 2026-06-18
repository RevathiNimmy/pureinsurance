Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	
	'**********
	' Returns the overdue tasks details for those task applicable for batch processing
	Public Const ACGetOverdueBatchTasksSQL As String = "{call spu_PM_Batch_Get_Overdue_Tasks}"
	Public Const ACGetOverdueBatchTasksName As String = "Return the overdue tasks applicable for batch processing"
	'**********
	
	'**********
	' updates the specified tasks outcome
	Public Const ACUpdateOverdueTaskOutcomesSQL As String = "{call spu_PM_Batch_Update_Overdue_Tasks(?,?,?)}"
	Public Const ACUpdateOverdueTaskOutcomesName As String = "updates the specified tasks outcome"
	'**********
End Module