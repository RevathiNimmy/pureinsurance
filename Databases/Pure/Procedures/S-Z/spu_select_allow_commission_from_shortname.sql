
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_select_allow_commission_from_shortname'
GO
Create Procedure spu_select_allow_commission_from_shortname
    @party_shortname varchar(60),
    @allow_commission_flag int OUTPUT
AS
    SELECT  @allow_commission_flag =ISNULL(allow_consolidated_commission,0) from party_agent 
    where party_cnt=(SELECT party_cnt  
    FROM Party  
    WHERE shortname = @party_shortname)
