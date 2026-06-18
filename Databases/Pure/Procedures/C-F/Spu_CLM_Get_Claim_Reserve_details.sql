
EXECUTE DDLDropProcedure spu_CLM_Get_Claim_Reserve_details
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Reserve_details
    @claim_peril_id INT,  
    @siriusproduct CHAR(5),  
    @insurance_file_cnt INT  
  
AS
BEGIN
  
    DECLARE @risk_cnt INT
      
    DECLARE @reserveid INT ,@reservetypeid INT,@periltypeid INT
    DECLARE @initialreserve CURRENCY,@paidtodate CURRENCY,@revisedreserve CURRENCY,@suminsured CURRENCY
    DECLARE @average CURRENCY, @revisioncount INT
    DECLARE @row NUMERIC
      
    BEGIN  

        SELECT @risk_cnt=risk_cnt
        FROM insurance_file_risk_link 
        WHERE insurance_file_cnt = @insurance_file_cnt  
        
        SELECT @periltypeid = peril_type_id 
        FROM claim_peril 
        WHERE claim_peril_id = @claim_peril_id  
        
        SELECT @row = COUNT(*) 
        FROM Reserve 
        WHERE claim_peril_id = @claim_peril_id  
        
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
                    AND wr.claim_peril_id = @claim_peril_id
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
        
                EXEC spu_add_reserve @reserveid,@claim_peril_id,@reservetypeid,@siriusproduct,  
                                              @insurance_file_cnt,@risk_cnt,@periltypeid  
        
            FETCH NEXT FROM reserve_details_cursor  
                INTO    @reserveid,  
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
                rt.description ,  
                wr.Initial_reserve,  
                wr.Revised_reserve,  
                wr.Revision_count,  
                wr.this_revision,  
                wr.this_payment,  
                wr.Paid_to_date,  
                rt.Is_Excess,  
                rt.Is_Indemnity,  
                rt.Is_Expense,
                rt.Reserve_type_id,  
                0 as 'IsUpdated',
				wr.Sum_Insured  

        FROM    Reserve wr
            RIGHT OUTER JOIN Reserve_type rt  
                ON wr.reserve_type_id = rt.reserve_type_id
                AND wr.claim_peril_id = @claim_peril_id
        WHERE
            rt.reserve_type_id IN (
                                   SELECT reserve_type_id
                                   FROM peril_type_reserve_type
                                   WHERE peril_type_id = @periltypeid
                                  )

    END

END
GO
