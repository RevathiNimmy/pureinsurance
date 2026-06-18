SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetCurrentReserveRecovery'
GO

CREATE PROCEDURE spu_GetCurrentReserveRecovery  
    @ClaimID integer  
AS  
Declare  
    @ClaimStatus integer,  
    -- 05/08/2003 Peter Finney - Use correct base datatype  
    @CurrentReserve money,  
    @CurrentRecovery money,  
    --@CurrentReserveInitial money,  
    --@CurrentReserveRevised money,  
    --@CurrentRecoveryInitial money,  
    --@CurrentRecoveryRevised money,  
    @hidden_option char(1)  
  
-- Check hidden options for  
select  @hidden_option = value  
from    hidden_options  
where   branch_id = 1  
and     option_number = 1  
  
-- If underwriting  
IF @hidden_option = 'U'  
BEGIN  
  -- Get claim status  
  select  @ClaimStatus = Claim_Status_id  
  from    Claim  
  where   Claim_Id = @ClaimID  
  
        SELECT  @CurrentReserve = sum(Initial_reserve + Revised_reserve - Paid_to_date)  
        FROM    reserve wr  
        JOIN    claim_peril wcp ON wr.claim_peril_id = wcp.claim_peril_id  
        AND     Claim_Id = @ClaimID  
  
        SELECT  @CurrentRecovery = sum(Initial_reserve + revised_reserve - received_to_date)  
        FROM    recovery wr  
        JOIN    claim_peril wcp ON wr.claim_peril_id = wcp.claim_peril_id  
        AND     Claim_Id = @ClaimID  
END  
ELSE  
BEGIN  
  
 -- Get claim status  
 select  @ClaimStatus = Claim_Status_id  
 from    Claim  
 where   Claim_Id = @ClaimID  
  
 CREATE TABLE    #Claim_Details  
 (  
  claimid        int,  
  currentreserve   numeric(19,4),  
        currentrecovery   numeric(19,4),  
  revisedreserveentered int,  
  paidtodate       numeric(19,4)  
 )  
 Insert Into #Claim_Details  
 (  
  claimid,  
  currentreserve,  
  currentrecovery,  
  revisedreserveentered,  
  paidtodate  
 )  
 Select  Claim.claim_Id,  
  ISNULL(Reserve.initial_reserve, 0) - ISNULL(Reserve.Paid_to_date, 0),  
  0,  
  Reserve.Revised_Reserve_Entered,  
  Reserve.Paid_To_Date  
 from reserve right outer join Claim_Peril  
 on reserve.claim_peril_id = Claim_Peril.claim_peril_id  
 and reserve.Reserve_type_id in  (select reserve_type_id  
     from reserve_type  
     where Include_in_Total = 1),  
 Claim, Peril_type  
 where Claim_Peril.Claim_ID = Claim.claim_Id  
 and Claim_Peril.Peril_Type_Id = Peril_type.Peril_Type_Id  
 and claim.Claim_Id = @ClaimId  
 and ( reserve.revised_reserve = 0 OR reserve.revised_reserve IS NULL )  
 and ( reserve.revised_reserve_entered = 0 OR reserve.revised_reserve_entered IS NULL)  
 and reserve.initial_reserve <> 0  
  
 Insert Into #Claim_Details  
 (  
  claimid,  
  currentreserve,  
                currentrecovery,  
  revisedreserveentered,  
  paidtodate  
 )  
 Select  Claim.claim_Id,  
         ((ISNULL(Recovery.initial_reserve, 0) + ISNULL(Recovery.Revised_Reserve, 0)) - ISNULL(Recovery.Received_To_Date,0)) * -1,  
  ((ISNULL(Recovery.initial_reserve, 0) + ISNULL(Recovery.Revised_Reserve, 0)) - ISNULL(Recovery.Received_To_Date,0)) * -1,  
  0,  
  0  
 from recovery  
 right outer join Claim_Peril  
 on recovery.claim_peril_id = Claim_Peril.claim_peril_id,  
 Claim, Peril_type  
 where Claim_Peril.Claim_ID = Claim.claim_Id  
 and Claim_Peril.Peril_Type_Id = Peril_type.Peril_Type_Id  
 and claim.Claim_Id = @ClaimId  
  
 Insert Into #Claim_Details  
 (  
  claimid,  
  currentreserve,  
  currentrecovery,  
  revisedreserveentered,  
  paidtodate  
 )  
 select  Claim.claim_Id,  
  ISNULL(Reserve.Revised_reserve,0) - ISNULL(Reserve.Paid_to_date, 0),  
  0,  
  Reserve.Revised_Reserve_Entered,  
  Reserve.Paid_To_Date  
 from reserve right outer join Claim_Peril  
 on reserve.claim_peril_id = Claim_Peril.claim_peril_id  
 and reserve.Reserve_type_id in  (select reserve_type_id  
     from reserve_type  
     where Include_in_Total = 1),  
 Claim, Peril_type  
 where Claim_Peril.Claim_ID = Claim.claim_Id  
 and Claim_Peril.Peril_Type_Id = Peril_Type.peril_type_id  
 and claim.Claim_Id = @ClaimId  
 and reserve.revised_reserve <> 0  
  
 Insert Into #Claim_Details  
 (  
  claimid,  
  currentreserve,  
  currentrecovery,  
  revisedreserveentered,  
  paidtodate  
 )  
 select  Claim.claim_Id,  
  0,  
  0,  
  0,  
  0  
 from reserve right outer join Claim_Peril  
 on reserve.claim_peril_id = Claim_Peril.claim_peril_id  
 and reserve.Reserve_type_id in  (select reserve_type_id  
     from reserve_type  
     where Include_in_Total = 1),  
 Claim, Peril_type  
 where Claim_Peril.Claim_ID = Claim.claim_Id  
 and Claim_Peril.Peril_Type_Id = Peril_Type.peril_type_id  
 and claim.Claim_Id = @ClaimId  
 and (reserve.revised_reserve = 0 OR reserve.revised_reserve IS NULL)  
 and (reserve.initial_reserve = 0 OR reserve.initial_reserve IS NULL)  
  
 Insert Into #Claim_Details  
 (  
  claimid,  
  currentreserve,  
  currentrecovery,  
  revisedreserveentered,  
  paidtodate  
 )  
 select  cp.claim_id,  
  0,  
  0,  
  0,  
  0  
 from claim c  
 join claim_peril cp  
 on c.claim_id = cp.claim_id  
 join peril_type pt  
 on pt.peril_type_id = cp.peril_type_id  
 where cp.claim_id = @claimid  
 and NOT EXISTS ( SELECT * FROM reserve WHERE claim_peril_id = cp.claim_peril_id  )  
  
 SET NOCOUNT OFF  
 select  @CurrentReserve = SUM(currentreserve),  
  @CurrentRecovery = SUM(currentrecovery)  
 from #Claim_Details  
 group by claimid  
 SET NOCOUNT ON  
 DELETE FROM #Claim_Details  
 DROP TABLE #Claim_Details  
  
END  
  
SELECT  @ClaimStatus,  
        @CurrentReserve,  
        @CurrentRecovery  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
