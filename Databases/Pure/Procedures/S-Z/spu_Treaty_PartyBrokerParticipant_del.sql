SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Treaty_PartyBrokerParticipant_del'
GO

CREATE PROCEDURE spu_Treaty_PartyBrokerParticipant_del  
    @treaty_party_id int = null,  
    @treaty_id int = null  
AS  
       
        Delete From RIBrokerParticipantOnTreaty  
        Where treaty_party_id=@treaty_party_id
			And treaty_id = @treaty_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
