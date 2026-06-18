SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetOtherParty_details'
GO
--************************************************************************************************************
-- VKG  11/09/2006        Get values of active_indicator, after_hours_indicator,priority_indicator other party
--*************************************************************************************************************
CREATE PROCEDURE spu_SAM_GetOtherParty_details 
@v_lPartyCnt int 
AS  
BEGIN 

SELECT active_indicator,after_hours_indicator,priority_indicator,isnull(is_TPA_settle_directly,0) FROM Party_Other
WHERE party_cnt = @v_lPartyCnt
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
