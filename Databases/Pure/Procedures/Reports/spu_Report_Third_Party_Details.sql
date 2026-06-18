SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Third_Party_Details'
GO

CREATE PROCEDURE spu_Report_Third_Party_Details
	@third_party_type VARCHAR(25)
AS
	DECLARE @agent_type_id INT

SET NOCOUNT ON

	IF @third_party_type = 'ALL'
		BEGIN
    			SELECT @agent_type_id = NULL

		END
	ELSE
		BEGIN
    			SELECT @agent_type_id =   
			(
               		SELECT party_agent_type_id
                	FROM party_agent_type
                	WHERE [description] = @third_party_type
                	)
		END

CREATE TABLE #TempThirdParty
(
	shortcode VARCHAR(20),
	resolved_name VARCHAR(225),
	party_agent_type_id INT,
	description VARCHAR(225),
	agency_account_number VARCHAR(225),
	address1 VARCHAR(60),
	address2 VARCHAR(60),
	address3 VARCHAR(60),
	address4 VARCHAR(60),
	postal_code VARCHAR(20)
)
INSERT INTO #TempThirdParty
SELECT 
	P.shortname,
	P.resolved_name,
	PAT.party_agent_type_id,
	PAT.description,
	PA.agency_account_number,
	A.address1,
	A.address2,
	A.address3,
	A.address4,
	A.postal_code
FROM
	party P JOIN party_agent PA
	ON P.party_cnt=PA.party_cnt
	JOIN party_agent_type PAT
	ON PA.party_agent_type_id=PAT.party_agent_type_id
 	AND PAT.is_visible = 1  	
	LEFT OUTER JOIN Party_Address_Usage PAU
    	JOIN Address_Usage_Type AUT
        ON AUT.address_usage_type_id = PAU.address_usage_type_id
        JOIN Address A
        ON A.address_cnt = PAU.address_cnt
        ON PAU.party_cnt = P.party_cnt
        AND AUT.code = '3131 XCO'

SET NOCOUNT OFF

SELECT * FROM #TempThirdParty WHERE 
	((@agent_type_id <> 0 AND party_agent_type_id = @agent_type_id) OR (@agent_type_id IS NULL AND party_agent_type_id IS NOT NULL))
ORDER BY
	party_agent_type_id

DROP TABLE #TempThirdParty
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

