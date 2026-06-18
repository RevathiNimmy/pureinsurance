SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_reserve_details
GO

CREATE PROCEDURE spu_get_reserve_details
    @perilid INT,
    @siriusproduct CHAR(5),  
    @policyid NUMERIC,  
    @riskid NUMERIC  
AS  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--*******************************************************************************************  
  
--DC121201 added revised_reserve_entered, for Broking use only, as will be possible for the revised reserve figure to be zeroised  
--      so need a flag that a revised reserve amount had previously been entered, so no confusion with initial reserve  
--RWH(08/02/2001) Removed mode as it is not used.  
-- @mode integer  
BEGIN

DECLARE @reserveid INT ,@reservetypeid INT,@periltypeid INT  
DECLARE @initialreserve CURRENCY,@paidtodate CURRENCY,@revisedreserve CURRENCY,@suminsured CURRENCY  
DECLARE @average CURRENCY, @revisioncount INT
DECLARE @row NUMERIC
  
DECLARE @revisedentered TINYINT
  

BEGIN  
    SELECT @periltypeid = peril_type_id 
    FROM   claim_peril 
    WHERE  claim_peril_id = @perilid  
  
    SELECT @row = COUNT(*)
    FROM Reserve 
    WHERE claim_peril_id = @perilid  
  
    DECLARE reserve_details_cursor CURSOR FAST_FORWARD FOR  
        SELECT  wr.Reserve_id,
                wr.Initial_reserve,  
                wr.Paid_to_date,  
                wr.Revised_reserve,  
                wr.Sum_insured,  
                wr.Average,  
                wr.Revision_count,  
                rt.Reserve_type_id  
        FROM    Reserve wr
            RIGHT OUTER JOIN Reserve_type rt  
                ON wr.reserve_type_id = rt.reserve_type_id 
                AND wr.claim_peril_id = @perilid         
        WHERE   
            rt.reserve_type_id IN (  
                                   SELECT reserve_type_id 
                                   FROM peril_type_reserve_type 
                                   WHERE peril_type_id = @periltypeid
                                  )
  
        OPEN reserve_details_cursor  
        FETCH NEXT FROM reserve_details_cursor  
                    INTO    @reserveid,  
                            @initialreserve,  
                            @paidtodate,  
                            @revisedreserve,  
                            @suminsured,  
                            @average,  
                            @revisioncount,  
                            @reservetypeid  
  
        WHILE @@FETCH_STATUS = 0  
        BEGIN  
            IF @reserveid IS NULL
                  EXEC spu_add_reserve  @reserveid,
                                        @perilid,
                                        @reservetypeid,
                                        @siriusproduct,  
                                        @policyid,
                                        @riskid,
                                        @periltypeid  
  
            FETCH NEXT FROM reserve_details_cursor  
                INTO @reserveid,  
                     @initialreserve,  
                     @paidtodate,  
                     @revisedreserve,  
                     @suminsured,  
                     @average,  
                     @revisioncount,  
                     @reservetypeid  
        END  
  
        CLOSE reserve_details_cursor  
        DEALLOCATE reserve_details_cursor  
  
        SELECT  wr.Reserve_id,  
                wr.Initial_reserve,  
                wr.Paid_to_date,  
                wr.Revised_reserve,  
                wr.Sum_insured,  
                wr.Average,  
                wr.Revision_count,  
                rt.Reserve_type_id ,
				wr.this_revision,
				ISNULL((SELECT TOP 1  ISNULL(wrl.Revised_reserve,0)  FROM Reserve AS wrl WHERE wrl.base_reserve_id = wr.base_reserve_id and wrl.version_id < wr.version_id ORDER BY wrl.version_id DESC ),0) AS Previous_Version_Revised_Reserve
        FROM
            Reserve wr			
            RIGHT OUTER JOIN Reserve_type rt  
                ON wr.reserve_type_id = rt.reserve_type_id  
                AND wr.claim_peril_id = @perilid  
        WHERE   
            rt.reserve_type_id IN (
                                   SELECT reserve_type_id 
                                   FROM peril_type_reserve_type
                                   WHERE peril_type_id = @periltypeid  
                                  )   
      END  
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO