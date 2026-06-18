SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_UpdateProrate_Utility'

GO
CREATE PROCEDURE spu_UpdateProrate_Utility  
   
    @risk_cnt int  
AS  

DECLARE  @pro_rate float

if exists ( SELECT risk_cnt, pro_rata_rate   FROM risk  WHERE   risk_cnt in(select distinct risk_cnt from Rating_Section  where abs(annual_premium)<>ABS(this_premium) )
and pro_rata_rate=1  and risk_cnt=@risk_cnt)
begin

select top 1 @pro_rate= abs(annual_premium)  from Rating_Section where risk_cnt= @risk_cnt  and abs(annual_premium)<>ABS(this_premium)

IF ABS(@pro_rate) <> 0
begiN

SET @pro_rate=1
select top 1 @pro_rate= ABS(this_premium)/abs(annual_premium)  from Rating_Section where risk_cnt= @risk_cnt 
and  abs(annual_premium)<>ABS(this_premium) 
update Risk set pro_rata_rate=@pro_rate where risk_cnt=@risk_cnt
end 

end
