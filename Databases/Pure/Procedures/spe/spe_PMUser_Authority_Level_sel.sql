SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Authority_Level_sel'
GO

/*************************************************************************/
/* 1.0  20/12/2000  RWH  Original (Based on Original by SP)  */
/*                             Updated manually to link to Product and   */
/*                             Authority_Level_Type to retrieve descriptions. */
/*************************************************************************/
CREATE PROCEDURE spe_PMUser_Authority_Level_sel
    @user_id int
AS

SELECT
    p.product_id,
    p.description,
    alt.authority_level_type_id,
    alt.description,
    alt.code	

 FROM PMUser_Authority_Level ual,
            Product p,
            Authority_Level_Type alt

WHERE ual.user_id = @user_id
               AND ual.product_id = p.product_id
      AND ual.authority_level_type_id = alt.authority_level_type_id

GO

