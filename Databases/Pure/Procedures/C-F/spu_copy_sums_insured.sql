if exists (select * from sysobjects where id = object_id(N'[dbo].[spu_copy_sums_insured]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_copy_sums_insured]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spu_copy_sums_insured
                    @old_policy_link_id int,
                    @new_policy_link_id int

AS

DECLARE @SQL VARCHAR(255),
        @data_model VARCHAR(20)

SELECT @data_model = LTRIM(RTRIM(gdm.code))
FROM    gis_policy_link gpl,
        risk r,
        gis_screen gs,
        gis_data_model gdm
WHERE   gpl.gis_policy_link_id = @old_policy_link_id
AND     gpl.risk_id = r.risk_cnt
AND     r.gis_screen_id = gs.gis_screen_id
AND     gs.gis_data_model_id = gdm.gis_data_model_id

SELECT  @SQL = 'spg_' + @data_model + '_copy_sums_insured'
             + ' @old_policy_link_id = ' + convert(varchar(20), @old_policy_link_id)
             + ', @new_policy_link_id = ' + convert(varchar(20), @new_policy_link_id)

EXEC (@SQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

