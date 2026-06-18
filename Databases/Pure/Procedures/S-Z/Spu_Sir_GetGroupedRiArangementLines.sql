SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_GetGroupedRiArangementLines'
GO

Create Procedure Spu_Sir_GetGroupedRiArangementLines
@RiArrangementId int,
@GroupingId int,
@ProcessId int
As

if @ProcessId = 1
Begin 
SELECT DISTINCT   
 shortname,  
 name,  
 Acct_Type = CASE WHEN is_RI_Broker=1   
    THEN 'Broker'  
    WHEN is_Retained=1  
    THEN 'Retained'
    ELSE 'Reinsurer' END,   
 participation_percent,
 sum_insured,
 premium_value,
 premium_tax,
 commission_percent,
 commission_value,
 commission_tax,	   
 agreement_code,
 ral.party_cnt,
 ri_arrangement_line_id
 From ri_arrangement_line ral
 left Join Party on party.party_cnt = ral.party_cnt
 Join Party_insurer On ral.party_cnt = party_insurer.party_cnt    
 Where Party.is_deleted = 0  
 AND ri_arrangement_id=@RiArrangementId
 AND grouping =@GroupingId
Order by ri_arrangement_line_id 	
End
GO

