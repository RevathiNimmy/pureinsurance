SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_AddBrokerParticipants'
GO

Create Procedure Spu_Sir_AddBrokerParticipants
@Ri_arrangement_line_id int,
@PartyCnt int,
@Part_percent float,
@ProcessId as int
As

If @ProcessId = 1 -- Policy Risk
Begin
 DELETE from Ri_arrangement_line_Broker_Participants 
		  Where ri_arrangement_line_id=@Ri_arrangement_line_id and ri_party_cnt=@PartyCnt
  	Insert Into Ri_arrangement_line_Broker_Participants (Ri_arrangement_line_id,ri_party_cnt,participation_percent)
	Values (@Ri_arrangement_line_id,@PartyCnt,@Part_percent)
    	
End
If @ProcessId = 2 -- Claims 
Begin
  if not exists(select * from Claim_ri_arrangement_line_Broker_Participants 
		  Where claim_ri_arrangement_line_id=@Ri_arrangement_line_id and ri_party_cnt=@PartyCnt)
    Begin
	Insert Into Claim_ri_arrangement_line_Broker_Participants (claim_ri_arrangement_line_id,ri_party_cnt,participation_percent)
	Values (@Ri_arrangement_line_id,@PartyCnt,@Part_percent)
    End	

End

Go