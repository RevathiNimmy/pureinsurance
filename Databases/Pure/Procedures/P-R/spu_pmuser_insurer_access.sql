Execute DDLDropProcedure 'spu_pmuser_insurer_access'
GO
Create Procedure spu_pmuser_insurer_access
@user_id int
As
   SELECT distinct USER_ID FROM PMUSER_GROUP_USER 
   WHERE PMUSER_GROUP_ID in (
				Select DISTINCT PMUSER_GROUP_ID FROM PMUSER_GROUP_ACTIVITY WHERE PMWRK_TASK_GROUP_ID IN 
                                 (
                                   Select PMWRK_TASK_GROUP_ID from PMWRK_TASK_GROUP_TASK Where PMWRK_TASK_ID in (28,55)
                                 )
                            )
    AND USER_ID=@user_id
GO