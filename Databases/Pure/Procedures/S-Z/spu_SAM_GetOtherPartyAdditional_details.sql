SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetOtherPartyAdditional_details'
GO
--*******************************************************************
-- VKG  11/09/2006        Get values of name,description code
--******************************************************************* 
CREATE PROCEDURE spu_SAM_GetOtherPartyAdditional_details 
@v_lPartyCnt int 
AS  
BEGIN 

SELECT      pt.description,   pt.code,p.name, popt.code 'party_other_posting_type'
FROM party p
  	JOIN party_type pt
  	ON pt.party_type_id = p.party_type_id
  	LEFT OUTER JOIN party_other_posting_type popt
	ON popt.party_other_posting_type_id = pt.party_other_posting_type_id
WHERE party_cnt =@v_lPartyCnt
    
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
