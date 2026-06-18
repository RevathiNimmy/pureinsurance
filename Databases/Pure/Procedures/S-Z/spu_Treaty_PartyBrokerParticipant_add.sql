SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_Treaty_PartyBrokerParticipant_add'
GO

CREATE PROCEDURE spu_Treaty_PartyBrokerParticipant_add  
    @participantontreaty_id int output,  
    @treaty_id int,  
    @treaty_party_id int,  
    @associated_party_cnt int,  
    @party_cnt int,
	@participant_percent numeric(11,6)
AS  
  
    Insert Into RIBrokerParticipantOnTreaty (  
            treaty_id,  
            treaty_party_id,  
            associated_party_cnt,  
            party_cnt,
			participant_percent)  
    Values (@treaty_id,  
            @treaty_party_id,  
            @associated_party_cnt,  
            @party_cnt,
			@participant_percent)  
  
    Select @participantontreaty_id = @@identity

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
