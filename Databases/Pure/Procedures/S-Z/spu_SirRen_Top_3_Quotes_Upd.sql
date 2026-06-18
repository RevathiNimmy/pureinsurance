SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Top_3_Quotes_Upd'
GO

CREATE PROCEDURE spu_SirRen_Top_3_Quotes_Upd
    @policy_link_id int
AS

/*****************************************************************************
* Sets a flag on the top 3 quotes (in terms of price) for a passed
* policy link id
* History: IJM 15082001 - Created
******************************************************************************/
DECLARE @quote_binder_id int
DECLARE @cntr int

DECLARE q_cursor CURSOR FOR
    SELECT qb.quote_binder_id
    FROM GIIMQuick_Quote_Result gqr,
            Quote_Binder qb,
            gis_policy_link l
    WHERE qb.gis_policy_link_id = @policy_link_id
    AND qb.Quote_Binder_id = gqr.Quote_Binder_id
    AND qb.Gis_Policy_Link_Id = gqr.Gis_Policy_Link_id
    AND l.gis_policy_link_id = qb.gis_policy_link_id
    AND l.gis_scheme_id <> qb.gis_scheme_id
    ORDER BY gqr.premium ASC

SET @cntr = 0

OPEN q_cursor

FETCH NEXT FROM q_cursor
    INTO @quote_binder_id
WHILE @@FETCH_STATUS = 0 AND @cntr < 3
BEGIN

    UPDATE Quote_Binder
    SET Preferred_Ind = 1
    WHERE quote_binder_id = @quote_binder_id
    AND gis_policy_link_id = @policy_link_id

    FETCH NEXT FROM q_cursor
        INTO @quote_binder_id

    SET @cntr = @cntr + 1

END

CLOSE q_cursor
DEALLOCATE q_cursor
GO

