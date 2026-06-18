SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACTSecurity_AccountRights'
GO

CREATE PROCEDURE spu_ACTSecurity_AccountRights
(
    @Account_id INT,
    @PMUser_id SMALLINT,
    @Has_Unrestricted_Enquiry TINYINT OUTPUT,
    @Has_Unrestricted_Update TINYINT OUTPUT
)
AS

SELECT
    @Has_Unrestricted_Enquiry=MAX(pga.Has_unrestricted_enquiry),
    @Has_Unrestricted_Update=MAX(pga.Has_unrestricted_update)
FROM
    PMUser_Group_Authorities pga
INNER JOIN
    StructureTree st ON st.parent_node_id=pga.node_id
INNER JOIN
    PMUser_Group_User pmu ON pga.pmuser_group_id=pmu.pmuser_group_id
WHERE
    st.account_id=@Account_id
AND pmu.user_id=@PMUser_id

IF @Has_Unrestricted_Enquiry IS NULL
BEGIN
    SELECT @Has_Unrestricted_Enquiry=0
    SELECT @Has_Unrestricted_Update=0
END
GO
