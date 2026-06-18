SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetPolicynumber'
GO


CREATE PROCEDURE spu_GetPolicynumber
    @EventCnt integer
AS

/*
- 07/12/2000 Tinny
- put in option for underwriting system
*/
--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 
--to ensure unique record
DECLARE @underwriting_broking char(1)

SELECT @underwriting_broking = value from hidden_options 
	where branch_id = 1 and option_number = 1

IF @underwriting_broking = 'U'
    SELECT @EventCnt
ELSE

    select distinct Insurance_File_cnt
    from Event_Log
    where Event_cnt = @EventCnt
GO


