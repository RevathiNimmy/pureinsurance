SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Check_treaty_retained_party'
GO

CREATE PROCEDURE spu_Check_treaty_retained_party  
    @treaty_id int  
AS  
  
    Select  p.party_cnt,
			i.is_retained  
    From    treaty_party tp  
    Join    party p  
            On p.party_cnt = tp.party_cnt  
    Left Join  
            party_insurer i  
            On i.party_cnt = p.party_cnt  
    Where   tp.treaty_id = @treaty_id and i.is_retained=1  
