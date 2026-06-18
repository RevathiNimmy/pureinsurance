SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PMUser_Auth_Rule_Link_saa'
GO

CREATE PROCEDURE spu_PMUser_Auth_Rule_Link_saa
AS
/*************************************************************************/
/* 1.0  02/01/2000  RWH  Original (Based on Original by SP)        */
/*************************************************************************/
SELECT
    arsl.rule_set_id,
    rs.caption_id,
    rs.code,
    rs.description AS rule_set_desc,
    rs.effective_date,
    rs.file_name,
    rs.live,
    arsl.authority_level_type_id,
    arsl.is_underwriter,
    arsl.product_id,
    arsl.transaction_type_id,
    alt.description AS auth_level_desc,
    p.description AS product_desc,
    t.description
FROM PMUser_Authority_Rule_Set_Link arsl,
     Product p,
     Authority_Level_Type alt,
     Rule_Set rs,
     transaction_type t

WHERE p.product_id = arsl.product_id
      AND rs.rule_set_id = arsl.rule_set_id
      AND alt.authority_level_type_id = arsl.authority_level_type_id
      AND t.transaction_type_id = arsl.transaction_type_id
GO

