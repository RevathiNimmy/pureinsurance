SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_GetBrokerParticipants'
GO

Create Procedure Spu_Sir_GetBrokerParticipants
@Ri_arrangement_line_id int,
@ProcessId as int
As

If @ProcessId = 1 -- Policy Risk
Begin
  Select shortname, name, participation_percent , ri_party_cnt
  From ri_arrangement_line_Broker_Participants Rbp
  Inner Join Party On Rbp.ri_party_cnt=party.party_cnt	 
  Where ri_arrangement_line_id = @Ri_arrangement_line_id	 
End
If @ProcessId = 2 -- Claims 
Begin
  Select shortname, name, participation_percent , ri_party_cnt
  From Claim_ri_arrangement_line_Broker_Participants Crbp
  Inner Join Party On Crbp.ri_party_cnt=party.party_cnt	 
  Where claim_ri_arrangement_line_id = @Ri_arrangement_line_id	 
End

Go