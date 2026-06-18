SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_STS_sel'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_STS_sel
    @sExternalSchemeNo VARCHAR(6),
    @dtPolicyStartDate DATETIME,
    @source_id INTEGER
AS

--********************************************************************************************************
--* Stored Procedure spu_GIS_Scheme_EDI_Link_STS_sel returns the scheme_id (and related insurer          *
--* information) to the STS layer based upon the external scheme identifier and policy start date passed *
--* in.	If more than one current scheme version is linked then the highest version will be returned.	 *
--********************************************************************************************************

SELECT  TOP 1 e.gis_scheme_id, 
              s.gis_insurer_id, 
              u.risk_group_id, 
              c.risk_code_id, 
              p.party_cnt, 
              s.scheme_desc, 
              i.description as [insurer_desc]
FROM    gis_scheme_edi_link e
INNER JOIN gis_scheme s ON s.gis_scheme_id = e.gis_scheme_id
INNER JOIN gis_insurer i ON i.gis_insurer_id = s.gis_insurer_id
INNER JOIN gis_qem_usage u ON u.gis_scheme_id = s.gis_scheme_id
INNER JOIN risk_code c ON c.risk_group_id = u.risk_group_id
INNER JOIN party p ON p.abi_code_on_81 = i.abi_81_insurer
WHERE   e.external_scheme_no = @sExternalSchemeNo
AND     s.start_date <= getdate()
AND     s.expiry_date >= getdate()
AND     p.source_id = @source_id
AND     c.is_deleted = 0
ORDER BY s.scheme_ver DESC
GO