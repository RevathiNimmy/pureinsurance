SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_reserve'
GO

CREATE PROCEDURE spu_add_reserve    
    @reserveid int OUTPUT,    
    @perilid int,    
    @reservetypeid int,    
    @siriusproduct char(5),    
    @policyid numeric,    
    @riskid numeric,    
    @periltypeid int    
AS    
    
--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
--    
--*******************************************************************************************    
Declare @suminsured currency    
Declare @mainreserve numeric    
Declare @count numeric    
DECLARE @version_id int    
    
EXEC spu_CLM_Get_Claim_Version @claim_peril_id = @perilid,    
     @version_id = @version_id OUTPUT    
    
IF @siriusproduct='A'    
    BEGIN    
        Select @suminsured=0    
    
        Insert into Reserve(    
         claim_peril_id,    
         reserve_type_id,    
         sum_insured,    
  version_id)    
 values(    
         @perilid,    
         @reservetypeid,    
         @suminsured,    
  @version_id    
 )    
    END    
ELSE    
    BEGIN    
        SELECT  @mainreserve=reserve_type_id    
        FROM    peril_type_reserve_type    
        WHERE   Peril_type_id In    
                (    
                SELECT peril_type_id    
                FROM claim_peril    
                WHERE claim_peril_id=@perilid    
                )    
        AND is_main_reserve=1    
    
        IF @mainreserve = @reservetypeid    
            SELECT  @suminsured = sum_insured    
            FROM    claim_peril    
            WHERE   claim_peril_id = @perilid    
    
        ELSE    
            SELECT @suminsured=0    
    
        INSERT INTO reserve    
        (    
         claim_peril_id,    
         reserve_type_id,    
         sum_insured,    
 	 version_id  ,  
  Initial_reserve,  
  Paid_to_date,  
  Revised_reserve,  
  Revision_count,  
  Average,  
  this_revision,  
  this_payment  
        )    
        VALUES    
        (    
         @perilid,    
         @reservetypeid,    
         @suminsured,    
    @version_id,  
  0,  
  0,  
  0,  
  0,  
   0,  
  0,  
  0   
        )    
    END    
   
    
-- get the reserve id    
SELECT @reserveId = @@IDENTITY    
    
-- update reserves base_reserve_id    
UPDATE reserve    
SET base_reserve_id = @reserveid    
WHERE reserve_id = @reserveid    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
