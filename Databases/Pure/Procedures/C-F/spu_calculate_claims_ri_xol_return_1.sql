SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_calculate_claims_ri_xol_return_1'
GO


CREATE PROCEDURE spu_calculate_claims_ri_xol_return_1
    @claim_id int,
    @ri_arrangement_id int, 
    @total_reserve money,
    @original_reserve money, 
    @original_payment money, 
    @revised_reserve money, 
    @revised_payment money
AS  
  
    -- Not implemented, time constraints on XOL development not sufficient to allow
    -- complex processing code to be completed and tested. Option also disabled in
    -- ri model maintenance


Go