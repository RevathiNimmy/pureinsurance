Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	
	'**********
	' Returns the overdue tasks
	Public Const ACGetOverdueTasksSQL As String = "{call spu_get_overdue_tasks}"
	Public Const ACGetOverdueTasksName As String = "Get overdue tasks"
	'**********
	'Creates a task alerting supervisor that user has outstanding task
	Public Const ACCreateCheckTaskSQL As String = "{call spu_create_check_task (?,?,?,?)}"
	Public Const ACCreateCheckTaskName As String = "Create check task"
	'**********
	' Select Useretails
	Public Const ACSelectUserDetailsStored As Boolean = False
	Public Const ACSelectUserDetailsName As String = "SelectUserId"
	Public Const ACSelectUserDetailsSQL As String = "SELECT user_id,language_id FROM PMUser where username = {username} and password = {password}"
End Module