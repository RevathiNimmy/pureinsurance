SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_Get_Debtor_User_Groups'
GO

CREATE PROCEDURE spu_Get_Debtor_User_Groups
    @GroupType varchar(20),
    @SourceID   int

/* Get the steps for the GroupType, and Branch, which are not deleted, have
     effective date < = today, and source date = @sourceId and step_number 
     is NOT NULL and <> 0 */

AS
SELECT     Debtor_User_Groups.Step_Number, Debtor_User_Groups.pmuser_group_id
FROM         Debtor_User_Groups INNER JOIN
                    Debtor_User_Groups_Type ON 
                    Debtor_User_Groups.debtor_user_groups_type_id = Debtor_User_Groups_Type.debtor_user_groups_type_id
WHERE     (Debtor_User_Groups.is_deleted = 0) AND 
                (Debtor_User_Groups_Type.description =@GroupType) AND
                (Debtor_User_Groups.source_id =@SourceID) AND 
                (Debtor_User_Groups.effective_date <= GETDATE())  AND 
               (Debtor_User_Groups.Step_Number IS NOT NULL OR  Debtor_User_Groups.Step_Number <> 0)
               
GO
