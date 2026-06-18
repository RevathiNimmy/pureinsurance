SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_payment_details
GO

CREATE PROCEDURE spu_get_payment_details
    @perilid INT,  
    @claimid INT,  
    @CreatedBy INT
AS
BEGIN 
  
DECLARE @paymentid INT  
DECLARE @reserveid INT
DECLARE @amount CURRENCY
DECLARE @actrow INT 
DECLARE @fullrow INT
DECLARE @periltypeid NUMERIC
DECLARE @AgentUnderwriter VARCHAR(1)  

SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 
    AND option_number = 1  

IF @AgentUnderwriter IS NULL
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = 'A' 
BEGIN  
    /*Get Peril Type Id */  
    SELECT @periltypeid=peril_type_id 
    FROM claim_peril 
    WHERE claim_peril_id=@perilid  
    /* Count the number of reserves attached to the peril where the reserve type is assoacited with the peril type*/  
    SELECT @fullrow=count(*)  
    FROM reserve
        RIGHT OUTER JOIN reserve_type 
            ON reserve.reserve_type_id= reserve_type.reserve_type_id 
            AND claim_peril_id=@perilid  
    WHERE reserve_type.reserve_type_id IN (
                                             SELECT reserve_type_id 
                                             FROM   peril_type_reserve_type 
                                             WHERE  peril_type_id =@periltypeid
                                            )
  
    DECLARE payment_details_cursor CURSOR FAST_FORWARD FOR  
        SELECT reserve.reserve_id 
        FROM reserve
            RIGHT OUTER JOIN reserve_type 
                ON reserve.reserve_type_id= reserve_type.reserve_type_id
                AND claim_peril_id=@perilid
        WHERE  
            reserve_type.reserve_type_id IN (
                                                 SELECT reserve_type_id 
                                                 FROM   peril_type_reserve_type 
                                                 WHERE  peril_type_id =@periltypeid
                                                )  
        OPEN payment_details_cursor  
        FETCH NEXT FROM payment_details_cursor  
        INTO @reserveid  
        
        WHILE @@FETCH_STATUS = 0  
        BEGIN  
            EXEC spu_add_payment 0,@reserveid,@perilid,@claimid,@CreatedBy  
            FETCH NEXT FROM payment_details_cursor  
            INTO @reserveid  
        END  
        
        CLOSE payment_details_cursor  
        DEALLOCATE payment_details_cursor  
        
        SELECT @paymentid=MAX(claim_payment_id) 
        FROM   Claim_Payment  
        SELECT Claim_Payment_id,amount 
        FROM   Claim_Payment 
        WHERE  claim_peril_id=@perilid 
            AND claim_payment_id BETWEEN (@paymentid -@fullrow+1)  
            AND @paymentid  
    END  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO