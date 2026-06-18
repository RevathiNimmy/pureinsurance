SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_prospect_sel'
GO

CREATE PROCEDURE spe_party_prospect_sel
    @party_cnt int
AS
SELECT
    pp.party_cnt,    
    pp.agent_reference,    
    pp.current_intermediary,    
    pp.prospect_status_id,    
    pp.Strength_code_id,    
    pp.previous_insurer_cnt,    
    pp.previous_broker_cnt,  
 --Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)  
    p1.resolved_name as 'currentintermediary_resolved_name',  
 p2.shortname as 'previous_insurer_shortname',  
 p2.resolved_name as 'previous_insurer_resolved_name',  
 p3.shortname as 'previous_broker_shortname',  
 p3.resolved_name as 'previous_broker_resolved_name'  
 FROM party_prospect pp  
 left join party p1 on pp.current_intermediary=p1.party_cnt  
 left join party p2 on pp.previous_insurer_cnt=p2.party_cnt  
 left join party p3 on pp.previous_broker_cnt=p3.party_cnt  
 --End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)  
 WHERE pp.party_cnt = @party_cnt   
