set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Diary'
go
-- NON-GENERIC REPORT FOR SPECIFIC CUSTOMER. DO NOT USE THIS AS A BASIS
-- FOR LINKING OR CREATING NEW VIEWS.
create view Diary as select
    WTI.pmwrk_task_instance_cnt     WorkItemID,
    WTI.description                 TaskDescription,
    WT.description                  WorkTaskDescription,
    WTG.description                 TaskGroup,
    CB.username                     CreatedBy,
    WTI.date_created                DateCreated,
    WTI.task_due_date               DueDate,
    (case WTI.task_status
        when 0 then 'New'
        when 1 then 'In Progress'
        when 2 then 'Incomplete'
        when 3 then 'Complete'
        else null
        end)                        TaskStatus,
    (case WTI.is_urgent
        when 0 then 'No'
        when 1 then 'Yes'
        else null
        end)                        IsUrgent,
    UG.description                  UserGroup,
    U.username                      UserName,
    WTI.last_modified               LastModified,
    MB.username                     ModifiedBy,
    WTI.customer                    Customer
    from PMWrk_Task_Instance as WTI
    inner join PMWrk_Task_Group as WTG on WTI.pmwrk_task_group_id = WTG.pmwrk_task_group_id
    inner join PMWrk_Task as WT on WTI.pmwrk_task_id = WT.pmwrk_task_id
    left outer join PMUser_group as UG on WTI.pmuser_group_id = UG.pmuser_group_id
    left outer join PMUser as U on WTI.user_id = U.user_id
    left outer join PMUser as CB on WTI.Created_by_id = CB.User_id
    left outer join PMUser as MB on WTI.Modified_By_id = MB.User_id
go
