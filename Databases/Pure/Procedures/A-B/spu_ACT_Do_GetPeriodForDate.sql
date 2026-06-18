SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_GetPeriodForDate'
GO


CREATE PROCEDURE spu_ACT_Do_GetPeriodForDate
    @company_id int,  
    @date_in_period datetime,  
    @sub_branch_id int=NULL,
	@include_closed int=0  
AS  
  
/* DD 23/08/2002 */  
/* Get the Product Option for multi-tree accounting */  
DECLARE @Value VARCHAR(20)  
SELECT  
    @Value=Value  
FROM  
    Hidden_options  
WHERE  
    option_number=16  
  
/*  
    If Null/0 then there is only one tree.  
    Hardcoded for performance reasons  
*/  
IF @Value IS NULL OR @Value=0  
BEGIN  
    SELECT @company_id=1  
    SELECT @sub_branch_id=1  
END  
ELSE  
BEGIN  
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT  
END  
  
SELECT period_id,  
       year_name,  
       period_name  
FROM   Period  
WHERE  sub_branch_id = @sub_branch_id  
AND    period_end_date =  
       (SELECT min (period_end_date)  
        FROM   Period  
        WHERE  sub_branch_id=@sub_branch_id and  
               period_end_date >= @date_in_period  
    AND (period_end_complete=0 or @include_closed=1))  
GO
