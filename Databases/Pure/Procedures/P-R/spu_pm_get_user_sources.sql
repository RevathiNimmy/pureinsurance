SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_pm_get_user_sources'
GO

CREATE PROCEDURE spu_pm_get_user_sources  
    @UserID SMALLINT,
    @IncludeClosed tinyint=0,
	@AgentKey INT=0	
AS  
BEGIN  
  
   SELECT source.source_id, [description], country_id, source.code,
		   party.shortname ,
    CASE   WHEN Source_Defaults.direct_business = 0 THEN 'AGENCY'
	   WHEN Source_Defaults.direct_business = 1 THEN 'DIRECT'
    ELSE '' END 'direct_business',
	party.party_cnt agentkey
    FROM source
		LEFT JOIN Source_Defaults on Source_Defaults.source_id = source.source_id 
	LEFT JOIN party  on party.party_cnt = Source_Defaults.agent_id 
    WHERE (source.is_deleted <> 1 OR @IncludeClosed=1)
      AND source.source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] = @UserID)
      AND((Source_Defaults.direct_business = 0 AND party.party_cnt=@AgentKey) OR (@AgentKey=0))
END  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
