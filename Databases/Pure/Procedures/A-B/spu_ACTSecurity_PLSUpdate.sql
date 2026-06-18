SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACTSecurity_PLSUpdate'
GO
CREATE PROCEDURE spu_ACTSecurity_PLSUpdate
    @node_id INT,
    @Key INT
AS

IF EXISTS(SELECT 1 FROM PMUser_Group_Authorities 
    WHERE node_id = @node_id
    AND pmuser_group_id = @Key)

    UPDATE PMUser_Group_Authorities
        SET Has_unrestricted_update = 1
        WHERE node_id = @node_id
        AND pmuser_group_id = @Key

ELSE
    INSERT INTO
        PMUser_Group_Authorities (node_id, pmuser_group_id, Has_unrestricted_enquiry, Has_unrestricted_update)
    VALUES
        (@node_id, @Key, 1, 1)
GO

