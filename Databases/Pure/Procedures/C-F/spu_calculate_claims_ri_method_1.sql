SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_calculate_claims_ri_method_1'
GO


CREATE PROCEDURE spu_calculate_claims_ri_method_1  
    @claim_id int,
    @ri_arrangement_id int, 
    @reserve money, 
    @payment money
AS  
  
    -- Not implemented, time constraints on XOL development not sufficient to allow
    -- complex processing code to be completed and tested. Option also disabled in
    -- ri model maintenance


Go