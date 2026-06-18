SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_Treaty_Party_BrokerParticipants_saa'
GO

CREATE PROCEDURE spu_Treaty_Party_BrokerParticipants_saa  
    @treaty_id int  
AS    
    Select  ribp.participantontreaty_id,  
			ribp.treaty_id,
			ribp.treaty_party_id,
			ribp.associated_party_cnt,
            ribp.party_cnt,
			ribp.participant_percent,
			p.shortname,
			p.name,
			ribp.treaty_party_id			
    From    treaty_party tp
    Join    RIBrokerParticipantOnTreaty ribp  
            On tp.treaty_id = ribp.treaty_id and tp.treaty_party_id=ribp.treaty_party_id
	Inner Join Party p ON p.party_cnt=ribp.associated_party_cnt
    Where   tp.treaty_id = @treaty_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
