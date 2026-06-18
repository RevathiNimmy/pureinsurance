SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_peril_totals'
GO

CREATE PROCEDURE spu_get_peril_totals  

    @claim_id int  

AS  
  


--*******************************************************************************************  

-- Version      Author  Date        Desc  

-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  

--  

--*******************************************************************************************  

DECLARE @AgentUnderwriter varchar(1)  
  


SELECT  @AgentUnderwriter = value  

FROM    hidden_options  

WHERE   branch_id = 1 and option_number = 1  
  


IF @AgentUnderwriter is null  

    SELECT @AgentUnderwriter = 'A'  
  


IF @AgentUnderwriter = ''  

    SELECT @AgentUnderwriter = 'A'  
  


IF @AgentUnderwriter = 'A'  

	SELECT 

		Claim_Peril.claim_peril_id, 

		Claim_Peril.Description,  

		SUM(Reserve.Initial_reserve) AS IR,  

		SUM(Reserve.Paid_to_date) AS PTD,  

		SUM(Reserve.Revised_reserve) AS RR,  

		SUM(Reserve.Initial_reserve) - SUM(Reserve.Paid_to_date)  

		+ SUM(Reserve.Revised_reserve) AS Currres,  

		SUM(Reserve.Sum_insured) AS SI,

       Reserve.Average AS Average

	FROM Claim_Peril 

		INNER JOIN  Reserve ON  

			Claim_Peril.claim_peril_id = Reserve.claim_peril_id 

		INNER JOIN  Reserve_type ON  

			Reserve.Reserve_type_id = Reserve_type.Reserve_type_id  

	WHERE Claim_Peril.Claim_id = @claim_id

	AND Reserve_type.Include_in_Total = 1  

	GROUP BY 	

		Claim_Peril.claim_peril_id, 

		Claim_Peril.Description,
		Reserve.Average  

ELSE  

--UNDERWRITING  

	SELECT  

		wcp.claim_peril_id,  

		wcp.Description,  

		SUM(wr.Initial_reserve) AS IR,  

		SUM(wr.Paid_to_date) AS PTD,  

		SUM(wr.Revised_reserve) AS RR,  

		SUM(wr.Initial_reserve) - SUM(wr.Paid_to_date) + SUM(wr.Revised_reserve) AS Currres,  

		SUM(wr.Sum_insured) AS SI,

	   Wr.Average AS Average

	FROM    Claim_Peril wcp 

		INNER JOIN Reserve wr ON  

			wcp.claim_peril_id = wr.claim_peril_id 

		INNER JOIN Reserve_type rt ON  

			wr.Reserve_type_id = rt.Reserve_type_id  

	WHERE   wcp.Claim_id = @claim_id  

	AND     rt.Include_in_Total = 1  

	GROUP BY 

		wcp.claim_peril_id, 

		wcp.Description,
		Wr.Average 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
