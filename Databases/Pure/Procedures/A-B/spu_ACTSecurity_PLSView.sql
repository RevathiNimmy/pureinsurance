SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACTSecurity_PLSView'
GO
CREATE PROCEDURE spu_ACTSecurity_PLSView
    @node_id INT,
    @Key INT
AS

INSERT INTO
    PMUser_Group_Authorities (node_id, pmuser_group_id, Has_unrestricted_enquiry, Has_unrestricted_update)
VALUES
    (@node_id, @Key, 1, 0)
GO

