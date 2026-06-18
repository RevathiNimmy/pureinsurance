SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SAM_Get_Products_For_Branch'
GO

CREATE PROCEDURE spu_SAM_Get_Products_For_Branch  
@user_id INT,
@AgentKey INT=0  
AS  
SELECT product_id, code,description  
FROM product   
WHERE product_id in (SELECT distinct product_id from Product_source where source_id not in  
                                      (select distinct Source_id from PMUser_Source pus LEFT OUTER JOIN PMUSER pu ON pus.user_id= pu.user_id where pu.user_id=@user_id  AND (pu.party_cnt=@AgentKey OR @AgentKey=0)))
  
Order by code

Go