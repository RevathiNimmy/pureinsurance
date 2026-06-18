EXECUTE DDLDropProcedure 'spu_Get_Scheme_Product_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Scheme_Product_List
    @Source_id int = 0

--
-- ED03072002 select from all of these tables because they will cut the list down to only
--            show products that have an insurer + linked product + scheme etc...
--
-- RDT22012003 Added the Gis Screen Code to the select list for use in Agents Online
-- Tracy Richards 04/04/2004 - Get Commission Account for this Risk_group (need SourceID passed in for this)

AS

--IF @Source_id = null
--    SELECT @Source_id = 0

SELECT DISTINCT isnull(max(gis_insurer.party_cnt),MAX(party.party_cnt)) party_cnt,
       risk_group.risk_group_id,
       risk_group.description,
       risk_group.gis_screen_id,
       null,
       gdm.code,
       risk_group.code,
       gs.code,
       rbs.commission_cnt,
       gq.code

  FROM risk_group

       INNER JOIN gis_qem_usage gqu ON risk_group.risk_group_id = gqu.risk_group_id
       INNER JOIN gis_qem gq ON gq.gis_qem_id = gqu.gis_qem_id
       INNER JOIN gis_scheme ON gqu.gis_scheme_id = gis_scheme.gis_scheme_id
       INNER JOIN gis_insurer ON gis_insurer.gis_insurer_id = gis_scheme.gis_insurer_id
       INNER JOIN party ON gis_insurer.abi_81_insurer = party.abi_code_on_81
       INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = gqu.gis_data_model_id
       INNER JOIN gis_screen gs ON gs.gis_screen_id = risk_group.gis_screen_id
       LEFT OUTER JOIN Risk_By_Source rbs ON rbs.risk_group_id = risk_group.risk_group_id
		AND
		(
			(
				rbs.source_id = @source_id
				AND
				EXISTS (SELECT NULL FROM Risk_By_Source WHERE risk_group_id = risk_group.risk_group_id AND source_id = @source_id)
			)
			OR
			(
				rbs.source_id = 0
				AND
				EXISTS (SELECT NULL FROM Risk_By_Source WHERE risk_group_id = risk_group.risk_group_id AND source_id = 0)
				AND
				NOT EXISTS (SELECT NULL FROM Risk_By_Source WHERE risk_group_id = risk_group.risk_group_id AND source_id = @source_id)
			)
		)

 GROUP BY risk_group.risk_group_id,
          risk_group.description,
          risk_group.gis_screen_id,
          gdm.code,
          risk_group.code,
          gs.code,
          rbs.commission_cnt,
          gq.code
 ORDER BY risk_group.description


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

