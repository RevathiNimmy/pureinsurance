SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_FPLEDI_last_message_received'
GO


CREATE PROCEDURE spu_Get_FPLEDI_last_message_received
    @insurance_file_cnt INT
AS

SELECT p.con_edi_old_ver_no
FROM insurance_file i
INNER JOIN gis_policy_link g ON g.insurance_file_cnt = i.insurance_file_cnt
INNER JOIN FPLHEDI_Policy p ON p.FPLHEDI_policy_binder_id = g.gis_policy_link_id
WHERE i.insurance_file_cnt = @insurance_file_cnt

GO


