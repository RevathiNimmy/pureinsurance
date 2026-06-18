SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Claim_RI_Arrangement_Line_add'
GO

CREATE PROCEDURE spu_SAM_Claim_RI_Arrangement_Line_add  
    @ri_arrangement_line_id int output,  
    @claim_id int,  
    @ri_arrangement_id int,  
    @type varchar(2),  
    @treaty_id int,  
    @party_cnt int,  
    @xol_arrangement_id int,  
    @default_share_percent float,  
    @this_share_percent float,  
    @agreement_code varchar(255),  
    @priority int,  
    @number_of_lines smallint,  
    @line_limit money,  
    @sum_insured money,  
    @reserve money,  
    @payment money,  
    @this_reserve money,  
    @this_payment money,
    @lower_limit money = 0,        
    @retained float = 0,
    @participation_percent float = 0  
AS  
  
 DECLARE @version_id int  
 DECLARE @claim_ri_arrangement_line_id int  
  
 EXEC spu_CLM_Get_Claim_version  
  @claim_id = @claim_id,  
  @version_id = @version_id OUTPUT  
  
 -- this value is just an initial default it is overriden by the update further down
 -- Get new id (we don't have an identity column in claims)  
 SELECT  @ri_arrangement_line_id = ISNULL(MAX(ri_arrangement_line_id), 0) + 1  
 FROM    claim_ri_arrangement_line  
 WHERE   claim_id = @claim_id  
 
 -- Insert record  
 INSERT  claim_ri_arrangement_line (  
  claim_id,  
  ri_arrangement_line_id,  
  ri_arrangement_id,  
  type,  
  treaty_id,  
  party_cnt,  
  xol_arrangement_id,  
  default_share_percent,  
  this_share_percent,  
  agreement_code,  
  priority,  
  number_of_lines,  
  line_limit,  
  sum_insured,  
  reserve,  
  payment,  
  salvage,  
  recovery,  
  this_reserve,  
  this_payment,  
  this_salvage,  
  this_recovery,  
  version_id,
  lower_limit,
  retained,
  participation_percent)
 VALUES (  
  @claim_id,  
  @ri_arrangement_line_id,  
  @ri_arrangement_id,  
  @type,  
  @treaty_id,  
  @party_cnt,  
  @xol_arrangement_id,  
  @default_share_percent,  
  @this_share_percent,  
  @agreement_code,  
  @priority,  
  @number_of_lines,  
  @line_limit,  
  @sum_insured,  
  @reserve,  
  @payment,  
  0, 0,  
  @this_reserve,  
  @this_payment,  
  0, 0,  
  @version_id,
  @lower_limit,      
  @retained,
  @participation_percent)  
  
 SELECT @claim_ri_arrangement_line_id = @@IDENTITY  
  
 UPDATE claim_ri_arrangement_line  
 SET base_claim_ri_arrangement_line_id = @claim_ri_arrangement_line_id ,
     ri_arrangement_line_id = @claim_ri_arrangement_line_id 
 WHERE claim_ri_arrangement_line_id = @claim_ri_arrangement_line_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

