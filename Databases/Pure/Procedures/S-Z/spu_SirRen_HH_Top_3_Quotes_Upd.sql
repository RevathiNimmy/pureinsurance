SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SirRen_HH_Top_3_Quotes_Upd'
GO

CREATE PROCEDURE spu_SirRen_HH_Top_3_Quotes_Upd
    @policy_link_id int
AS

/*****************************************************************************
* Sets a flag on the top 3 quotes (in terms of price) for a passed
* policy link id
* History: SJ 14092001 - Created
******************************************************************************/
DECLARE @quote_binder_id int
DECLARE @cntr int

DECLARE q_cursor CURSOR FAST_FORWARD FOR
    SELECT qb.GIIHquote_binder_id
    FROM qh_quote_out gqr,
    GIIHQuote_Binder qb,
    gis_policy_link l
    WHERE qb.gis_policy_link_id = @policy_link_id
    AND qb.GIIHQuote_Binder_id = gqr.GIIHQuote_Binder_id
    AND qb.Gis_Policy_Link_Id = gqr.Gis_Policy_Link_id
    AND gqr.out_contents_flag IS NULL
    AND gqr.out_buildings_flag is NULL
    AND l.gis_policy_link_id = qb.gis_policy_link_id
    AND l.gis_scheme_id <> gqr.out_gis_scheme_id
    ORDER BY gqr.out_total_ann_premium ASC

SET @cntr = 0
OPEN q_cursor
FETCH NEXT FROM q_cursor INTO @quote_binder_id

WHILE @@FETCH_STATUS = 0 AND @cntr < 3 BEGIN
    UPDATE GIIHQuote_Binder
        SET Preferred_Ind = 1
        WHERE GIIHquote_binder_id = @quote_binder_id
        AND gis_policy_link_id = @policy_link_id

    FETCH NEXT FROM q_cursor INTO @quote_binder_id

    SET @cntr = @cntr + 1
END

CLOSE q_cursor
DEALLOCATE q_cursor
GO

