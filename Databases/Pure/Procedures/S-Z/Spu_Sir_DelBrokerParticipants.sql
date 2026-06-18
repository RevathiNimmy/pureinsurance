SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_DelBrokerParticipants'
GO

Create Procedure Spu_Sir_DelBrokerParticipants
@Ri_arrangement_line_id int,
@PartyCnt int,
@ProcessId as int
As

If @ProcessId = 1 -- Policy Risk
Begin
  if @PartyCnt > 0  	
	  Delete from Ri_arrangement_line_Broker_Participants 
	  Where ri_arrangement_line_id=@Ri_arrangement_line_id and ri_party_cnt=@PartyCnt
  else
	  Delete from Ri_arrangement_line_Broker_Participants 
	  Where ri_arrangement_line_id=@Ri_arrangement_line_id
End

If @ProcessId = 2 -- Claims 
Begin
  if @PartyCnt > 0  	
	  Delete from Claim_ri_arrangement_line_Broker_Participants 
	  Where claim_ri_arrangement_line_id=@Ri_arrangement_line_id and ri_party_cnt=@PartyCnt
  else
	  Delete from Claim_ri_arrangement_line_Broker_Participants 
	  Where claim_ri_arrangement_line_id=@Ri_arrangement_line_id
End

Go